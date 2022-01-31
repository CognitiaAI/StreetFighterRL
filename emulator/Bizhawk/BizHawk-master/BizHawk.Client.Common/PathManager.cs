﻿using System;
using System.Linq;
using System.IO;
using System.Reflection;

using BizHawk.Common.StringExtensions;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Common.IEmulatorExtensions;
using BizHawk.Emulation.Cores.Nintendo.SNES;
using BizHawk.Emulation.Cores.Nintendo.SNES9X;

namespace BizHawk.Client.Common
{
	public static class PathManager
	{
		public static string GetExeDirectoryAbsolute()
		{
			var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				path = path.Remove(path.Length - 1, 1);
			}

			return path;
		}

		/// <summary>
		/// Makes a path relative to the %exe% directory
		/// </summary>
		public static string MakeProgramRelativePath(string path)
		{
			return MakeAbsolutePath("%exe%/" + path, null);
		}

		public static string GetDllDirectory()
		{
			return Path.Combine(GetExeDirectoryAbsolute(), "dll");
		}

		/// <summary>
		/// The location of the default INI file
		/// </summary>
		public static string DefaultIniPath => MakeProgramRelativePath("config.ini");

		/// <summary>
		/// Gets absolute base as derived from EXE
		/// </summary>
		public static string GetGlobalBasePathAbsolute()
		{
			var gbase = Global.Config.PathEntries.GlobalBaseFragment;

			// if %exe% prefixed then substitute exe path and repeat
			if(gbase.StartsWith("%exe%",StringComparison.InvariantCultureIgnoreCase))
				gbase = GetExeDirectoryAbsolute() + gbase.Substring(5);

			//rooted paths get returned without change
			//(this is done after keyword substitution to avoid problems though)
			if (Path.IsPathRooted(gbase))
				return gbase;

			//not-rooted things are relative to exe path
			gbase = Path.Combine(GetExeDirectoryAbsolute(), gbase);

			return gbase;
		}

		public static string GetPlatformBase(string system)
		{
			return Global.Config.PathEntries[system, "Base"].Path;
		}

		public static string StandardFirmwareName(string name)
		{
			return Path.Combine(MakeAbsolutePath(Global.Config.PathEntries.FirmwaresPathFragment, null), name);
		}

		public static string MakeAbsolutePath(string path, string system)
		{
			//warning: supposedly Path.GetFullPath accesses directories (and needs permissions)
			//if this poses a problem, we need to paste code from .net or mono sources and fix them to not pose problems, rather than homebrew stuff
			return Path.GetFullPath(MakeAbsolutePathInner(path, system));
		}

		static string MakeAbsolutePathInner(string path, string system)
		{
			// Hack
			if (system == "Global")
			{
				system = null;
			}

			// This function translates relative path and special identifiers in absolute paths
			if (path.Length < 1)
			{
				return GetGlobalBasePathAbsolute();
			}

			if (path == "%recent%")
			{
				return Environment.SpecialFolder.Recent.ToString();
			}

			if (path.Length >= 5 && path.Substring(0, 5) == "%exe%")
			{
				if (path.Length == 5)
				{
					return GetExeDirectoryAbsolute();
				}

				var tmp = path.Remove(0, 5);
				tmp = tmp.Insert(0, GetExeDirectoryAbsolute());
				return tmp;
			}

			if (path[0] == '.')
			{
				if (!string.IsNullOrWhiteSpace(system))
				{
					path = path.Remove(0, 1);
					path = path.Insert(0, GetPlatformBase(system));
				}

				if (path.Length == 1)
				{
					return GetGlobalBasePathAbsolute();
				}

				if (path[0] == '.')
				{
					path = path.Remove(0, 1);
					path = path.Insert(0, GetGlobalBasePathAbsolute());
				}

				return path;
			}

			if (Path.IsPathRooted(path))
				return path;

			//handling of initial .. was removed (Path.GetFullPath can handle it)
			//handling of file:// or file:\\ was removed  (can Path.GetFullPath handle it? not sure)

			// all pad paths default to EXE
			return GetExeDirectoryAbsolute();
		}

		public static string RemoveParents(string path, string workingpath)
		{
			// determines number of parents, then removes directories from working path, return absolute path result
			// Ex: "..\..\Bob\", "C:\Projects\Emulators\Bizhawk" will return "C:\Projects\Bob\" 
			int x = NumParentDirectories(path);
			if (x > 0)
			{
				int y = path.HowMany("..\\");
				int z = workingpath.HowMany("\\");
				if (y >= z)
				{
					// Return drive letter only, working path must be absolute?
				}

				return "";
			}

			return path;
		}

