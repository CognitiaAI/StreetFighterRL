﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

using BizHawk.Common;

namespace BizHawk.Common.BizInvoke
{
	public static class BizInvoker
	{
		/// <summary>
		/// holds information about a proxy implementation, including type and setup hooks
		/// </summary>
		private class InvokerImpl
		{
			public Type ImplType;
			public List<Action<object, IImportResolver, ICallingConventionAdapter>> Hooks;
			public Action<object, IMonitor> ConnectMonitor;
			public Action<object, ICallingConventionAdapter> ConnectCallingConventionAdapter;

			public object Create(IImportResolver dll, IMonitor monitor, ICallingConventionAdapter adapter)
			{
				var ret = Activator.CreateInstance(ImplType);
				ConnectCallingConventionAdapter(ret, adapter);
				foreach (var f in Hooks)
				{
					f(ret, dll, adapter);
				}
				ConnectMonitor?.Invoke(ret, monitor);
				return ret;
			}
		}

		/// <summary>
		/// dictionary of all generated proxy implementations and their base types
		/// </summary>
		private static readonly IDictionary<Type, InvokerImpl> Impls = new Dictionary<Type, InvokerImpl>();

		/// <summary>
		/// the assembly that all proxies are placed in
		/// </summary>
		private static readonly AssemblyBuilder ImplAssemblyBuilder;

		/// <summary>
		/// the module that all proxies are placed in
		/// </summary>
		private static readonly ModuleBuilder ImplModuleBuilder;

		static BizInvoker()
		{
			var aname = new AssemblyName("BizInvokeProxyAssembly");
			ImplAssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(aname, AssemblyBuilderAccess.Run);
			ImplModuleBuilder = ImplAssemblyBuilder.DefineDynamicModule("BizInvokerModule");
		}

		/// <summary>
		/// get an implementation proxy for an interop class
		/// </summary>
		/// <typeparam name="T">The class type that represents the DLL</typeparam>
		public static T GetInvoker<T>(IImportResolver dll, ICallingConventionAdapter adapter)
			where T : class
		{
			InvokerImpl impl;
			lock (Impls)
			{
				var baseType = typeof(T);
				if (!Impls.TryGetValue(baseType, out impl))
				{
					impl = CreateProxy(baseType, false);
					Impls.Add(baseType, impl);
				}
			}

			if (impl.ConnectMonitor != null)
			{
				throw new InvalidOperationException("Class was previously proxied with a monitor!");
			}

			return (T)impl.Create(dll, null, adapter);
		}

		public static T GetInvoker<T>(IImportResolver dll, IMonitor monitor, ICallingConventionAdapter adapter)
			where T : class
		{
			InvokerImpl impl;
			lock (Impls)
			{
				var baseType = typeof(T);
				if (!Impls.TryGetValue(baseType, out impl))
				{
					impl = CreateProxy(baseType, true);
					Impls.Add(baseType, impl);
				}
			}

			if (impl.ConnectMonitor == null)
			{
				throw new InvalidOperationException("Class was previously proxied without a monitor!");
			}

			return (T)impl.Create(dll, monitor, adapter);
		}

