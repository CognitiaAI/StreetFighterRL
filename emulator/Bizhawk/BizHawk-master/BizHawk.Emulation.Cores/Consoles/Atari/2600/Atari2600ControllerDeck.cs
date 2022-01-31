﻿using System;
using System.Collections.Generic;
using System.Linq;

using BizHawk.Common;
using BizHawk.Common.ReflectionExtensions;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Atari.Atari2600
{
	public class Atari2600ControllerDeck
	{
		private static readonly Type[] Implementors =
		{
			typeof(UnpluggedController), // Order must match Atari2600ControllerTypes enum values
			typeof(StandardController),
			typeof(PaddleController)
		};

		public Atari2600ControllerDeck(Atari2600ControllerTypes controller1, Atari2600ControllerTypes controller2)
		{
			Port1 = (IPort)Activator.CreateInstance(Implementors[(int)controller1], 1);
			Port2 = (IPort)Activator.CreateInstance(Implementors[(int)controller2], 2);

			Definition = new ControllerDefinition
			{
				Name = "Atari 2600 Basic Controller",
				BoolButtons = Port1.Definition.BoolButtons
					.Concat(Port2.Definition.BoolButtons)
					.Concat(new[]
					{
						"Reset", "Select", "Power", "Toggle Left Difficulty", "Toggle Right Difficulty"
					})
					.ToList()
			};

			Definition.FloatControls.AddRange(Port1.Definition.FloatControls);
			Definition.FloatControls.AddRange(Port2.Definition.FloatControls);

			Definition.FloatRanges.AddRange(Port1.Definition.FloatRanges);
			Definition.FloatRanges.AddRange(Port2.Definition.FloatRanges);
		}

		public byte ReadPort1(IController c)
		{
			return Port1.Read(c);
		}

		public byte ReadPort2(IController c)
		{
			return Port2.Read(c);
		}

		public int ReadPot1(IController c, int pot)
		{
			return Port1.Read_Pot(c, pot);
		}

		public int ReadPot2(IController c, int pot)
		{
			return Port2.Read_Pot(c, pot);
		}

		public ControllerDefinition Definition { get; }

		public void SyncState(Serializer ser)
		{
			ser.BeginSection("Port1");
			Port1.SyncState(ser);
			ser.EndSection();

			ser.BeginSection("Port2");
			Port2.SyncState(ser);
			ser.EndSection();
		}

		private readonly IPort Port1;
		private readonly IPort Port2;

		private static Dictionary<string, Type> _controllerTypes;

		public static Dictionary<string, Type> ValidControllerTypes
		{
			get
			{
				if (_controllerTypes == null)
				{
					_controllerTypes = typeof(Atari2600ControllerDeck).Assembly
						.GetTypes()
						.Where(t => typeof(IPort).IsAssignableFrom(t))
						.Where(t => !t.IsAbstract && !t.IsInterface)
						.ToDictionary(tkey => tkey.DisplayName());
				}

				return _controllerTypes;
			}
		}

		public static string DefaultControllerName => typeof(StandardController).DisplayName();
	}
}