		public static int NumParentDirectories(string path)
		{
			// determine the number of parent directories in path and return result
			int x = path.HowMany('\\');
			if (x > 0)
			{
				return path.HowMany("..\\");
			}

			return 0;
		}

		public static bool IsRecent(string path)
		{
			return path == "%recent%";
		}

		public static string GetLuaPath()
		{
			return MakeAbsolutePath(Global.Config.PathEntries.LuaPathFragment, null);
		}

		// Decides if a path is non-empty, not . and not .\
		private static bool PathIsSet(string path)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				return path != "." && path != ".\\";
			}

			return false;
		}

		public static string GetRomsPath(string sysId)
		{
			if (Global.Config.UseRecentForROMs)
			{
				return Environment.SpecialFolder.Recent.ToString();
			}

			var path = Global.Config.PathEntries[sysId, "ROM"];

			if (path == null || !PathIsSet(path.Path))
			{
				path = Global.Config.PathEntries["Global", "ROM"];

				if (path != null && PathIsSet(path.Path))
				{
					return MakeAbsolutePath(path.Path, null);
				}
			}

			return MakeAbsolutePath(path.Path, sysId);
		}

		public static string RemoveInvalidFileSystemChars(string name)
		{
			var newStr = name;
			var chars = Path.GetInvalidFileNameChars();
			return chars.Aggregate(newStr, (current, c) => current.Replace(c.ToString(), ""));
		}

		public static string FilesystemSafeName(GameInfo game)
		{
			var filesystemSafeName = game.Name
				.Replace("|", "+")
				.Replace(":", " -") // adelikat - Path.GetFileName scraps everything to the left of a colon unfortunately, so we need this hack here
				.Replace("\"", ""); // adelikat - Ivan Ironman Stewart's Super Off-Road has quotes in game name

			// zero 06-nov-2015 - regarding the below, i changed my mind. for libretro i want subdirectories here.
			var filesystemDir = Path.GetDirectoryName(filesystemSafeName);
			filesystemSafeName = Path.GetFileName(filesystemSafeName);

			filesystemSafeName = RemoveInvalidFileSystemChars(filesystemSafeName);

			// zero 22-jul-2012 - i dont think this is used the same way it used to. game.Name shouldnt be a path, so this stuff is illogical.
			// if game.Name is a path, then someone shouldve made it not-a-path already.
			// return Path.Combine(Path.GetDirectoryName(filesystemSafeName), Path.GetFileNameWithoutExtension(filesystemSafeName));

			// adelikat:
			// This hack is to prevent annoying things like Super Mario Bros..bk2
			if (filesystemSafeName.EndsWith("."))
			{
				filesystemSafeName = filesystemSafeName.Remove(filesystemSafeName.Length - 1, 1);
			}

			return Path.Combine(filesystemDir, filesystemSafeName);
		}

		public static string SaveRamPath(GameInfo game)
		{
			var name = FilesystemSafeName(game);
			if (Global.MovieSession.Movie.IsActive)
			{
				name += "." + Path.GetFileNameWithoutExtension(Global.MovieSession.Movie.Filename);
			}

			var pathEntry = Global.Config.PathEntries[game.System, "Save RAM"] ??
							Global.Config.PathEntries[game.System, "Base"];

			return Path.Combine(MakeAbsolutePath(pathEntry.Path, game.System), name) + ".SaveRAM";
		}
		
		public static string AutoSaveRamPath(GameInfo game)
		{
			var path = SaveRamPath(game);
			return path.Insert(path.Length - 8, ".AutoSaveRAM");
		}

		public static string RetroSaveRAMDirectory(GameInfo game)
		{
			// hijinx here to get the core name out of the game name
			var name = FilesystemSafeName(game);
			name = Path.GetDirectoryName(name);
			if (name == "")
			{
				name = FilesystemSafeName(game);
			}

			if (Global.MovieSession.Movie.IsActive)
			{
				name = Path.Combine(name, "movie-" + Path.GetFileNameWithoutExtension(Global.MovieSession.Movie.Filename));
			}

			var pathEntry = Global.Config.PathEntries[game.System, "Save RAM"] ??
							Global.Config.PathEntries[game.System, "Base"];

			return Path.Combine(MakeAbsolutePath(pathEntry.Path, game.System), name);
		}

		public static string RetroSystemPath(GameInfo game)
		{
			// hijinx here to get the core name out of the game name
			var name = FilesystemSafeName(game);
			name = Path.GetDirectoryName(name);
			if (name == "")
			{
				name = FilesystemSafeName(game);
			}

			var pathEntry = Global.Config.PathEntries[game.System, "System"] ??
							Global.Config.PathEntries[game.System, "Base"];

			return Path.Combine(MakeAbsolutePath(pathEntry.Path, game.System), name);
		}

		public static string GetGameBasePath(GameInfo game)
		{
			var name = FilesystemSafeName(game);

			var pathEntry = Global.Config.PathEntries[game.System, "Base"];
			return MakeAbsolutePath(pathEntry.Path, game.System);
		}

		public static string GetSaveStatePath(GameInfo game)
		{
			var pathEntry = Global.Config.PathEntries[game.System, "Savestates"] ??
							Global.Config.PathEntries[game.System, "Base"];

			return MakeAbsolutePath(pathEntry.Path, game.System);
		}

		public static string SaveStatePrefix(GameInfo game)
		{
			var name = FilesystemSafeName(game);

			// Neshawk and Quicknes have incompatible savestates, store the name to keep them separate
			if (Global.Emulator.SystemId == "NES")
			{
				name += "." + Global.Emulator.Attributes().CoreName;
			}

			if (Global.Emulator is Snes9x) // Keep snes9x savestate away from libsnes, we want to not be too tedious so bsnes names will just have the profile name not the core name
			{
				name += "." + Global.Emulator.Attributes().CoreName;
			}

			// Bsnes profiles have incompatible savestates so save the profile name
			if (Global.Emulator is LibsnesCore)
			{
				name += "." + (Global.Emulator as LibsnesCore).CurrentProfile;
			}

			if (Global.Emulator.SystemId == "GBA")
			{
				name += "." + Global.Emulator.Attributes().CoreName;
			}

			if (Global.MovieSession.Movie.IsActive)
			{
				name += "." + Path.GetFileNameWithoutExtension(Global.MovieSession.Movie.Filename);
			}

			var pathEntry = Global.Config.PathEntries[game.System, "Savestates"] ??
							Global.Config.PathEntries[game.System, "Base"];

			return Path.Combine(MakeAbsolutePath(pathEntry.Path, game.System), name);
		}

		public static string GetCheatsPath(GameInfo game)
		{
			var pathEntry = Global.Config.PathEntries[game.System, "Cheats"] ??
							Global.Config.PathEntries[game.System, "Base"];

			return MakeAbsolutePath(pathEntry.Path, game.System);
		}

		public static string GetPathType(string system, string type)
		{
			var path = GetPathEntryWithFallback(type, system).Path;
			return MakeAbsolutePath(path, system);
		}

		public static string ScreenshotPrefix(GameInfo game)
		{
			var name = FilesystemSafeName(game);

			var pathEntry = Global.Config.PathEntries[game.System, "Screenshots"] ??
							Global.Config.PathEntries[game.System, "Base"];

			return Path.Combine(MakeAbsolutePath(pathEntry.Path, game.System), name);
		}

		/// <summary>
		/// Takes an absolute path and attempts to convert it to a relative, based on the system, 
		/// or global base if no system is supplied, if it is not a subfolder of the base, it will return the path unaltered
		/// </summary>
		public static string TryMakeRelative(string absolutePath, string system = null)
		{
			var parentPath = string.IsNullOrWhiteSpace(system) ?
				GetGlobalBasePathAbsolute() :
				MakeAbsolutePath(GetPlatformBase(system), system);

			if (IsSubfolder(parentPath, absolutePath))
			{
				return absolutePath.Replace(parentPath, ".");
			}

			return absolutePath;
		}

		public static string MakeRelativeTo(string absolutePath, string basePath)
		{
			if (IsSubfolder(basePath, absolutePath))
			{
				return absolutePath.Replace(basePath, ".");
			}

			return absolutePath;
		}

		// http://stackoverflow.com/questions/3525775/how-to-check-if-directory-1-is-a-subdirectory-of-dir2-and-vice-versa
		private static bool IsSubfolder(string parentPath, string childPath)
		{
			var parentUri = new Uri(parentPath);

			var childUri = new DirectoryInfo(childPath).Parent;

			while (childUri != null)
			{
				if (new Uri(childUri.FullName) == parentUri)
				{
					return true;
				}

				childUri = childUri.Parent;
			}

			return false;
		}

		/// <summary>
		/// Don't only valid system ids to system ID, pathType is ROM, Screenshot, etc
		/// Returns the desired path, if does not exist, returns platform base, else it returns base
		/// </summary>
		private static PathEntry GetPathEntryWithFallback(string pathType, string systemId)
		{
			var entry = Global.Config.PathEntries[systemId, pathType];
			if (entry == null)
			{
				entry = Global.Config.PathEntries[systemId, "Base"];
			}

			if (entry == null)
			{
				entry = Global.Config.PathEntries["Global", "Base"];
			}

			return entry;
		}
	}
}
