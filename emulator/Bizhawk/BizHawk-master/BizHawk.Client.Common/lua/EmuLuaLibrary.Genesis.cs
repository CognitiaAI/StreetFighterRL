﻿using System;
using System.ComponentModel;

using NLua;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Consoles.Sega.gpgx;

namespace BizHawk.Client.Common
{
	[Description("Functions specific to GenesisHawk (functions may not run when an Genesis game is not loaded)")]
	public sealed class GenesisLuaLibrary : LuaLibraryBase
	{
		[OptionalService]
		private GPGX Genesis { get; set; }

		public GenesisLuaLibrary(Lua lua)
			: base(lua) { }

		public GenesisLuaLibrary(Lua lua, Action<string> logOutputCallback)
			: base(lua, logOutputCallback) { }

		public override string Name => "genesis";

		private GPGX.GPGXSettings GetSettings()
		{
			if (Genesis != null)
			{
				return Genesis.GetSettings();
			}

			return new GPGX.GPGXSettings();
		}

		private void PutSettings(GPGX.GPGXSettings settings)
		{
			Genesis?.PutSettings(settings);
		}

		[LuaMethod("getlayer_bga", "Returns whether the bg layer A is displayed")]
		public bool GetLayerBgA()
		{
			return GetSettings().DrawBGA;
		}

		[LuaMethod("getlayer_bgb", "Returns whether the bg layer B is displayed")]
		public bool GetLayerBgB()
		{
			return GetSettings().DrawBGB;
		}

		[LuaMethod("getlayer_bgw", "Returns whether the bg layer W is displayed")]
		public bool GetLayerBgW()
		{
			return GetSettings().DrawBGW;
		}

		[LuaMethod("setlayer_bga", "Sets whether the bg layer A is displayed")]
		public void SetLayerBgA(bool value)
		{
			var s = GetSettings();
			s.DrawBGA = value;
			PutSettings(s);
		}

		[LuaMethod("setlayer_bgb", "Sets whether the bg layer B is displayed")]
		public void SetLayerBgB(bool value)
		{
			var s = GetSettings();
			s.DrawBGB = value;
			PutSettings(s);
		}

		[LuaMethod("setlayer_bgw", "Sets whether the bg layer W is displayed")]
		public void SetLayerBgW(bool value)
		{
			var s = GetSettings();
			s.DrawBGW = value;
			PutSettings(s);
		}
	}
}
