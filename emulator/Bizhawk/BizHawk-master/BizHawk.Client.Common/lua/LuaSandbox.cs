﻿using System;
using System.Runtime.InteropServices;
using NLua;

// TODO - evaluate for re-entrancy problems
namespace BizHawk.Client.Common
{
	public unsafe class LuaSandbox
	{
		private static readonly System.Runtime.CompilerServices.ConditionalWeakTable<Lua, LuaSandbox> SandboxForThread = new System.Runtime.CompilerServices.ConditionalWeakTable<Lua, LuaSandbox>();

		public static Action<string> DefaultLogger { get; set; }

		public void SetSandboxCurrentDirectory(string dir)
		{
			_currentDirectory = dir;
		}

		private string _currentDirectory;

		#if WINDOWS
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool SetCurrentDirectoryW(byte* lpPathName);
		[DllImport("kernel32.dll", SetLastError=true)]
		static extern uint GetCurrentDirectoryW(uint nBufferLength, byte* pBuffer);
		#endif

		private bool CoolSetCurrentDirectory(string path, string currDirSpeedHack = null)
		{
			string target = _currentDirectory + "\\";

			// first we'll bypass it with a general hack: dont do any setting if the value's already there (even at the OS level, setting the directory can be slow)
			// yeah I know, not the smoothest move to compare strings here, in case path normalization is happening at some point
			// but you got any better ideas?
			if (currDirSpeedHack == null)
			{
				currDirSpeedHack = CoolGetCurrentDirectory();
			}

			if (currDirSpeedHack == path)
			{
				return true;
			}

			// WARNING: setting the current directory is SLOW!!! security checks for some reason.
			// so we're bypassing it with windows hacks
			#if WINDOWS
				fixed (byte* pstr = &System.Text.Encoding.Unicode.GetBytes(target + "\0")[0])
					return SetCurrentDirectoryW(pstr);
			#else
				if (System.IO.Directory.Exists(CurrentDirectory)) // race condition for great justice
				{
					Environment.CurrentDirectory = CurrentDirectory; // thats right, you can't set a directory as current that doesnt exist because .net's got to do SENSELESS SLOW-ASS SECURITY CHECKS on it and it can't do that on a NONEXISTENT DIRECTORY
					return true;
				}
				else
				{
					return false;
				}
			#endif
		}

		private string CoolGetCurrentDirectory()
		{
			// GUESS WHAT!
			// .NET DOES A SECURITY CHECK ON THE DIRECTORY WE JUST RETRIEVED
			// AS IF ASKING FOR THE CURRENT DIRECTORY IS EQUIVALENT TO TRYING TO ACCESS IT
			// SCREW YOU
			#if WINDOWS
				var buf = new byte[32768];
				fixed(byte* pBuf = &buf[0])
				{
					uint ret = GetCurrentDirectoryW(32767, pBuf);
					return System.Text.Encoding.Unicode.GetString(buf, 0, (int)ret*2);
				}
			#else
				return Environment.CurrentDirectory;
			#endif
		}

		private void Sandbox(Action callback, Action exceptionCallback)
		{
			string savedEnvironmentCurrDir = null;
			try
			{
				savedEnvironmentCurrDir = Environment.CurrentDirectory;

				if (_currentDirectory != null)
				{
					CoolSetCurrentDirectory(_currentDirectory, savedEnvironmentCurrDir);
				}

				EnvironmentSandbox.Sandbox(callback);
			}
			catch (NLua.Exceptions.LuaException ex)
			{
				Console.WriteLine(ex);
				DefaultLogger(ex.ToString());
				exceptionCallback?.Invoke();
			}
			finally
			{
				if (_currentDirectory != null)
				{
					CoolSetCurrentDirectory(savedEnvironmentCurrDir);
				}
			}
		}

		public static LuaSandbox CreateSandbox(Lua thread, string initialDirectory)
		{
			var sandbox = new LuaSandbox();
			SandboxForThread.Add(thread, sandbox);
			sandbox.SetSandboxCurrentDirectory(initialDirectory);
			return sandbox;
		}

		public static LuaSandbox GetSandbox(Lua thread)
		{
			// this is just placeholder.
			// we shouldnt be calling a sandbox with no thread--construct a sandbox properly
			if (thread == null)
			{
				return new LuaSandbox();
			}

			lock (SandboxForThread)
			{
				LuaSandbox sandbox;
				if (SandboxForThread.TryGetValue(thread, out sandbox))
				{
					return sandbox;
				}
				
				// for now: throw exception (I want to manually creating them)
				// return CreateSandbox(thread);
				throw new InvalidOperationException("HOARY GORILLA HIJINX");
			}
		}

		public static void Sandbox(Lua thread, Action callback, Action exceptionCallback = null)
		{
			GetSandbox(thread).Sandbox(callback, exceptionCallback);
		}
	}
}
