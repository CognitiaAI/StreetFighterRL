﻿using System;
using System.IO;
using NLua;

namespace BizHawk.Client.Common
{
	public sealed class MovieLuaLibrary : LuaLibraryBase
	{
		public MovieLuaLibrary(Lua lua)
			: base(lua) { }

		public MovieLuaLibrary(Lua lua, Action<string> logOutputCallback)
			: base(lua, logOutputCallback) { }

		public override string Name => "movie";

		[LuaMethod("startsfromsavestate", "Returns whether or not the movie is a savestate-anchored movie")]
		public bool StartsFromSavestate()
		{
			return Global.MovieSession.Movie.IsActive && Global.MovieSession.Movie.StartsFromSavestate;
		}

		[LuaMethod("startsfromsaveram", "Returns whether or not the movie is a saveram-anchored movie")]
		public bool StartsFromSaveram()
		{
			return Global.MovieSession.Movie.IsActive && Global.MovieSession.Movie.StartsFromSaveRam;
		}

		[LuaMethod("filename", "Returns the file name including path of the currently loaded movie")]
		public static string Filename()
		{
			return Global.MovieSession.Movie.Filename;
		}

		[LuaMethod("getinput", "Returns a table of buttons pressed on a given frame of the loaded movie")]
		public LuaTable GetInput(int frame)
		{
			if (!Global.MovieSession.Movie.IsActive)
			{
				Log("No movie loaded");
				return null;
			}

			var input = Lua.NewTable();
			var adapter = Global.MovieSession.Movie.GetInputState(frame);

			if (adapter == null)
			{
				Log("Can't get input of the last frame of the movie. Use the previous frame");
				return null;
			}

			foreach (var button in adapter.Definition.BoolButtons)
			{
				input[button] = adapter.IsPressed(button);
			}

			foreach (var button in adapter.Definition.FloatControls)
			{
				input[button] = adapter.GetFloat(button);
			}

			return input;
		}

		[LuaMethod("getinputasmnemonic", "Returns the input of a given frame of the loaded movie in a raw inputlog string")]
		public string GetInputAsMnemonic(int frame)
		{
			if (Global.MovieSession.Movie.IsActive && frame < Global.MovieSession.Movie.InputLogLength)
			{
				var lg = Global.MovieSession.LogGeneratorInstance();
				lg.SetSource(Global.MovieSession.Movie.GetInputState(frame));
				return lg.GenerateLogEntry();
			}

			return "";
		}

		[LuaMethod("getreadonly", "Returns true if the movie is in read-only mode, false if in read+write")]
		public static bool GetReadOnly()
		{
			return Global.MovieSession.ReadOnly;
		}

		[LuaMethod("getrerecordcount", "Gets the rerecord count of the current movie.")]
		public static ulong GetRerecordCount()
		{
			return Global.MovieSession.Movie.Rerecords;
		}

		[LuaMethod("getrerecordcounting", "Returns whether or not the current movie is incrementing rerecords on loadstate")]
		public static bool GetRerecordCounting()
		{
			return Global.MovieSession.Movie.IsCountingRerecords;
		}

		[LuaMethod("isloaded", "Returns true if a movie is loaded in memory (play, record, or finished modes), false if not (inactive mode)")]
		public static bool IsLoaded()
		{
			return Global.MovieSession.Movie.IsActive;
		}

		[LuaMethod("length", "Returns the total number of frames of the loaded movie")]
		public static double Length()
		{
			return Global.MovieSession.Movie.FrameCount;
		}

		[LuaMethod("mode", "Returns the mode of the current movie. Possible modes: \"PLAY\", \"RECORD\", \"FINISHED\", \"INACTIVE\"")]
		public static string Mode()
		{
			if (Global.MovieSession.Movie.IsFinished)
			{
				return "FINISHED";
			}
			
			if (Global.MovieSession.Movie.IsPlaying)
			{
				return "PLAY";
			}
			
			if (Global.MovieSession.Movie.IsRecording)
			{
				return "RECORD";
			}
			
			return "INACTIVE";
		}

