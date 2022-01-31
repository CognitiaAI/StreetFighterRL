﻿using System;
using System.ComponentModel;
using System.Linq;

using NLua;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.NES;
using BizHawk.Emulation.Cores.Consoles.Nintendo.QuickNES;

namespace BizHawk.Client.Common
{
	[Description("Functions related specifically to Nes Cores")]
	public sealed class NesLuaLibrary : LuaLibraryBase
	{
		// TODO:  
		// perhaps with the new core config system, one could
		// automatically bring out all of the settings to a lua table, with names.  that
		// would be completely arbitrary and would remove the whole requirement for this mess
		public NesLuaLibrary(Lua lua)
			: base(lua) { }

		[OptionalService]
		private NES Neshawk { get; set; }

		[OptionalService]
		private QuickNES Quicknes { get; set; }

		[OptionalService]
		private IMemoryDomains MemoryDomains { get; set; }

		private bool NESAvailable => Neshawk != null || Quicknes != null;

		private bool HasMemoryDOmains => MemoryDomains != null;

		public NesLuaLibrary(Lua lua, Action<string> logOutputCallback)
			: base(lua, logOutputCallback) { }

		public override string Name => "nes";

		[LuaMethod("addgamegenie", "Adds the specified game genie code. If an NES game is not currently loaded or the code is not a valid game genie code, this will have no effect")]
		public void AddGameGenie(string code)
		{
			if (NESAvailable && HasMemoryDOmains)
			{
				var decoder = new NESGameGenieDecoder(code);
				var watch = Watch.GenerateWatch(
					MemoryDomains["System Bus"],
					decoder.Address,
					WatchSize.Byte,
					DisplayType.Hex,
					false,
					code);

				Global.CheatList.Add(new Cheat(
					watch,
					decoder.Value,
					decoder.Compare));
			}
		}

		[LuaMethod("getallowmorethaneightsprites", "Gets the NES setting 'Allow more than 8 sprites per scanline' value")]
		public bool GetAllowMoreThanEightSprites()
		{
			if (Quicknes != null)
			{
				return Quicknes.GetSettings().NumSprites != 8;
			}

			if (Neshawk != null)
			{
				return Neshawk.GetSettings().AllowMoreThanEightSprites;
			}

			throw new InvalidOperationException();
		}

		[LuaMethod("getbottomscanline", "Gets the current value for the bottom scanline value")]
		public int GetBottomScanline(bool pal = false)
		{
			if (Quicknes != null)
			{
				return Quicknes.GetSettings().ClipTopAndBottom ? 231 : 239;
			}

			if (Neshawk != null)
			{
				return pal
					? Neshawk.GetSettings().PAL_BottomLine
					: Neshawk.GetSettings().NTSC_BottomLine;
			}

			throw new InvalidOperationException();
		}

		[LuaMethod("getclipleftandright", "Gets the current value for the Clip Left and Right sides option")]
		public bool GetClipLeftAndRight()
		{
			if (Quicknes != null)
			{
				return Quicknes.GetSettings().ClipLeftAndRight;
			}

			if (Neshawk != null)
			{
				return Neshawk.GetSettings().ClipLeftAndRight;
			}

			throw new InvalidOperationException();
		}

		[LuaMethod("getdispbackground", "Indicates whether or not the bg layer is being displayed")]
		public bool GetDisplayBackground()
		{
			if (Quicknes != null)
			{
				return true;
			}

			if (Neshawk != null)
			{
				return Neshawk.GetSettings().DispBackground;
			}

			throw new InvalidOperationException();
		}

		[LuaMethod("getdispsprites", "Indicates whether or not sprites are being displayed")]
		public bool GetDisplaySprites()
		{
			if (Quicknes != null)
			{
				return Quicknes.GetSettings().NumSprites > 0;
			}

			if (Neshawk != null)
			{
				return Neshawk.GetSettings().DispSprites;
			}

			throw new InvalidOperationException();
		}

		[LuaMethod("gettopscanline", "Gets the current value for the top scanline value")]
		public int GetTopScanline(bool pal = false)
		{
			if (Quicknes != null)
			{
				return Quicknes.GetSettings().ClipTopAndBottom ? 8 : 0;
			}

			if (Neshawk != null)
			{
				return pal
					? Neshawk.GetSettings().PAL_TopLine
					: Neshawk.GetSettings().NTSC_TopLine;
			}

			throw new InvalidOperationException();
		}

		[LuaMethod("removegamegenie", "Removes the specified game genie code. If an NES game is not currently loaded or the code is not a valid game genie code, this will have no effect")]
		public void RemoveGameGenie(string code)
		{
			if (NESAvailable)
			{
				var decoder = new NESGameGenieDecoder(code);
				Global.CheatList.RemoveRange(
					Global.CheatList.Where(c => c.Address == decoder.Address));
			}
		}

		[LuaMethod("setallowmorethaneightsprites", "Sets the NES setting 'Allow more than 8 sprites per scanline'")]
		public void SetAllowMoreThanEightSprites(bool allow)
		{
			if (Neshawk != null)
			{
				var s = Neshawk.GetSettings();
				s.AllowMoreThanEightSprites = allow;
				Neshawk.PutSettings(s);
			}
			else if (Quicknes != null)
			{
				var s = Quicknes.GetSettings();
				s.NumSprites = allow ? 64 : 8;
				Quicknes.PutSettings(s);
			}
		}

		[LuaMethod("setclipleftandright", "Sets the Clip Left and Right sides option")]
		public void SetClipLeftAndRight(bool leftandright)
		{
			if (Neshawk != null)
			{
				var s = Neshawk.GetSettings();
				s.ClipLeftAndRight = leftandright;
				Neshawk.PutSettings(s);
			}
			else if (Quicknes != null)
			{
				var s = Quicknes.GetSettings();
				s.ClipLeftAndRight = leftandright;
				Quicknes.PutSettings(s);
			}
		}

		[LuaMethod("setdispbackground", "Sets whether or not the background layer will be displayed")]
		public void SetDisplayBackground(bool show)
		{
			if (Neshawk != null)
			{
				var s = Neshawk.GetSettings();
				s.DispBackground = show;
				Neshawk.PutSettings(s);
			}
		}

		[LuaMethod("setdispsprites", "Sets whether or not sprites will be displayed")]
		public void SetDisplaySprites(bool show)
		{
			if (Neshawk != null)
			{
				var s = Neshawk.GetSettings();
				s.DispSprites = show;
				Neshawk.PutSettings(s);
			}
			else if (Quicknes != null)
			{
				var s = Quicknes.GetSettings();
				s.NumSprites = show ? 8 : 0;
				Quicknes.PutSettings(s);
			}
		}

		[LuaMethod("setscanlines", "sets the top and bottom scanlines to be drawn (same values as in the graphics options dialog). Top must be in the range of 0 to 127, bottom must be between 128 and 239. Not supported in the Quick Nes core")]
		public void SetScanlines(int top, int bottom, bool pal = false)
		{
			if (Neshawk != null)
			{
				if (top > 127)
				{
					top = 127;
				}
				else if (top < 0)
				{
					top = 0;
				}

				if (bottom > 239)
				{
					bottom = 239;
				}
				else if (bottom < 128)
				{
					bottom = 128;
				}

				var s = Neshawk.GetSettings();

				if (pal)
				{
					s.PAL_TopLine = top;
					s.PAL_BottomLine = bottom;
				}
				else
				{
					s.NTSC_TopLine = top;
					s.NTSC_BottomLine = bottom;
				}

				Neshawk.PutSettings(s);
			}
		}
	}
}
