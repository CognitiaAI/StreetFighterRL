﻿using System;
using System.IO;
using System.Linq;

using BizHawk.Common;

namespace BizHawk.Emulation.Common
{
	/// <summary>
	/// Can freeze a copy of a controller input set and serialize\deserialize it
	/// </summary>
	public class SaveController : IController
	{
		private readonly WorkingDictionary<string, float> _buttons = new WorkingDictionary<string, float>();

		public SaveController()
		{
			Definition = null;
		}

		public SaveController(ControllerDefinition def)
		{
			Definition = def;
		}

		/// <summary>
		/// Gets the current definition.
		/// Invalid until CopyFrom has been called
		/// </summary>
		public ControllerDefinition Definition { get; private set; }

		public void Serialize(BinaryWriter b)
		{
			b.Write(_buttons.Keys.Count);
			foreach (var k in _buttons.Keys)
			{
				b.Write(k);
				b.Write(_buttons[k]);
			}
		}

		/// <summary>
		/// No checking to see if the deserialized controls match any definition
		/// </summary>
		public void DeSerialize(BinaryReader b)
		{
			_buttons.Clear();
			int numbuttons = b.ReadInt32();
			for (int i = 0; i < numbuttons; i++)
			{
				string k = b.ReadString();
				float v = b.ReadSingle();
				_buttons.Add(k, v);
			}
		}

		/// <summary>
		/// This controller's definition changes to that of source
		/// </summary>
		public void CopyFrom(IController source)
		{
			Definition = source.Definition;
			_buttons.Clear();
			foreach (var k in Definition.BoolButtons)
			{
				_buttons.Add(k, source.IsPressed(k) ? 1.0f : 0);
			}

			foreach (var k in Definition.FloatControls)
			{
				if (_buttons.Keys.Contains(k))
				{
					throw new Exception("name collision between bool and float lists!");
				}

				_buttons.Add(k, source.GetFloat(k));
			}
		}

		public void Clear()
		{
			_buttons.Clear();
		}

		public void Set(string button)
		{
			_buttons[button] = 1.0f;
		}

		public bool IsPressed(string button)
		{
			return _buttons[button] != 0;
		}

		public float GetFloat(string name)
		{
			return _buttons[name];
		}
	}
}