		private static InvokerImpl CreateProxy(Type baseType, bool monitor)
		{
			if (baseType.IsSealed)
			{
				throw new InvalidOperationException("Can't proxy a sealed type");
			}

			if (!baseType.IsPublic)
			{
				// the proxy type will be in a new assembly, so public is required here
				throw new InvalidOperationException("Type must be public");
			}

			var baseConstructor = baseType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
			if (baseConstructor == null)
			{
				throw new InvalidOperationException("Base type must have a zero arg constructor");
			}

			var baseMethods = baseType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
				.Select(m => new
				{
					Info = m,
					Attr = m.GetCustomAttributes(true).OfType<BizImportAttribute>().FirstOrDefault()
				})
				.Where(a => a.Attr != null)
				.ToList();

			if (baseMethods.Count == 0)
			{
				throw new InvalidOperationException("Couldn't find any [BizImport] methods to proxy");
			}

			{
				var uo = baseMethods.FirstOrDefault(a => !a.Info.IsVirtual || a.Info.IsFinal);
				if (uo != null)
				{
					throw new InvalidOperationException("Method " + uo.Info.Name + " cannot be overriden!");
				}

				// there's no technical reason to disallow this, but we wouldn't be doing anything
				// with the base implementation, so it's probably a user error
				var na = baseMethods.FirstOrDefault(a => !a.Info.IsAbstract);
				if (na != null)
				{
					throw new InvalidOperationException("Method " + na.Info.Name + " is not abstract!");
				}
			}

			// hooks that will be run on the created proxy object
			var postCreateHooks = new List<Action<object, IImportResolver, ICallingConventionAdapter>>();

			var type = ImplModuleBuilder.DefineType("Bizhawk.BizInvokeProxy" + baseType.Name, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed, baseType);

			var monitorField = monitor ? type.DefineField("MonitorField", typeof(IMonitor), FieldAttributes.Public) : null;

			var adapterField = type.DefineField("CallingConvention", typeof(ICallingConventionAdapter), FieldAttributes.Public);

			foreach (var mi in baseMethods)
			{
				var entryPointName = mi.Attr.EntryPoint ?? mi.Info.Name;

				var hook = mi.Attr.Compatibility
					? ImplementMethodDelegate(type, mi.Info, mi.Attr.CallingConvention, entryPointName, monitorField)
					: ImplementMethodCalli(type, mi.Info, mi.Attr.CallingConvention, entryPointName, monitorField, adapterField);

				postCreateHooks.Add(hook);
			}

			var ret = new InvokerImpl
			{
				Hooks = postCreateHooks,
				ImplType = type.CreateType()
			};
			if (monitor)
			{
				ret.ConnectMonitor = (o, m) => o.GetType().GetField(monitorField.Name).SetValue(o, m);
			}
			ret.ConnectCallingConventionAdapter = (o, a) => o.GetType().GetField(adapterField.Name).SetValue(o, a);

			return ret;
		}