		[LuaMethod("save", "Saves the current movie to the disc. If the filename is provided (no extension or path needed), the movie is saved under the specified name to the current movie directory. The filename may contain a subdirectory, it will be created if it doesn't exist. Existing files won't get overwritten.")]
		public void Save(string filename = "")
		{
			if (!Global.MovieSession.Movie.IsActive)
			{
				return;
			}

			if (!string.IsNullOrEmpty(filename))
			{
				filename += "." + Global.MovieSession.Movie.PreferredExtension;
				var test = new FileInfo(filename);
				if (test.Exists)
				{
					Log($"File {filename} already exists, will not overwrite");
					return;
				}

				Global.MovieSession.Movie.Filename = filename;
			}

			Global.MovieSession.Movie.Save();
		}

		[LuaMethod("setreadonly", "Sets the read-only state to the given value. true for read only, false for read+write")]
		public static void SetReadOnly(bool readOnly)
		{
			Global.MovieSession.ReadOnly = readOnly;
		}

		[LuaMethod("setrerecordcount", "Sets the rerecord count of the current movie.")]
		public static void SetRerecordCount(double count)
		{
			// Lua numbers are always double, integer precision holds up
			// to 53 bits, so throw an error if it's bigger than that.
			const double PrecisionLimit = 9007199254740992d;

			if (count > PrecisionLimit)
			{
				throw new Exception("Rerecord count exceeds Lua integer precision.");
			}

			Global.MovieSession.Movie.Rerecords = (ulong)count;
		}

		[LuaMethod("setrerecordcounting", "Sets whether or not the current movie will increment the rerecord counter on loadstate")]
		public static void SetRerecordCounting(bool counting)
		{
			Global.MovieSession.Movie.IsCountingRerecords = counting;
		}

		[LuaMethod("stop", "Stops the current movie")]
		public static void Stop()
		{
			Global.MovieSession.Movie.Stop();
		}

		[LuaMethod("getfps", "If a movie is loaded, gets the frames per second used by the movie to determine the movie length time")]
		public static double GetFps()
		{
			if (Global.MovieSession.Movie.IsActive)
			{
				var movie = Global.MovieSession.Movie;
				var system = movie.HeaderEntries[HeaderKeys.PLATFORM];
				var pal = movie.HeaderEntries.ContainsKey(HeaderKeys.PAL) &&
						movie.HeaderEntries[HeaderKeys.PAL] == "1";

				return new PlatformFrameRates()[system, pal];
			}

			return 0.0;
		}

		[LuaMethod("getheader", "If a movie is active, will return the movie header as a lua table")]
		public LuaTable GetHeader()
		{
			var luaTable = Lua.NewTable();
			if (Global.MovieSession.Movie.IsActive)
			{
				foreach (var kvp in Global.MovieSession.Movie.HeaderEntries)
				{
					luaTable[kvp.Key] = kvp.Value;
				}
			}

			return luaTable;
		}

		[LuaMethod("getcomments", "If a movie is active, will return the movie comments as a lua table")]
		public LuaTable GetComments()
		{
			var luaTable = Lua.NewTable();
			if (Global.MovieSession.Movie.IsActive)
			{
				for (int i = 0; i < Global.MovieSession.Movie.Comments.Count; i++)
				{
					luaTable[i] = Global.MovieSession.Movie.Comments[i];
				}
			}

			return luaTable;
		}

		[LuaMethod("getsubtitles", "If a movie is active, will return the movie subtitles as a lua table")]
		public LuaTable GetSubtitles()
		{
			var luaTable = Lua.NewTable();
			if (Global.MovieSession.Movie.IsActive)
			{
				for (int i = 0; i < Global.MovieSession.Movie.Subtitles.Count; i++)
				{
					luaTable[i] = Global.MovieSession.Movie.Subtitles[i].ToString();
				}
			}

			return luaTable;
		}
	}
}
