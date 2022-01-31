﻿using System.Collections.Generic;

namespace BizHawk.Client.Common
{
	public class Bk2FloatConstants
	{
		public string this[string button]
		{
			get
			{
				var key = button
					.Replace("P1 ", "")
					.Replace("P2 ", "")
					.Replace("P3 ", "")
					.Replace("P4 ", "")
					.Replace("Key ", "");

				if (_systemOverrides.ContainsKey(Global.Emulator.SystemId) && _systemOverrides[Global.Emulator.SystemId].ContainsKey(key))
				{
					return _systemOverrides[Global.Emulator.SystemId][key];
				}

				if (_baseMnemonicLookupTable.ContainsKey(key))
				{
					return _baseMnemonicLookupTable[key];
				}

				return button;
			}
		}

		private readonly Dictionary<string, string> _baseMnemonicLookupTable = new Dictionary<string, string>
		{
			["Zapper X"] = "zapX",
			["Zapper Y"] = "zapY",
			["Paddle"] = "Pad",
			["Pen"] = "Pen",
			["Mouse X"] = "mX",
			["Mouse Y"] = "mY",
			["Lightgun X"] = "lX",
			["Lightgun Y"] = "lY",
			["X Axis"] = "aX",
			["Y Axis"] = "aY",
			["LStick X"] = "lsX",
			["LStick Y"] = "lsY",
			["RStick X"] = "rsX",
			["RStick Y"] = "rsY",
			["Disc Select"] = "Disc"
		};

		private readonly Dictionary<string, Dictionary<string, string>> _systemOverrides = new Dictionary<string, Dictionary<string, string>>
		{
			["A78"] = new Dictionary<string, string>
			{
				["VPos"] = "X",
				["HPos"] = "Y"
			}
		};
	}
}