		/// <summary>
		/// create a method implementation that uses GetDelegateForFunctionPointer internally
		/// </summary>
		private static Action<object, IImportResolver, ICallingConventionAdapter> ImplementMethodDelegate(
			TypeBuilder type, MethodInfo baseMethod, CallingConvention nativeCall, string entryPointName, FieldInfo monitorField)
		{
			// create the delegate type
			MethodBuilder delegateInvoke;
			var delegateType = BizInvokeUtilities.CreateDelegateType(baseMethod, nativeCall, type, out delegateInvoke);

			var paramInfos = baseMethod.GetParameters();
			var paramTypes = paramInfos.Select(p => p.ParameterType).ToArray();
			var returnType = baseMethod.ReturnType;

			if (paramTypes.Concat(new[] { returnType }).Any(typeof(Delegate).IsAssignableFrom))
			{
				// this isn't a problem if CallingConventionAdapters.Waterbox is a no-op
				if (CallingConventionAdapters.Waterbox.GetType() != CallingConventionAdapters.Native.GetType())
					throw new InvalidOperationException("Compatibility call mode cannot use ICallingConventionAdapters!");
			}

			// define a field on the class to hold the delegate
			var field = type.DefineField(
				"DelegateField" + baseMethod.Name,
				delegateType,
				FieldAttributes.Public);

			var method = type.DefineMethod(
				baseMethod.Name,
				MethodAttributes.Virtual | MethodAttributes.Public,
				CallingConventions.HasThis,
				returnType,
				paramTypes);

			var il = method.GetILGenerator();

			Label exc = new Label();
			if (monitorField != null) // monitor: enter and then begin try
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, monitorField);
				il.Emit(OpCodes.Callvirt, typeof(IMonitor).GetMethod("Enter"));
				exc = il.BeginExceptionBlock();
			}

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, field);
			for (int i = 0; i < paramTypes.Length; i++)
			{
				il.Emit(OpCodes.Ldarg, (short)(i + 1));
			}

			il.Emit(OpCodes.Callvirt, delegateInvoke);

			if (monitorField != null) // monitor: finally exit
			{
				LocalBuilder loc = null;
				if (returnType != typeof(void))
				{
					loc = il.DeclareLocal(returnType);
					il.Emit(OpCodes.Stloc, loc);
				}

				il.Emit(OpCodes.Leave, exc);
				il.BeginFinallyBlock();
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, monitorField);
				il.Emit(OpCodes.Callvirt, typeof(IMonitor).GetMethod("Exit"));
				il.EndExceptionBlock();

				if (returnType != typeof(void))
				{
					il.Emit(OpCodes.Ldloc, loc);
				}
			}

			il.Emit(OpCodes.Ret);

			type.DefineMethodOverride(method, baseMethod);

			return (o, dll, adapter) =>
			{
				var entryPtr = dll.SafeResolve(entryPointName);
				var interopDelegate = adapter.GetDelegateForFunctionPointer(entryPtr, delegateType.CreateType());
				o.GetType().GetField(field.Name).SetValue(o, interopDelegate);
			};
		}

		/// <summary>
		/// create a method implementation that uses calli internally
		/// </summary>
		private static Action<object, IImportResolver, ICallingConventionAdapter> ImplementMethodCalli(
			TypeBuilder type, MethodInfo baseMethod,
			CallingConvention nativeCall, string entryPointName, FieldInfo monitorField, FieldInfo adapterField)
		{
			var paramInfos = baseMethod.GetParameters();
			var paramTypes = paramInfos.Select(p => p.ParameterType).ToArray();
			var nativeParamTypes = new List<Type>();
			var returnType = baseMethod.ReturnType;
			if (returnType != typeof(void) && !returnType.IsPrimitive)
			{
				throw new InvalidOperationException("Only primitive return types are supported");
			}

			// define a field on the type to hold the entry pointer
			var field = type.DefineField(
				"EntryPtrField" + baseMethod.Name,
				typeof(IntPtr),
				FieldAttributes.Public);

			var method = type.DefineMethod(
				baseMethod.Name,
				MethodAttributes.Virtual | MethodAttributes.Public,
				CallingConventions.HasThis,
				returnType,
				paramTypes);

			var il = method.GetILGenerator();

			Label exc = new Label();
			if (monitorField != null) // monitor: enter and then begin try
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, monitorField);
				il.Emit(OpCodes.Callvirt, typeof(IMonitor).GetMethod("Enter"));
				exc = il.BeginExceptionBlock();
			}

			for (int i = 0; i < paramTypes.Length; i++)
			{
				// arg 0 is this, so + 1
				nativeParamTypes.Add(EmitParamterLoad(il, i + 1, paramTypes[i], adapterField));
			}

			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, field);
			il.EmitCalli(OpCodes.Calli, 
				nativeCall, 
				returnType == typeof(bool) ? typeof(byte) : returnType, // undo winapi style bool garbage
				nativeParamTypes.ToArray());

			if (monitorField != null) // monitor: finally exit
			{
				LocalBuilder loc = null;
				if (returnType != typeof(void))
				{
					loc = il.DeclareLocal(returnType);
					il.Emit(OpCodes.Stloc, loc);
				}

				il.Emit(OpCodes.Leave, exc);
				il.BeginFinallyBlock();
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, monitorField);
				il.Emit(OpCodes.Callvirt, typeof(IMonitor).GetMethod("Exit"));
				il.EndExceptionBlock();

				if (returnType != typeof(void))
				{
					il.Emit(OpCodes.Ldloc, loc);
				}
			}

			// either there's a primitive on the stack and we're expected to return that primitive,
			// or there's nothing on the stack and we're expected to return nothing
			il.Emit(OpCodes.Ret);

			type.DefineMethodOverride(method, baseMethod);

			return (o, dll, adapter) =>
			{
				var entryPtr = dll.SafeResolve(entryPointName);
				o.GetType().GetField(field.Name).SetValue(
					o, adapter.GetDepartureFunctionPointer(entryPtr, new ParameterInfo(returnType, paramTypes), o));
			};
		}

		/// <summary>
		/// load an IntPtr constant in an IL stream
		/// </summary>
		private static void LoadConstant(ILGenerator il, IntPtr p)
		{
			if (p == IntPtr.Zero)
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			else if (IntPtr.Size == 4)
			{
				il.Emit(OpCodes.Ldc_I4, (int)p);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I8, (long)p);
			}

			il.Emit(OpCodes.Conv_I);
		}

		/// <summary>
		/// load a UIntPtr constant in an IL stream
		/// </summary>
		private static void LoadConstant(ILGenerator il, UIntPtr p)
		{
			if (p == UIntPtr.Zero)
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			else if (UIntPtr.Size == 4)
			{
				il.Emit(OpCodes.Ldc_I4, (int)p);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I8, (long)p);
			}

			il.Emit(OpCodes.Conv_U);
		}

		/// <summary>
		/// emit a single parameter load with unmanaged conversions
		/// </summary>
		private static Type EmitParamterLoad(ILGenerator il, int idx, Type type, FieldInfo adapterField)
		{
			if (type.IsGenericType)
			{
				throw new NotImplementedException("Generic types not supported");
			}

			if (type.IsByRef)
			{
				var et = type.GetElementType();
				if (!et.IsPrimitive && !et.IsEnum)
				{
					throw new NotImplementedException("Only refs of primitive or enum types are supported!");
				}

				var loc = il.DeclareLocal(type, true);
				il.Emit(OpCodes.Ldarg, (short)idx);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, loc);
				il.Emit(OpCodes.Conv_I);
				return typeof(IntPtr);
			}

			if (type.IsArray)
			{
				var et = type.GetElementType();
				if (!et.IsValueType)
				{
					throw new NotImplementedException("Only arrays of value types are supported!");
				}

				// these two cases aren't too hard to add
				if (type.GetArrayRank() > 1)
				{
					throw new NotImplementedException("Multidimensional arrays are not supported!");
				}

				if (type.Name.Contains('*'))
				{
					throw new NotImplementedException("Only 0-based 1-dimensional arrays are supported!");
				}

				var loc = il.DeclareLocal(type, true);
				var end = il.DefineLabel();
				var isNull = il.DefineLabel();

				il.Emit(OpCodes.Ldarg, (short)idx);
				il.Emit(OpCodes.Brfalse, isNull);

				il.Emit(OpCodes.Ldarg, (short)idx);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, loc);
				il.Emit(OpCodes.Ldc_I4_0);
				il.Emit(OpCodes.Ldelema, et);
				il.Emit(OpCodes.Conv_I);
				il.Emit(OpCodes.Br, end);

				il.MarkLabel(isNull);
				LoadConstant(il, IntPtr.Zero);
				il.MarkLabel(end);

				return typeof(IntPtr);
			}

			if (typeof(Delegate).IsAssignableFrom(type))
			{
				var mi = typeof(ICallingConventionAdapter).GetMethod("GetFunctionPointerForDelegate");
				var end = il.DefineLabel();
				var isNull = il.DefineLabel();

				il.Emit(OpCodes.Ldarg, (short)idx);
				il.Emit(OpCodes.Brfalse, isNull);

				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldfld, adapterField);
				il.Emit(OpCodes.Ldarg, (short)idx);
				il.Emit(OpCodes.Call, mi);
				il.Emit(OpCodes.Br, end);

				il.MarkLabel(isNull);
				LoadConstant(il, IntPtr.Zero);
				il.MarkLabel(end);
				return typeof(IntPtr);
			}

			if (type == typeof(string))
			{
				throw new NotImplementedException("Cannot marshal strings");
			}

			if (type.IsClass)
			{
				// non ref of class can just be passed as pointer
				var loc = il.DeclareLocal(type, true);
				var end = il.DefineLabel();
				var isNull = il.DefineLabel();

				il.Emit(OpCodes.Ldarg, (short)idx);
				il.Emit(OpCodes.Brfalse, isNull);

				il.Emit(OpCodes.Ldarg, (short)idx);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, loc);
				il.Emit(OpCodes.Conv_I);
				// skip past the methodtable pointer to the first field
				il.Emit(IntPtr.Size == 4 ? OpCodes.Ldc_I4_4 : OpCodes.Ldc_I4_8);
				il.Emit(OpCodes.Conv_I);
				il.Emit(OpCodes.Add);
				il.Emit(OpCodes.Br, end);

				il.MarkLabel(isNull);
				LoadConstant(il, IntPtr.Zero);
				il.MarkLabel(end);

				return typeof(IntPtr);
			}

			if (type.IsPrimitive || type.IsEnum)
			{
				il.Emit(OpCodes.Ldarg, (short)idx);
				return type;
			}

			throw new NotImplementedException("Unrecognized parameter type!");
		}
	}

	/// <summary>
	/// mark an abstract method to be proxied by BizInvoker
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class BizImportAttribute : Attribute
	{
		public CallingConvention CallingConvention { get; }

		/// <summary>
		/// Gets or sets the name of entry point; if not given, the method's name is used
		/// </summary>
		public string EntryPoint { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not to use a slower interop that supports more argument types
		/// </summary>
		public bool Compatibility { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BizImportAttribute"/> class. 
		/// </summary>
		/// <param name="c">unmanaged calling convention</param>
		public BizImportAttribute(CallingConvention c)
		{
			CallingConvention = c;
		}
	}
}
