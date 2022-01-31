﻿using System.Collections.Generic;
using System.Linq;
using System.Globalization;

using BizHawk.Common;

namespace BizHawk.Emulation.Common
{
	public class GameInfo
	{
		public bool IsRomStatusBad()
		{
			return Status == RomStatus.BadDump || Status == RomStatus.Overdump;
		}

		public string Name { get; set; }
		public string System { get; set; }
		public string Hash { get; set; }
		public string Region { get; set; }
		public RomStatus Status { get; set; } = RomStatus.NotInDatabase;
		public bool NotInDatabase { get; set; } = true;
		public string FirmwareHash { get; set; }
		public string ForcedCore { get; private set; }

		private Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();

		public GameInfo()
		{
		}

		public GameInfo Clone()
		{
			var ret = (GameInfo)MemberwiseClone();
			ret.Options = new Dictionary<string, string>(Options);
			return ret;
		}

		public static GameInfo NullInstance => new GameInfo
		{
			Name = "Null",
			System = "NULL",
			Hash = "",
			Region = "",
			Status = RomStatus.GoodDump,
			ForcedCore = "",
			NotInDatabase = false
		};

		public bool IsNullInstance => System == "NULL";

		internal GameInfo(CompactGameInfo cgi)
		{
			Name = cgi.Name;
			System = cgi.System;
			Hash = cgi.Hash;
			Region = cgi.Region;
			Status = cgi.Status;
			ForcedCore = cgi.ForcedCore;
			NotInDatabase = false;
			ParseOptionsDictionary(cgi.MetaData);
		}

		public void AddOption(string option)
		{
			Options[option] = "";
		}

		public void AddOption(string option, string param)
		{
			Options[option] = param;
		}

		public void RemoveOption(string option)
		{
			Options.Remove(option);
		}

		public bool this[string option] => Options.ContainsKey(option);

		public bool OptionPresent(string option)
		{
			return Options.ContainsKey(option);
		}

		public string OptionValue(string option)
		{
			if (Options.ContainsKey(option))
			{
				return Options[option];
			}

			return null;
		}

		public int GetIntValue(string option)
		{
			return int.Parse(Options[option]);
		}

		public string GetStringValue(string option)
		{
			return Options[option];
		}

		public int GetHexValue(string option)
		{
			return int.Parse(Options[option], NumberStyles.HexNumber);
		}

		/// <summary>
		/// /// Gets a boolean value from the database
		/// </summary>
		/// <param name="parameter">The option to look up</param>
		/// <param name="defaultVal">The value to return if the option is invalid or doesn't exist</param>
		/// <returns> The boolean value from the database if present, otherwise the given default value</returns>
		public bool GetBool(string parameter, bool defaultVal)
		{
			if (OptionPresent(parameter) && OptionValue(parameter) == "true")
			{
				return true;
			}

			if (OptionPresent(parameter) && OptionValue(parameter) == "false")
			{
				return false;
			}
			
			return defaultVal;
		}

		/// <summary>
		/// /// Gets an integer value from the database
		/// </summary>
		/// <param name="parameter">The option to look up</param>
		/// <param name="defaultVal">The value to return if the option is invalid or doesn't exist</param>
		/// <returns> The integer value from the database if present, otherwise the given default value</returns>
		public int GetInt(string parameter, int defaultVal)
		{
			if (OptionPresent(parameter))
			{
				try
				{
					return int.Parse(OptionValue(parameter));
				}
				catch
				{
					return defaultVal;
				}
			}

			return defaultVal;
		}

		public ICollection<string> GetOptions()
		{
			return Options.Keys;
		}

		public IDictionary<string, string> GetOptionsDict()
		{
			return new ReadOnlyDictionary<string, string>(Options);
		}

		private void ParseOptionsDictionary(string metaData)
		{
			if (string.IsNullOrEmpty(metaData))
			{
				return;
			}

			var options = metaData.Split(';').Where(opt => string.IsNullOrEmpty(opt) == false).ToArray();

			foreach (var opt in options)
			{
				var parts = opt.Split('=');
				var key = parts[0];
				var value = parts.Length > 1 ? parts[1] : "";
				Options[key] = value;
			}
		}
	}
}
