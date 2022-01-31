﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using BizHawk.Common;
using BizHawk.Common.BufferExtensions;
using BizHawk.Common.IOExtensions;

using BizHawk.Emulation.Common;
using BizHawk.Client.Common.MovieConversionExtensions;

using BizHawk.Emulation.Cores.Nintendo.SNES;
using BizHawk.Emulation.Cores.Nintendo.SNES9X;

namespace BizHawk.Client.Common
{
	public static class MovieImport
	{
		// Movies 2.0 TODO: this is Movie.cs specific, can it be IMovie based? If not, needs to be refactored to a hardcoded 2.0 implementation, client needs to know what kind of type it imported to, or the mainform method needs to be moved here
		private const string COMMENT = "comment";
		private const string COREORIGIN = "CoreOrigin";
		private const string CRC16 = "CRC16";
		private const string CRC32 = "CRC32";
		private const string EMULATIONORIGIN = "emuOrigin";
		private const string GAMECODE = "GameCode";
		private const string INTERNALCHECKSUM = "InternalChecksum";
		private const string JAPAN = "Japan";
		private const string MD5 = "MD5";
		private const string MOVIEORIGIN = "MovieOrigin";
		private const string PORT1 = "port1";
		private const string PORT2 = "port2";
		private const string PROJECTID = "ProjectID";
		private const string SHA256 = "SHA256";
		private const string SUPERGAMEBOYMODE = "SuperGameBoyMode";
		private const string STARTSECOND = "StartSecond";
		private const string STARTSUBSECOND = "StartSubSecond";
		private const string SYNCHACK = "SyncHack";
		private const string UNITCODE = "UnitCode";

		public static void ProcessMovieImport(string fn, Action<string> conversionErrorCallback, Action<string> messageCallback)
		{
			var d = PathManager.MakeAbsolutePath(Global.Config.PathEntries.MoviesPathFragment, null);
			string errorMsg;
			string warningMsg;
			var m = ImportFile(fn, out errorMsg, out warningMsg);

			if (!string.IsNullOrWhiteSpace(errorMsg))
			{
				conversionErrorCallback(errorMsg);
			}

			if (!string.IsNullOrWhiteSpace(warningMsg))
			{
				messageCallback(warningMsg);
			}
			else
			{
				messageCallback(Path.GetFileName(fn) + " imported as " + m.Filename);
			}

			if (!Directory.Exists(d))
			{
				Directory.CreateDirectory(d);
			}
		}

		// Attempt to import another type of movie file into a movie object.
		public static Bk2Movie ImportFile(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = "";
			warningMsg = "";
			string ext = path != null ? Path.GetExtension(path).ToUpper() : "";

			if (UsesLegacyImporter(ext))
			{
				return LegacyImportFile(ext, path, out errorMsg, out warningMsg).ToBk2();
			}

			var importers = ImportersForExtension(ext);
			var importerType = importers.FirstOrDefault();

			if (importerType == default(Type))
			{
				errorMsg = "No importer found for file type " + ext;
				return null;
			}

			// Create a new instance of the importer class using the no-argument constructor
			IMovieImport importer = importerType.GetConstructor(new Type[] { })
												.Invoke(new object[] { }) as IMovieImport;

			Bk2Movie movie = null;

			try
			{
				var result = importer.Import(path);
				if (result.Errors.Count > 0)
				{
					errorMsg = result.Errors.First();
				}

				if (result.Warnings.Count > 0)
				{
					warningMsg = result.Warnings.First();
				}

				movie = result.Movie;
			}
			catch (Exception ex)
			{
				errorMsg = ex.ToString();
			}

			return movie;
		}

		private static IEnumerable<Type> ImportersForExtension(string ext)
		{
			var info = typeof(MovieImport).Module;
			var importers = from t in info.GetTypes()
			                where typeof(IMovieImport).IsAssignableFrom(t)
			                   && TypeImportsExtension(t, ext)
			                select t;

			return importers;
		}

		private static bool TypeImportsExtension(Type t, string ext)
		{
			var attrs = (ImportExtensionAttribute[])t.GetCustomAttributes(typeof(ImportExtensionAttribute), inherit: false);

			if (attrs.Any(a => a.Extension.ToUpper() == ext.ToUpper()))
			{
				return true;
			}

			return false;
		}

		private static BkmMovie LegacyImportFile(string ext, string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = "";
			warningMsg = "";

			BkmMovie m = new BkmMovie();

			try
			{
				switch (ext)
				{
					case ".FCM":
						m = ImportFcm(path, out errorMsg, out warningMsg);
						break;
					case ".FM2":
						m = ImportFm2(path, out errorMsg, out warningMsg);
						break;
					case ".FMV":
						m = ImportFmv(path, out errorMsg, out warningMsg);
						break;
					case ".GMV":
						m = ImportGmv(path, out errorMsg, out warningMsg);
						break;
					case ".LSMV":
						m = ImportLsmv(path, out errorMsg, out warningMsg);
						break;
					case ".MCM":
						m = ImportMcm(path, out errorMsg, out warningMsg);
						break;
					case ".MC2":
						m = ImportMc2(path, out errorMsg, out warningMsg);
						break;
					case ".MMV":
						m = ImportMmv(path, out errorMsg, out warningMsg);
						break;
					case ".NMV":
						m = ImportNmv(path, out errorMsg, out warningMsg);
						break;
					case ".SMV":
						m = ImportSmv(path, out errorMsg, out warningMsg);
						break;
					case ".VBM":
						m = ImportVbm(path, out errorMsg, out warningMsg);
						break;
					case ".VMV":
						m = ImportVmv(path, out errorMsg, out warningMsg);
						break;
					case ".YMV":
						m = ImportYmv(path, out errorMsg, out warningMsg);
						break;
					case ".ZMV":
						m = ImportZmv(path, out errorMsg, out warningMsg);
						break;
					case ".BKM":
						m.Filename = path;
						m.Load(false);
						break;
				}
			}
			catch (Exception except)
			{
				errorMsg = except.ToString();
			}

			m.Filename += "." + BkmMovie.Extension;

			return m;
		}

		// Return whether or not the type of file provided can currently be imported.
		public static bool IsValidMovieExtension(string extension)
		{
			// TODO: Other movie formats that don't use a legacy importer (PJM/PXM, etc),
			//       when those are implemented
			return UsesLegacyImporter(extension);
		}

		// Return whether or not the type of file provided is currently imported by a legacy (i.e. to BKM not BK2) importer
		public static bool UsesLegacyImporter(string extension)
		{
			string[] extensions =
			{
				"BKM", "FCM", "FM2", "FMV", "GMV", "MCM", "MC2", "MMV", "NMV", "LSMV", "SMV", "VBM", "VMV", "YMV", "ZMV"
			};
			return extensions.Any(ext => extension.ToUpper() == "." + ext);
		}

		// Reduce all whitespace to single spaces.
		private static string SingleSpaces(string line)
		{
			line = line.Replace("\t", " ");
			line = line.Replace("\n", " ");
			line = line.Replace("\r", " ");
			line = line.Replace("\r\n", " ");
			string prev;
			do
			{
				prev = line;
				line = line.Replace("  ", " ");
			}
			while (prev != line);
			return line;
		}

		private static IController EmptyLmsvFrame(string line)
		{
			var emptyController = new SimpleController { Definition = new ControllerDefinition { Name = "SNES Controller" } };
			emptyController["Reset"] = false;
			emptyController["Power"] = false;

			string[] buttons = { "B", "Y", "Select", "Start", "Up", "Down", "Left", "Right", "A", "X", "L", "R" };
			string[] sections = line.Split('|');
			for (int section = 2; section < sections.Length - 1; section++)
			{
				int player = section - 1; // We start with 1
				string prefix = "P" + player + " "; // "P1"

				for (int button = 0; button < buttons.Length; button++)
				{
					emptyController[prefix + buttons[button]] = false;
				}
			}

			return emptyController;
		}

		// Import a frame from a text-based format.
		private static BkmMovie ImportTextFrame(string line, int lineNum, BkmMovie m, string path, string platform, ref string warningMsg)
		{
			string[] buttons = { };
			var controller = "";
			var ext = path != null ? Path.GetExtension(path).ToUpper() : "";
			switch (ext)
			{
				case ".FM2":
					buttons = new[] { "Right", "Left", "Down", "Up", "Start", "Select", "B", "A" };
					controller = "NES Controller";
					break;
				case ".MC2":
					buttons = new[] { "Up", "Down", "Left", "Right", "B1", "B2", "Run", "Select" };
					controller = "PC Engine Controller";
					break;
				case ".LSMV":
					buttons = new[]
					{
						"B", "Y", "Select", "Start", "Up", "Down", "Left", "Right", "A", "X", "L", "R"
					};
					controller = "SNES Controller";
					if (platform == "GB" || platform == "GBC")
					{
						buttons = new[] { "A", "B", "Select", "Start", "Right", "Left", "Up", "Down" };
						controller = "Gameboy Controller";
					}

					break;
				case ".YMV":
					buttons = new[] { "Left", "Right", "Up", "Down", "Start", "A", "B", "C", "X", "Y", "Z", "L", "R" };
					controller = "Saturn Controller";
					break;
			}

			var controllers = new SimpleController { Definition = new ControllerDefinition { Name = controller } };

			// Split up the sections of the frame.
			string[] sections = line.Split('|');
			if (ext == ".FM2" && sections.Length >= 2 && sections[1].Length != 0)
			{
				controllers["Reset"] = sections[1][0] == '1';

				// Get the first invalid command warning message that arises.
				if (string.IsNullOrEmpty(warningMsg))
				{
					switch (sections[1][0])
					{
						case '0':
							break;
						case '1':
							break;
						case '2':
							if ((int)m.FrameCount != 0)
							{
								warningMsg = "hard reset";
							}

							break;
						case '4':
							warningMsg = "FDS Insert";
							break;
						case '8':
							warningMsg = "FDS Select Side";
							break;
						default:
							warningMsg = "unknown";
							break;
					}

					if (warningMsg != "")
					{
						warningMsg = "Unable to import " + warningMsg + " command on line " + lineNum + ".";
					}
				}
			}

			if (ext == ".LSMV" && sections.Length != 0)
			{
				string flags = sections[0];
				char[] off = { '.', ' ', '\t', '\n', '\r' };
				if (flags.Length == 0 || off.Contains(flags[0]))
				{
					if (warningMsg == "")
					{
						warningMsg = "Unable to import subframe.";
					}

					return m;
				}

				bool reset = flags.Length >= 2 && !off.Contains(flags[1]);
				flags = SingleSpaces(flags.Substring(2));
				if (reset && ((flags.Length >= 2 && flags[1] != '0') || (flags.Length >= 4 && flags[3] != '0')))
				{
					if (warningMsg == "")
					{
						warningMsg = "Unable to import delayed reset.";
					}

					return m;
				}

				controllers["Reset"] = reset;
			}

			/*
			 Skip the first two sections of the split, which consist of everything before the starting | and the command.
			 Do not use the section after the last |. In other words, get the sections for the players.
			*/
			int start = 2;
			int end = sections.Length - 1;
			int playerOffset = -1;
			if (ext == ".LSMV")
			{
				// LSNES frames don't start or end with a |.
				start--;
				end++;
				playerOffset++;
			}

			for (int section = start; section < end; section++)
			{
				// The player number is one less than the section number for the reasons explained above.
				int player = section + playerOffset;
				string prefix = "P" + player + " ";
				
				// Gameboy doesn't currently have a prefix saying which player the input is for.
				if (controllers.Definition.Name == "Gameboy Controller")
				{
					prefix = "";
				}

				// Only count lines with that have the right number of buttons and are for valid players.
				if (
					sections[section].Length == buttons.Length &&
					player <= BkmMnemonicConstants.Players[controllers.Definition.Name])
				{
					for (int button = 0; button < buttons.Length; button++)
					{
						// Consider the button pressed so long as its spot is not occupied by a ".".
						controllers[prefix + buttons[button]] = sections[section][button] != '.';
					}
				}
			}

			// Convert the data for the controllers to a mnemonic and add it as a frame.
			m.AppendFrame(controllers);
			return m;
		}

		// Import a subtitle from a text-based format.
		private static BkmMovie ImportTextSubtitle(string line, BkmMovie m, string path)
		{
			line = SingleSpaces(line);

			// The header name, frame, and message are separated by whitespace.
			int first = line.IndexOf(' ');
			int second = line.IndexOf(' ', first + 1);
			if (first != -1 && second != -1)
			{
				// Concatenate the frame and message with default values for the additional fields.
				string frame;
				string length;
				string ext = path != null ? Path.GetExtension(path).ToUpper() : "";

				if (ext != ".LSMV")
				{
					frame = line.Substring(first + 1, second - first - 1);
					length = "200";
				}
				else
				{
					frame = line.Substring(0, first);
					length = line.Substring(first + 1, second - first - 1);
				}

				string message = line.Substring(second + 1).Trim();
				m.Subtitles.AddFromString("subtitle " + frame + " 0 0 " + length + " FFFFFFFF " + message);
			}

			return m;
		}

		// Import a text-based movie format. This works for .FM2, .MC2, and .YMV.
		private static BkmMovie ImportText(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			var m = new BkmMovie(path);
			var file = new FileInfo(path);
			var sr = file.OpenText();
			var emulator = "";
			var platform = "";
			switch (Path.GetExtension(path).ToUpper())
			{
				case ".FM2":
					emulator = "FCEUX";
					platform = "NES";
					break;
				case ".MC2":
					emulator = "Mednafen/PCEjin";
					platform = "PCE";
					break;
				case ".YMV":
					emulator = "Yabause";
					platform = "Sega Saturn";
					break;
			}

			m.Header[HeaderKeys.PLATFORM] = platform;
			int lineNum = 0;
			string line;
			while ((line = sr.ReadLine()) != null)
			{
				lineNum++;
				if (line == "")
				{
					continue;
				}

				if (line[0] == '|')
				{
					m = ImportTextFrame(line, lineNum, m, path, platform, ref warningMsg);
					if (errorMsg != "")
					{
						sr.Close();
						return null;
					}
				}
				else if (line.ToLower().StartsWith("sub"))
				{
					m = ImportTextSubtitle(line, m, path);
				}
				else if (line.ToLower().StartsWith("emuversion"))
				{
					m.Comments.Add(
						EMULATIONORIGIN + " " + emulator + " version " + ParseHeader(line, "emuVersion"));
				}
				else if (line.ToLower().StartsWith("version"))
				{
					string version = ParseHeader(line, "version");
					m.Comments.Add(
						MOVIEORIGIN + " " + Path.GetExtension(path) + " version " + version);
					if (Path.GetExtension(path).ToUpper() == ".FM2" && version != "3")
					{
						errorMsg = ".FM2 movie version must always be 3.";
						sr.Close();
						return null;
					}
				}
				else if (line.ToLower().StartsWith("romfilename"))
				{
					m.Header[HeaderKeys.GAMENAME] = ParseHeader(line, "romFilename");
				}
				else if (line.ToLower().StartsWith("cdgamename"))
				{
					m.Header[HeaderKeys.GAMENAME] = ParseHeader(line, "cdGameName");
				}
				else if (line.ToLower().StartsWith("romchecksum"))
				{
					string blob = ParseHeader(line, "romChecksum");
					byte[] md5 = DecodeBlob(blob);
					if (md5 != null && md5.Length == 16)
					{
						m.Header[MD5] = md5.BytesToHexString().ToLower();
					}
					else
					{
						warningMsg = "Bad ROM checksum.";
					}
				}
				else if (line.ToLower().StartsWith("comment author"))
				{
					m.Header[HeaderKeys.AUTHOR] = ParseHeader(line, "comment author");
				}
				else if (line.ToLower().StartsWith("rerecordcount"))
				{
					int rerecordCount;

					// Try to parse the re-record count as an integer, defaulting to 0 if it fails.
					try
					{
						rerecordCount = int.Parse(ParseHeader(line, "rerecordCount"));
					}
					catch
					{
						rerecordCount = 0;
					}

					m.Rerecords = (ulong)rerecordCount;
				}
				else if (line.ToLower().StartsWith("guid"))
				{
					continue; // We no longer care to keep this info
				}
				else if (line.ToLower().StartsWith("startsfromsavestate"))
				{
					// If this movie starts from a savestate, we can't support it.
					if (ParseHeader(line, "StartsFromSavestate") == "1")
					{
						errorMsg = "Movies that begin with a savestate are not supported.";
						sr.Close();
						return null;
					}
				}
				else if (line.ToLower().StartsWith("palflag"))
				{
					bool pal = ParseHeader(line, "palFlag") == "1";
					m.Header[HeaderKeys.PAL] = pal.ToString();
				}
				else if (line.ToLower().StartsWith("ispal"))
				{
					bool pal = ParseHeader(line, "isPal") == "1";
					m.Header[HeaderKeys.PAL] = pal.ToString();
				}
				else if (line.ToLower().StartsWith("fourscore"))
				{
					bool fourscore = ParseHeader(line, "fourscore") == "1";
					m.Header[HeaderKeys.FOURSCORE] = fourscore.ToString();
				}
				else
				{
					// Everything not explicitly defined is treated as a comment.
					m.Comments.Add(line);
				}
			}

			sr.Close();
			return m;
		}

		// Get the content for a particular header.
		private static string ParseHeader(string line, string headerName)
		{
			// Case-insensitive search.
			int x = line.ToLower().LastIndexOf(
				headerName.ToLower()) + headerName.Length;
			string str = line.Substring(x + 1, line.Length - x - 1);
			return str.Trim();
		}

		// Decode a blob used in FM2 (base64:..., 0x123456...)
		private static byte[] DecodeBlob(string blob)
		{
			if (blob.Length < 2)
			{
				return null;
			}

			if (blob[0] == '0' && (blob[1] == 'x' || blob[1] == 'X'))
			{
				// hex
				return Util.HexStringToBytes(blob.Substring(2));
			}

			// base64
			if (!blob.ToLower().StartsWith("base64:"))
			{
				return null;
			}

			try
			{
				return Convert.FromBase64String(blob.Substring(7));
			}
			catch (FormatException)
			{
				return null;
			}
		}

		// Ends the string where a NULL character is found.
		private static string NullTerminated(string str)
		{
			int pos = str.IndexOf('\0');
			if (pos != -1)
			{
				str = str.Substring(0, pos);
			}

			return str;
		}

		// FCM file format: http://code.google.com/p/fceu/wiki/FCM
		private static BkmMovie ImportFcm(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			BkmMovie m = new BkmMovie(path);
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader r = new BinaryReader(fs);

			// 000 4-byte signature: 46 43 4D 1A "FCM\x1A"
			string signature = r.ReadStringFixedAscii(4);
			if (signature != "FCM\x1A")
			{
				errorMsg = "This is not a valid .FCM file.";
				r.Close();
				fs.Close();
				return null;
			}

			// 004 4-byte little-endian unsigned int: version number, must be 2
			uint version = r.ReadUInt32();
			if (version != 2)
			{
				errorMsg = ".FCM movie version must always be 2.";
				r.Close();
				fs.Close();
				return null;
			}

			m.Comments.Add(MOVIEORIGIN + " .FCM version " + version);

			// 008 1-byte flags
			byte flags = r.ReadByte();

			/*
			 * bit 0: reserved, set to 0
			 * bit 1:
			 * if "0", movie begins from an embedded "quicksave" snapshot
			 * if "1", movie begins from reset or power-on[1]
			*/
			if (((flags >> 1) & 0x1) == 0)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			/*
			 bit 2:
			 * if "0", NTSC timing
			 * if "1", "PAL" timing
			 Starting with version 0.98.12 released on September 19, 2004, a "PAL" flag was added to the header but
			 unfortunately it is not reliable - the emulator does not take the "PAL" setting from the ROM, but from a user
			 preference. This means that this site cannot calculate movie lengths reliably.
			*/
			bool pal = ((flags >> 2) & 0x1) != 0;
			m.Header[HeaderKeys.PAL] = pal.ToString();

			// other: reserved, set to 0
			bool syncHack = ((flags >> 4) & 0x1) != 0;
			m.Comments.Add(SYNCHACK + " " + syncHack);

			// 009 1-byte flags: reserved, set to 0
			r.ReadByte();

			// 00A 1-byte flags: reserved, set to 0
			r.ReadByte();

			// 00B 1-byte flags: reserved, set to 0
			r.ReadByte();

			// 00C 4-byte little-endian unsigned int: number of frames
			uint frameCount = r.ReadUInt32();

			// 010 4-byte little-endian unsigned int: rerecord count
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;
			/*
			 018 4-byte little-endian unsigned int: offset to the savestate inside file
			 The savestate offset is <header_size + length_of_metadata_in_bytes + padding>. The savestate offset should be
			 4-byte aligned. At the savestate offset there is a savestate file. The savestate exists even if the movie is
			 reset-based.
			*/
			r.ReadUInt32();

			// 01C 4-byte little-endian unsigned int: offset to the controller data inside file
			uint firstFrameOffset = r.ReadUInt32();

			// 020 16-byte md5sum of the ROM used
			byte[] md5 = r.ReadBytes(16);
			m.Header[MD5] = md5.BytesToHexString().ToLower();

			// 030 4-byte little-endian unsigned int: version of the emulator used
			uint emuVersion = r.ReadUInt32();
			m.Comments.Add(EMULATIONORIGIN + " FCEU " + emuVersion);

			// 034 name of the ROM used - UTF8 encoded nul-terminated string.
			List<byte> gameBytes = new List<byte>();
			while (r.PeekChar() != 0)
			{
				gameBytes.Add(r.ReadByte());
			}

			// Advance past null byte.
			r.ReadByte();
			string gameName = Encoding.UTF8.GetString(gameBytes.ToArray());
			m.Header[HeaderKeys.GAMENAME] = gameName;

			/*
			 After the header comes "metadata", which is UTF8-coded movie title string. The metadata begins after the ROM
			 name and ends at the savestate offset. This string is displayed as "Author Info" in the Windows version of the
			 emulator.
			*/
			List<byte> authorBytes = new List<byte>();
			while (r.PeekChar() != 0)
			{
				authorBytes.Add(r.ReadByte());
			}

			// Advance past null byte.
			r.ReadByte();
			string author = Encoding.UTF8.GetString(authorBytes.ToArray());
			m.Header[HeaderKeys.AUTHOR] = author;

			// Advance to first byte of input data.
			r.BaseStream.Position = firstFrameOffset;
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition { Name = "NES Controller" } };
			string[] buttons = { "A", "B", "Select", "Start", "Up", "Down", "Left", "Right" };
			bool fds = false;
			bool fourscore = false;
			int frame = 1;
			while (frame <= frameCount)
			{
				byte update = r.ReadByte();

				// aa: Number of delta bytes to follow
				int delta = (update >> 5) & 0x3;
				int frames = 0;

				/*
				 The delta byte(s) indicate the number of emulator frames between this update and the next update. It is
				 encoded in little-endian format and its size depends on the magnitude of the delta:
				 Delta of:	  Number of bytes:
				 0			  0
				 1-255		  1
				 256-65535	  2
				 65536-(2^24-1) 3
				*/
				for (int b = 0; b < delta; b++)
				{
					frames += r.ReadByte() * (int)Math.Pow(2, b * 8);
				}

				frame += frames;
				while (frames > 0)
				{
					m.AppendFrame(controllers);
					if (controllers["Reset"])
					{
						controllers["Reset"] = false;
					}

					frames--;
				}

				if (((update >> 7) & 0x1) != 0)
				{
					// Control update: 1aabbbbb
					bool reset = false;
					int command = update & 0x1F;

					// bbbbb:
					controllers["Reset"] = command == 1;
					if (warningMsg == "")
					{
						switch (command)
						{
							case 0: // Do nothing
								break;
							case 1: // Reset
								reset = true;
								break;
							case 2: // Power cycle
								reset = true;
								if (frame != 1)
								{
									warningMsg = "hard reset";
								}

								break;
							case 7: // VS System Insert Coin
								warningMsg = "VS System Insert Coin";
								break;
							case 8: // VS System Dipswitch 0 Toggle
								warningMsg = "VS System Dipswitch 0 Toggle";
								break;
							case 24: // FDS Insert
								fds = true;
								warningMsg = "FDS Insert";
								break;
							case 25: // FDS Eject
								fds = true;
								warningMsg = "FDS Eject";
								break;
							case 26: // FDS Select Side
								fds = true;
								warningMsg = "FDS Select Side";
								break;
							default:
								warningMsg = "unknown";
								break;
						}

						if (warningMsg != "")
						{
							warningMsg = "Unable to import " + warningMsg + " command at frame " + frame + ".";
						}
					}

					/*
					 1 Even if the header says "movie begins from reset", the file still contains a quicksave, and the
					 quicksave is actually loaded. This flag can't therefore be trusted. To check if the movie actually
					 begins from reset, one must analyze the controller data and see if the first non-idle command in the
					 file is a Reset or Power Cycle type control command.
					*/
					if (!reset && frame == 1)
					{
						errorMsg = "Movies that begin with a savestate are not supported.";
						r.Close();
						fs.Close();
						return null;
					}
				}
				else
				{
					/*
					 Controller update: 0aabbccc
					 * bb: Gamepad number minus one (?)
					*/
					int player = ((update >> 3) & 0x3) + 1;
					if (player > 2)
					{
						fourscore = true;
					}

					/*
					 ccc:
					 * 0	  A
					 * 1	  B
					 * 2	  Select
					 * 3	  Start
					 * 4	  Up
					 * 5	  Down
					 * 6	  Left
					 * 7	  Right
					*/
					int button = update & 0x7;

					/*
					 The controller update toggles the affected input. Controller update data is emitted to the movie file
					 only when the state of the controller changes.
					*/
					controllers["P" + player + " " + buttons[button]] = !controllers["P" + player + " " + buttons[button]];
				}
			}

			m.Header[HeaderKeys.PLATFORM] = "NES";
			if (fds)
			{
				m.Header[HeaderKeys.BOARDNAME] = "FDS";
			}

			m.Header[HeaderKeys.FOURSCORE] = fourscore.ToString();
			r.Close();
			fs.Close();
			return m;
		}

		// FM2 file format: http://www.fceux.com/web/FM2.html
		private static BkmMovie ImportFm2(string path, out string errorMsg, out string warningMsg)
		{
			return ImportText(path, out errorMsg, out warningMsg);
		}

		// FMV file format: http://tasvideos.org/FMV.html
		private static BkmMovie ImportFmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			BkmMovie m = new BkmMovie(path);
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader r = new BinaryReader(fs);

			// 000 4-byte signature: 46 4D 56 1A "FMV\x1A"
			string signature = r.ReadStringFixedAscii(4);
			if (signature != "FMV\x1A")
			{
				errorMsg = "This is not a valid .FMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			// 004 1-byte flags:
			byte flags = r.ReadByte();

			// bit 7: 0=reset-based, 1=savestate-based
			if (((flags >> 2) & 0x1) != 0)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			// other bits: unknown, set to 0
			// 005 1-byte flags:
			flags = r.ReadByte();

			// bit 5: is a FDS recording
			bool fds;
			if (((flags >> 5) & 0x1) != 0)
			{
				fds = true;
				m.Header[HeaderKeys.BOARDNAME] = "FDS";
			}
			else
			{
				fds = false;
			}

			m.Header[HeaderKeys.PLATFORM] = "NES";

			// bit 6: uses controller 2
			bool controller2 = ((flags >> 6) & 0x1) != 0;

			// bit 7: uses controller 1
			bool controller1 = ((flags >> 7) & 0x1) != 0;

			// other bits: unknown, set to 0
			// 006 4-byte little-endian unsigned int: unknown, set to 00000000
			r.ReadInt32();

			// 00A 4-byte little-endian unsigned int: rerecord count minus 1
			uint rerecordCount = r.ReadUInt32();

			/*
			 The rerecord count stored in the file is the number of times a savestate was loaded. If a savestate was never
			 loaded, the number is 0. Famtasia however displays "1" in such case. It always adds 1 to the number found in
			 the file.
			*/
			m.Rerecords = rerecordCount + 1;

			// 00E 2-byte little-endian unsigned int: unknown, set to 0000
			r.ReadInt16();

			// 010 64-byte zero-terminated emulator identifier string
			string emuVersion = NullTerminated(r.ReadStringFixedAscii(64));
			m.Comments.Add(EMULATIONORIGIN + " Famtasia version " + emuVersion);
			m.Comments.Add(MOVIEORIGIN + " .FMV");

			// 050 64-byte zero-terminated movie title string
			string description = NullTerminated(r.ReadStringFixedAscii(64));
			m.Comments.Add(COMMENT + " " + description);
			if (!controller1 && !controller2 && !fds)
			{
				warningMsg = "No input recorded.";
				r.Close();
				fs.Close();
				return m;
			}

			/*
			 The file format has no means of identifying NTSC/"PAL". It is always assumed that the game is NTSC - that is,
			 60 fps.
			*/
			m.Header[HeaderKeys.PAL] = "False";

			// 090 frame data begins here
			var controllers = new SimpleController { Definition = new ControllerDefinition { Name = "NES Controller" } };

			/*
			 * 01 Right
			 * 02 Left
			 * 04 Up
			 * 08 Down
			 * 10 B
			 * 20 A
			 * 40 Select
			 * 80 Start
			*/
			string[] buttons = { "Right", "Left", "Up", "Down", "B", "A", "Select", "Start" };
			bool[] masks = { controller1, controller2, fds };
			/*
			 The file has no terminator byte or frame count. The number of frames is the <filesize minus 144> divided by
			 <number of bytes per frame>.
			*/
			int bytesPerFrame = 0;
			for (int player = 1; player <= masks.Length; player++)
			{
				if (masks[player - 1])
				{
					bytesPerFrame++;
				}
			}

			long frameCount = (fs.Length - 144) / bytesPerFrame;
			for (long frame = 1; frame <= frameCount; frame++)
			{
				/*
				 Each frame consists of 1 or more bytes. Controller 1 takes 1 byte, controller 2 takes 1 byte, and the FDS
				 data takes 1 byte. If all three exist, the frame is 3 bytes. For example, if the movie is a regular NES game
				 with only controller 1 data, a frame is 1 byte.
				*/
				for (int player = 1; player <= masks.Length; player++)
				{
					if (!masks[player - 1])
					{
						continue;
					}

					byte controllerState = r.ReadByte();
					if (player != 3)
					{
						for (int button = 0; button < buttons.Length; button++)
						{
							controllers["P" + player + " " + buttons[button]] = ((controllerState >> button) & 0x1) != 0;
						}
					}
					else
					{
						warningMsg = "FDS commands are not properly supported.";
					}
				}

				m.AppendFrame(controllers);
			}

			r.Close();
			fs.Close();
			return m;
		}

		// GMV file format: http://code.google.com/p/gens-rerecording/wiki/GMV
		private static BkmMovie ImportGmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			var m = new BkmMovie(path);
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			var r = new BinaryReader(fs);

			// 000 16-byte signature and format version: "Gens Movie TEST9"
			string signature = r.ReadStringFixedAscii(15);
			if (signature != "Gens Movie TEST")
			{
				errorMsg = "This is not a valid .GMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			m.Header[HeaderKeys.PLATFORM] = "GEN";

			// 00F ASCII-encoded GMV file format version. The most recent is 'A'. (?)
			string version = r.ReadStringFixedAscii(1);
			m.Comments.Add(MOVIEORIGIN + " .GMV version " + version);
			m.Comments.Add(EMULATIONORIGIN + " Gens");

			// 010 4-byte little-endian unsigned int: rerecord count
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;

			// 014 ASCII-encoded controller config for player 1. '3' or '6'.
			string player1Config = r.ReadStringFixedAscii(1);

			// 015 ASCII-encoded controller config for player 2. '3' or '6'.
			string player2Config = r.ReadStringFixedAscii(1);
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition() };
			if (player1Config == "6" || player2Config == "6")
			{
				controllers.Definition.Name = "GPGX Genesis Controller";
			}
			else
			{
				controllers.Definition.Name = "GPGX 3-Button Controller";
			}

			// 016 special flags (Version A and up only)
			byte flags = r.ReadByte();

			/*
			 bit 7 (most significant): if "1", movie runs at 50 frames per second; if "0", movie runs at 60 frames per
			 second The file format has no means of identifying NTSC/"PAL", but the FPS can still be derived from the
			 header.
			*/
			bool pal = ((flags >> 7) & 0x1) != 0;
			m.Header[HeaderKeys.PAL] = pal.ToString();

			// bit 6: if "1", movie requires a savestate.
			if (((flags >> 6) & 0x1) != 0)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			// bit 5: if "1", movie is 3-player movie; if "0", movie is 2-player movie
			bool threePlayers = ((flags >> 5) & 0x1) != 0;

			// Unknown.
			r.ReadByte();

			// 018 40-byte zero-terminated ASCII movie name string
			string description = NullTerminated(r.ReadStringFixedAscii(40));
			m.Comments.Add(COMMENT + " " + description);

			/*
			 040 frame data
			 For controller bytes, each value is determined by OR-ing together values for whichever of the following are
			 left unpressed:
			 * 0x01 Up
			 * 0x02 Down
			 * 0x04 Left
			 * 0x08 Right
			 * 0x10 A
			 * 0x20 B
			 * 0x40 C
			 * 0x80 Start
			*/
			string[] buttons = { "Up", "Down", "Left", "Right", "A", "B", "C", "Start" };
			/*
			 For XYZ-mode, each value is determined by OR-ing together values for whichever of the following are left
			 unpressed:
			 * 0x01 Controller 1 X
			 * 0x02 Controller 1 Y
			 * 0x04 Controller 1 Z
			 * 0x08 Controller 1 Mode
			 * 0x10 Controller 2 X
			 * 0x20 Controller 2 Y
			 * 0x40 Controller 2 Z
			 * 0x80 Controller 2 Mode
			*/
			string[] other = { "X", "Y", "Z", "Mode" };

			// The file has no terminator byte or frame count. The number of frames is the <filesize minus 64> divided by 3.
			long frameCount = (fs.Length - 64) / 3;
			for (long frame = 1; frame <= frameCount; frame++)
			{
				// Each frame consists of 3 bytes.
				for (int player = 1; player <= 3; player++)
				{
					byte controllerState = r.ReadByte();

					// * is controller 3 if a 3-player movie, or XYZ-mode if a 2-player movie.
					if (player != 3 || threePlayers)
					{
						for (int button = 0; button < buttons.Length; button++)
						{
							controllers["P" + player + " " + buttons[button]] = ((controllerState >> button) & 0x1) == 0;
						}
					}
					else
					{
						for (int button = 0; button < other.Length; button++)
						{
							if (player1Config == "6")
							{
								controllers["P1 " + other[button]] = ((controllerState >> button) & 0x1) == 0;
							}

							if (player2Config == "6")
							{
								controllers["P2 " + other[button]] = ((controllerState >> (button + 4)) & 0x1) == 0;
							}
						}
					}
				}

				m.AppendFrame(controllers);
			}

			return m;
		}

		// LSMV file format: http://tasvideos.org/Lsnes/Movieformat.html
		private static BkmMovie ImportLsmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			var m = new BkmMovie(path);
			var hf = new HawkFile(path);

			// .LSMV movies are .zip files containing data files.
			if (!hf.IsArchive)
			{
				errorMsg = "This is not an archive.";
				return null;
			}

			string platform = "SNES";
			foreach (var item in hf.ArchiveItems)
			{
				if (item.Name == "authors")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string authors = Encoding.UTF8.GetString(stream.ReadAllBytes());
					string authorList = "";
					string authorLast = "";
					using (var reader = new StringReader(authors))
					{
						string line;

						// Each author is on a different line.
						while ((line = reader.ReadLine()) != null)
						{
							string author = line.Trim();
							if (author != "")
							{
								if (authorLast != "")
								{
									authorList += authorLast + ", ";
								}

								authorLast = author;
							}
						}
					}

					if (authorList != "")
					{
						authorList += "and ";
					}

					if (authorLast != "")
					{
						authorList += authorLast;
					}

					m.Header[HeaderKeys.AUTHOR] = authorList;
					hf.Unbind();
				}
				else if (item.Name == "coreversion")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string coreversion = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Comments.Add(COREORIGIN + " " + coreversion);
					hf.Unbind();
				}
				else if (item.Name == "gamename")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string gamename = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Header[HeaderKeys.GAMENAME] = gamename;
					hf.Unbind();
				}
				else if (item.Name == "gametype")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string gametype = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();

					// TODO: Handle the other types.
					switch (gametype)
					{
						case "gdmg":
							platform = "GB";
							break;
						case "ggbc":
						case "ggbca":
							platform = "GBC";
							break;
						case "sgb_ntsc":
						case "sgb_pal":
							platform = "SNES";
							Global.Config.GB_AsSGB = true;
							break;
					}

					bool pal = gametype == "snes_pal" || gametype == "sgb_pal";
					m.Header[HeaderKeys.PAL] = pal.ToString();
					hf.Unbind();
				}
				else if (item.Name == "input")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string input = Encoding.UTF8.GetString(stream.ReadAllBytes());

					int lineNum = 0;
					using (var reader = new StringReader(input))
					{
						lineNum++;
						string line;
						while ((line = reader.ReadLine()) != null)
						{
							if (line == "")
							{
								continue;
							}

							// Insert an empty frame in lsmv snes movies
							// https://github.com/TASVideos/BizHawk/issues/721
							// Both emulators send the input to bsnes core at the same V interval, but:
							// lsnes' frame boundary occurs at V = 241, after which the input is read;
							// BizHawk's frame boundary is just before automatic polling;
							// This isn't a great place to add this logic but this code is a mess
							if (lineNum == 1 && platform == "SNES")
							{
								// Note that this logic assumes the first non-empty log entry is a valid input log entry
								// and that it is NOT a subframe input entry.  It seems safe to assume subframe input would not be on the first line
								m.AppendFrame(EmptyLmsvFrame(line)); // line is needed to parse pipes and know the controller configuration
							}

							m = ImportTextFrame(line, lineNum, m, path, platform, ref warningMsg);
							if (errorMsg != "")
							{
								hf.Unbind();
								return null;
							}
						}
					}

					hf.Unbind();
				}
				else if (item.Name.StartsWith("moviesram."))
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					byte[] moviesram = stream.ReadAllBytes();
					if (moviesram.Length != 0)
					{
						errorMsg = "Movies that begin with SRAM are not supported.";
						hf.Unbind();
						return null;
					}

					hf.Unbind();
				}
				else if (item.Name == "port1")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string port1 = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Header[PORT1] = port1;
					hf.Unbind();
				}
				else if (item.Name == "port2")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string port2 = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Header[PORT2] = port2;
					hf.Unbind();
				}
				else if (item.Name == "projectid")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string projectid = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Header[PROJECTID] = projectid;
					hf.Unbind();
				}
				else if (item.Name == "rerecords")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string rerecords = Encoding.UTF8.GetString(stream.ReadAllBytes());
					int rerecordCount;

					// Try to parse the re-record count as an integer, defaulting to 0 if it fails.
					try
					{
						rerecordCount = int.Parse(rerecords);
					}
					catch
					{
						rerecordCount = 0;
					}

					m.Rerecords = (ulong)rerecordCount;
					hf.Unbind();
				}
				else if (item.Name.EndsWith(".sha256"))
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string rom = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					int pos = item.Name.LastIndexOf(".sha256");
					string name = item.Name.Substring(0, pos);
					m.Header[SHA256 + "_" + name] = rom;
					hf.Unbind();
				}
				else if (item.Name == "savestate")
				{
					errorMsg = "Movies that begin with a savestate are not supported.";
					return null;
				}
				else if (item.Name == "subtitles")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string subtitles = Encoding.UTF8.GetString(stream.ReadAllBytes());
					using (StringReader reader = new StringReader(subtitles))
					{
						string line;
						while ((line = reader.ReadLine()) != null)
						{
							m = ImportTextSubtitle(line, m, path);
						}
					}

					hf.Unbind();
				}
				else if (item.Name == "starttime.second")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string startSecond = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Header[STARTSECOND] = startSecond;
					hf.Unbind();
				}
				else if (item.Name == "starttime.subsecond")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string startSubSecond = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Header[STARTSUBSECOND] = startSubSecond;
					hf.Unbind();
				}
				else if (item.Name == "systemid")
				{
					hf.BindArchiveMember(item.Index);
					var stream = hf.GetStream();
					string systemid = Encoding.UTF8.GetString(stream.ReadAllBytes()).Trim();
					m.Comments.Add(EMULATIONORIGIN + " " + systemid);
					hf.Unbind();
				}
			}

			m.Header[HeaderKeys.PLATFORM] = platform;

			var ss = new LibsnesCore.SnesSyncSettings();
			m.SyncSettingsJson = ConfigService.SaveWithType(ss);
			Global.Config.SNES_InSnes9x = true; // This could be annoying to a user if they don't notice we set this preference, but the alternative is for the movie import to fail to load the movie

			return m;
		}

		/*
		 MCM file format: http://code.google.com/p/mednafen-rr/wiki/MCM
		 Mednafen-rr switched to MC2 from r261, so see r260 for details.
		*/
		private static BkmMovie ImportMcm(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			BkmMovie m = new BkmMovie(path);
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader r = new BinaryReader(fs);

			// 000 8-byte	"MDFNMOVI" signature
			var signature = r.ReadStringFixedAscii(8);
			if (signature != "MDFNMOVI")
			{
				errorMsg = "This is not a valid .MCM file.";
				r.Close();
				fs.Close();
				return null;
			}

			// 008 uint32	 Mednafen Version (Current is 0A 08)
			uint emuVersion = r.ReadUInt32();
			m.Comments.Add(EMULATIONORIGIN + " Mednafen " + emuVersion);

			// 00C uint32	 Movie Format Version (Current is 01)
			uint version = r.ReadUInt32();
			m.Comments.Add(MOVIEORIGIN + " .MCM version " + version);

			// 010 32-byte	MD5 of the ROM used
			byte[] md5 = r.ReadBytes(16);

			// Discard the second 16 bytes.
			r.ReadBytes(16);
			m.Header[MD5] = md5.BytesToHexString().ToLower();

			// 030 64-byte	Filename of the ROM used (with extension)
			string gameName = NullTerminated(r.ReadStringFixedAscii(64));
			m.Header[HeaderKeys.GAMENAME] = gameName;

			// 070 uint32	 Re-record Count
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;

			// 074 5-byte	 Console indicator (pce, ngp, pcfx, wswan)
			string platform = NullTerminated(r.ReadStringFixedAscii(5));
			Dictionary<string, Dictionary<string, object>> platforms = new Dictionary<string, Dictionary<string, object>>
			{
				// Normally, NES receives from 5 input ports, where the first 4 have a length of 1 byte, and the last has
				// a length of 0. For the sake of simplicity, it is interpreted as 4 ports of 1 byte length for
				// re-recording.
				["nes"] = new Dictionary<string, object>
				{
					["name"] = "NES",
					["ports"] = 4,
					["bytesPerPort"] = 1,
					["buttons"] = new[] { "A", "B", "Select", "Start", "Up", "Down", "Left", "Right" }
				},
				["pce"] = new Dictionary<string, object>
				{
					["name"] = "PC Engine",
					["ports"] = 5,
					["bytesPerPort"] = 2,
					["buttons"] = new[] { "B1", "B2", "Select", "Run", "Up", "Right", "Down", "Left" }
				},
				["lynx"] = new Dictionary<string, object>
				{
					["name"] = "Lynx",
					["ports"] = 2,
					["bytesPerPort"] = 1,
					["buttons"] = new[] { "A", "B", "Up", "Down", "Left", "Right" }
				}
			};
			if (!platforms.ContainsKey(platform))
			{
				errorMsg = "Platform " + platform + " not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			string name = (string)platforms[platform]["name"];

			string systemId = name.ToUpper(); // Hack

			m.Header[HeaderKeys.PLATFORM] = systemId;

			// 079 32-byte	Author name
			string author = NullTerminated(r.ReadStringFixedAscii(32));
			m.Header[HeaderKeys.AUTHOR] = author;

			// 099 103-byte   Padding 0s
			r.ReadBytes(103);

			// TODO: Verify if NTSC/"PAL" mode used for the movie can be detected or not.
			// 100 variable   Input data
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition { Name = name + " Controller" } };
			int bytes = 256;

			// The input stream consists of 1 byte for power-on and reset, and then X bytes per each input port per frame.
			if (platform == "nes")
			{
				// Power-on.
				r.ReadByte();
				bytes++;
			}

			string[] buttons = (string[])platforms[platform]["buttons"];
			int ports = (int)platforms[platform]["ports"];
			int bytesPerPort = (int)platforms[platform]["bytesPerPort"];

			// Frame Size (with Control Byte)
			int size = (ports * bytesPerPort) + 1;
			long frameCount = (fs.Length - bytes) / size;
			for (int frame = 1; frame <= frameCount; frame++)
			{
				for (int player = 1; player <= ports; player++)
				{
					if (bytesPerPort == 2)
					{
						// Discard the first byte.
						r.ReadByte();
					}

					ushort controllerState = r.ReadByte();
					for (int button = 0; button < buttons.Length; button++)
					{
						string prefix = platform == "lynx" ? "" : "P" + player + " "; // hack
						controllers[prefix + buttons[button]] = ((controllerState >> button) & 0x1) != 0;
					}
				}

				r.ReadByte();
				if (platform == "nes" && warningMsg == "")
				{
					warningMsg = "Control commands are not properly supported.";
				}

				m.AppendFrame(controllers);
			}

			r.Close();
			fs.Close();
			return m;
		}

		// MC2 file format: http://code.google.com/p/pcejin/wiki/MC2
		private static BkmMovie ImportMc2(string path, out string errorMsg, out string warningMsg)
		{
			return ImportText(path, out errorMsg, out warningMsg);
		}

		// MMV file format: http://tasvideos.org/MMV.html
		private static BkmMovie ImportMmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			BkmMovie m = new BkmMovie(path);
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader r = new BinaryReader(fs);

			// 0000: 4-byte signature: "MMV\0"
			string signature = r.ReadStringFixedAscii(4);
			if (signature != "MMV\0")
			{
				errorMsg = "This is not a valid .MMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			// 0004: 4-byte little endian unsigned int: dega version
			uint emuVersion = r.ReadUInt32();
			m.Comments.Add(EMULATIONORIGIN + " Dega version " + emuVersion);
			m.Comments.Add(MOVIEORIGIN + " .MMV");

			// 0008: 4-byte little endian unsigned int: frame count
			uint frameCount = r.ReadUInt32();

			// 000c: 4-byte little endian unsigned int: rerecord count
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;

			// 0010: 4-byte little endian flag: begin from reset?
			uint reset = r.ReadUInt32();
			if (reset == 0)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			// 0014: 4-byte little endian unsigned int: offset of state information
			r.ReadUInt32();

			// 0018: 4-byte little endian unsigned int: offset of input data
			r.ReadUInt32();

			// 001c: 4-byte little endian unsigned int: size of input packet
			r.ReadUInt32();

			// 0020-005f: string: author info (UTF-8)
			string author = NullTerminated(r.ReadStringFixedAscii(64));
			m.Header[HeaderKeys.AUTHOR] = author;

			// 0060: 4-byte little endian flags
			byte flags = r.ReadByte();

			// bit 0: unused
			// bit 1: "PAL"
			bool pal = ((flags >> 1) & 0x1) != 0;
			m.Header[HeaderKeys.PAL] = pal.ToString();

			// bit 2: Japan
			bool japan = ((flags >> 2) & 0x1) != 0;
			m.Header[JAPAN] = japan.ToString();

			// bit 3: Game Gear (version 1.16+)
			bool gamegear;
			if (((flags >> 3) & 0x1) != 0)
			{
				gamegear = true;
				m.Header[HeaderKeys.PLATFORM] = "GG";
			}
			else
			{
				gamegear = false;
				m.Header[HeaderKeys.PLATFORM] = "SMS";
			}

			// bits 4-31: unused
			r.ReadBytes(3);

			// 0064-00e3: string: rom name (ASCII)
			string gameName = NullTerminated(r.ReadStringFixedAscii(128));
			m.Header[HeaderKeys.GAMENAME] = gameName;

			// 00e4-00f3: binary: rom MD5 digest
			byte[] md5 = r.ReadBytes(16);
			m.Header[MD5] = $"{md5.BytesToHexString().ToLower():x8}";
			var controllers = new SimpleController { Definition = new ControllerDefinition { Name = "SMS Controller" } };

			/*
			 76543210
			 * bit 0 (0x01): up
			 * bit 1 (0x02): down
			 * bit 2 (0x04): left
			 * bit 3 (0x08): right
			 * bit 4 (0x10): 1
			 * bit 5 (0x20): 2
			 * bit 6 (0x40): start (Master System)
			 * bit 7 (0x80): start (Game Gear)
			*/
			string[] buttons = { "Up", "Down", "Left", "Right", "B1", "B2" };
			for (int frame = 1; frame <= frameCount; frame++)
			{
				/*
				 Controller data is made up of one input packet per frame. Each packet currently consists of 2 bytes. The
				 first byte is for controller 1 and the second controller 2. The Game Gear only uses the controller 1 input
				 however both bytes are still present.
				*/
				for (int player = 1; player <= 2; player++)
				{
					byte controllerState = r.ReadByte();
					for (int button = 0; button < buttons.Length; button++)
					{
						controllers["P" + player + " " + buttons[button]] = ((controllerState >> button) & 0x1) != 0;
					}

					if (player == 1)
					{
						controllers["Pause"] = 
							(((controllerState >> 6) & 0x1) != 0 && (!gamegear))
							|| (((controllerState >> 7) & 0x1) != 0 && gamegear);
					}
				}

				m.AppendFrame(controllers);
			}

			r.Close();
			fs.Close();
			return m;
		}

		// NMV file format: http://tasvideos.org/NMV.html
		private static BkmMovie ImportNmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			var m = new BkmMovie(path);
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			var r = new BinaryReader(fs);

			// 000 4-byte signature: 4E 53 53 1A "NSS\x1A"
			string signature = r.ReadStringFixedAscii(4);
			if (signature != "NSS\x1A")
			{
				errorMsg = "This is not a valid .NMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			// 004 4-byte version string (example "0960")
			string emuVersion = r.ReadStringFixedAscii(4);
			m.Comments.Add(EMULATIONORIGIN + " Nintendulator version " + emuVersion);
			m.Comments.Add(MOVIEORIGIN + " .NMV");

			// 008 4-byte file size, not including the 16-byte header
			r.ReadUInt32();

			/*
			 00C 4-byte file type string
			 * "NSAV" - standard savestate
			 * "NREC" - savestate saved during movie recording
			 * "NMOV" - standalone movie file
			*/
			string type = r.ReadStringFixedAscii(4);
			if (type != "NMOV")
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			/*
			 Individual blocks begin with an 8-byte header, consisting of a 4-byte signature and a 4-byte length (which
			 does not include the length of the block header).
			 The final block in the file is of type "NMOV"
			*/
			string header = r.ReadStringFixedAscii(4);
			if (header != "NMOV")
			{
				errorMsg = "This is not a valid .NMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			r.ReadUInt32();

			// 000 1-byte controller #1 type (see below)
			byte controller1 = r.ReadByte();

			// 001 1-byte controller #2 type (or four-score mask, see below)
			byte controller2 = r.ReadByte();

			/*
			 Controller data is variant, depending on which controllers are attached at the time of recording. The
			 following controllers are implemented:
			 * 0 - Unconnected
			 * 1 - Standard Controller (1 byte)
			 * 2 - Zapper (3 bytes)
			 * 3 - Arkanoid Paddle (2 bytes)
			 * 4 - Power Pad (2 bytes)
			 * 5 - Four-Score (special)
			 * 6 - SNES controller (2 bytes) - A/B become B/Y, adds A/X and L/R shoulder buttons
			 * 7 - Vs Unisystem Zapper (3 bytes)
			*/
			bool fourscore = controller1 == 5;
			m.Header[HeaderKeys.FOURSCORE] = fourscore.ToString();
			bool[] masks = { false, false, false, false, false };
			if (fourscore)
			{
				/*
				 When a Four-Score is indicated for Controller #1, the Controller #2 byte becomes a bit mask to indicate
				 which ports on the Four-Score have controllers connected to them. Each connected controller stores 1 byte
				 per frame. Nintendulator's Four-Score recording is seemingly broken.
				*/
				for (int controller = 1; controller < masks.Length; controller++)
				{
					masks[controller - 1] = ((controller2 >> (controller - 1)) & 0x1) != 0;
				}

				warningMsg = "Nintendulator's Four Score recording is seemingly broken.";
			}
			else
			{
				byte[] types = { controller1, controller2 };
				for (int controller = 1; controller <= types.Length; controller++)
				{
					masks[controller - 1] = types[controller - 1] == 1;

					// Get the first unsupported controller warning message that arises.
					if (warningMsg == "")
					{
						switch (types[controller - 1])
						{
							case 0:
								break;
							case 2:
								warningMsg = "Zapper";
								break;
							case 3:
								warningMsg = "Arkanoid Paddle";
								break;
							case 4:
								warningMsg = "Power Pad";
								break;
							case 5:
								warningMsg = "A Four Score in the second controller port is invalid.";
								continue;
							case 6:
								warningMsg = "SNES controller";
								break;
							case 7:
								warningMsg = "Vs Unisystem Zapper";
								break;
						}

						if (warningMsg != "")
						{
							warningMsg = warningMsg + " is not properly supported.";
						}
					}
				}
			}

			// 002 1-byte expansion port controller type
			byte expansion = r.ReadByte();

			/*
			 The expansion port can potentially have an additional controller connected. The following expansion
			 controllers are implemented:
			 * 0 - Unconnected
			 * 1 - Famicom 4-player adapter (2 bytes)
			 * 2 - Famicom Arkanoid paddle (2 bytes)
			 * 3 - Family Basic Keyboard (currently does not support demo recording)
			 * 4 - Alternate keyboard layout (currently does not support demo recording)
			 * 5 - Family Trainer (2 bytes)
			 * 6 - Oeka Kids writing tablet (3 bytes)
			*/
			string[] expansions =
			{
				"Unconnected", "Famicom 4-player adapter", "Famicom Arkanoid paddle", "Family Basic Keyboard",
				"Alternate keyboard layout", "Family Trainer", "Oeka Kids writing tablet"
			};
			if (expansion != 0 && warningMsg == "")
			{
				warningMsg = "Expansion port is not properly supported. This movie uses " + expansions[expansion] + ".";
			}

			// 003 1-byte number of bytes per frame, plus flags
			byte data = r.ReadByte();
			int bytesPerFrame = data & 0xF;
			int bytes = 0;
			for (int controller = 1; controller < masks.Length; controller++)
			{
				if (masks[controller - 1])
				{
					bytes++;
				}
			}

			/*
			 Depending on the mapper used by the game in question, an additional byte of data may be stored during each
			 frame. This is most frequently used for FDS games (storing either the disk number or 0xFF to eject) or VS
			 Unisystem coin/DIP switch toggles (limited to 1 action per frame). This byte exists if the bytes per frame do
			 not match up with the amount of bytes the controllers take up.
			*/
			if (bytes != bytesPerFrame)
			{
				masks[4] = true;
			}

			// bit 6: Game Genie active
			/*
			 bit 7: Framerate
			 * if "0", NTSC timing
			 * if "1", "PAL" timing
			*/
			bool pal = ((data >> 7) & 0x1) != 0;
			m.Header[HeaderKeys.PAL] = pal.ToString();

			// 004 4-byte little-endian unsigned int: rerecord count
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;

			/*
			 008 4-byte little-endian unsigned int: length of movie description
			 00C (variable) null-terminated UTF-8 text, movie description (currently not implemented)
			*/
			string movieDescription = NullTerminated(r.ReadStringFixedAscii((int)r.ReadUInt32()));
			m.Comments.Add(COMMENT + " " + movieDescription);

			// ... 4-byte little-endian unsigned int: length of controller data in bytes
			uint length = r.ReadUInt32();

			// ... (variable) controller data
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition { Name = "NES Controller" } };
			/*
			 Standard controllers store data in the following format:
			 * 01: A
			 * 02: B
			 * 04: Select
			 * 08: Start
			 * 10: Up
			 * 20: Down
			 * 40: Left
			 * 80: Right
			 Other controllers store data in their own formats, and are beyond the scope of this document.
			*/
			string[] buttons = { "A", "B", "Select", "Start", "Up", "Down", "Left", "Right" };

			// The controller data contains <number_of_bytes> / <bytes_per_frame> frames.
			long frameCount = length / bytesPerFrame;
			for (int frame = 1; frame <= frameCount; frame++)
			{
				// Controller update data is emitted to the movie file during every frame.
				for (int player = 1; player <= masks.Length; player++)
				{
					if (!masks[player - 1])
					{
						continue;
					}

					byte controllerState = r.ReadByte();
					if (player != 5)
					{
						for (int button = 0; button < buttons.Length; button++)
						{
							controllers["P" + player + " " + buttons[button]] = ((controllerState >> button) & 0x1) != 0;
						}
					}
					else if (warningMsg == "")
					{
						warningMsg = "Extra input is not properly supported.";
					}
				}

				m.AppendFrame(controllers);
			}

			r.Close();
			fs.Close();
			return m;
		}

		private static BkmMovie ImportSmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			BkmMovie m = new BkmMovie(path);
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader r = new BinaryReader(fs);

			// 000 4-byte signature: 53 4D 56 1A "SMV\x1A"
			string signature = r.ReadStringFixedAscii(4);
			if (signature != "SMV\x1A")
			{
				errorMsg = "This is not a valid .SMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			m.Header[HeaderKeys.PLATFORM] = "SNES";

			// 004 4-byte little-endian unsigned int: version number
			uint versionNumber = r.ReadUInt32();
			string version;
			switch (versionNumber)
			{
				case 1:
					version = "1.43";
					break;
				case 4:
					version = "1.51";
					break;
				case 5:
					version = "1.52";
					break;
				default:
					errorMsg = "SMV version not recognized. 1.43, 1.51, and 1.52 are currently supported.";
					r.Close();
					fs.Close();
					return null;
			}

			m.Comments.Add(EMULATIONORIGIN + " Snes9x version " + version);
			m.Comments.Add(MOVIEORIGIN + " .SMV");
			/*
			 008 4-byte little-endian integer: movie "uid" - identifies the movie-savestate relationship, also used as the
			 recording time in Unix epoch format
			*/
			uint uid = r.ReadUInt32();

			// 00C 4-byte little-endian unsigned int: rerecord count
			m.Rerecords = r.ReadUInt32();

			// 010 4-byte little-endian unsigned int: number of frames
			uint frameCount = r.ReadUInt32();

			// 014 1-byte flags "controller mask"
			byte controllerFlags = r.ReadByte();
			/*
			 * bit 0: controller 1 in use
			 * bit 1: controller 2 in use
			 * bit 2: controller 3 in use
			 * bit 3: controller 4 in use
			 * bit 4: controller 5 in use
			 * other: reserved, set to 0
			*/
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition { Name = "SNES Controller" } };
			bool[] controllersUsed = new bool[5];
			for (int controller = 1; controller <= controllersUsed.Length; controller++)
			{
				controllersUsed[controller - 1] = ((controllerFlags >> (controller - 1)) & 0x1) != 0;
			}

			// 015 1-byte flags "movie options"
			byte movieFlags = r.ReadByte();
			/*
				 bit 0:
					 if "0", movie begins from an embedded "quicksave" snapshot
					 if "1", a SRAM is included instead of a quicksave; movie begins from reset
			*/
			if ((movieFlags & 0x1) == 0)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			// bit 1: if "0", movie is NTSC (60 fps); if "1", movie is PAL (50 fps)
			bool pal = ((movieFlags >> 1) & 0x1) != 0;
			m.Header[HeaderKeys.PAL] = pal.ToString();

			// other: reserved, set to 0
			/*
			 016 1-byte flags "sync options":
				 bit 0: MOVIE_SYNC2_INIT_FASTROM
				 other: reserved, set to 0
			*/
			r.ReadByte();
			/*
			 017 1-byte flags "sync options":
				 bit 0: MOVIE_SYNC_DATA_EXISTS
					 if "1", all sync options flags are defined.
					 if "0", all sync options flags have no meaning.
				 bit 1: MOVIE_SYNC_WIP1TIMING
				 bit 2: MOVIE_SYNC_LEFTRIGHT
				 bit 3: MOVIE_SYNC_VOLUMEENVX
				 bit 4: MOVIE_SYNC_FAKEMUTE
				 bit 5: MOVIE_SYNC_SYNCSOUND
				 bit 6: MOVIE_SYNC_HASROMINFO
					 if "1", there is extra ROM info located right in between of the metadata and the savestate.
				 bit 7: set to 0.
			*/
			byte syncFlags = r.ReadByte();
			/*
			 Extra ROM info is always positioned right before the savestate. Its size is 30 bytes if MOVIE_SYNC_HASROMINFO
			 is used (and MOVIE_SYNC_DATA_EXISTS is set), 0 bytes otherwise.
			*/
			int extraRomInfo = (((syncFlags >> 6) & 0x1) != 0 && (syncFlags & 0x1) != 0) ? 30 : 0;

			// 018 4-byte little-endian unsigned int: offset to the savestate inside file
			uint savestateOffset = r.ReadUInt32();

			// 01C 4-byte little-endian unsigned int: offset to the controller data inside file
			uint firstFrameOffset = r.ReadUInt32();
			int[] controllerTypes = new int[2];

			// The (.SMV 1.51 and up) header has an additional 32 bytes at the end
			if (version != "1.43")
			{
				// 020 4-byte little-endian unsigned int: number of input samples, primarily for peripheral-using games
				r.ReadBytes(4);
				/*
				 024 2 1-byte unsigned ints: what type of controller is plugged into ports 1 and 2 respectively: 0=NONE,
				 1=JOYPAD, 2=MOUSE, 3=SUPERSCOPE, 4=JUSTIFIER, 5=MULTITAP
				*/
				controllerTypes[0] = r.ReadByte();
				controllerTypes[1] = r.ReadByte();

				// 026 4 1-byte signed ints: controller IDs of port 1, or -1 for unplugged
				r.ReadBytes(4);

				// 02A 4 1-byte signed ints: controller IDs of port 2, or -1 for unplugged
				r.ReadBytes(4);

				// 02E 18 bytes: reserved for future use
				r.ReadBytes(18);
			}

			/*
			 After the header comes "metadata", which is UTF16-coded movie title string (author info). The metadata begins
			 from position 32 (0x20 (0x40 for 1.51 and up)) and ends at <savestate_offset -
			 length_of_extra_rom_info_in_bytes>.
			*/
			byte[] metadata = r.ReadBytes((int)(savestateOffset - extraRomInfo - ((version != "1.43") ? 0x40 : 0x20)));
			string author = NullTerminated(Encoding.Unicode.GetString(metadata).Trim());
			if (author != "")
			{
				m.Header[HeaderKeys.AUTHOR] = author;
			}

			if (extraRomInfo == 30)
			{
				// 000 3 bytes of zero padding: 00 00 00 003 4-byte integer: CRC32 of the ROM 007 23-byte ascii string
				r.ReadBytes(3);
				int crc32 = r.ReadInt32();
				m.Header[CRC32] = crc32.ToString();

				// the game name copied from the ROM, truncated to 23 bytes (the game name in the ROM is 21 bytes)
				string gameName = NullTerminated(Encoding.UTF8.GetString(r.ReadBytes(23)));
				m.Header[HeaderKeys.GAMENAME] = gameName;
			}

			r.BaseStream.Position = firstFrameOffset;
			/*
			 01 00 (reserved)
			 02 00 (reserved)
			 04 00 (reserved)
			 08 00 (reserved)
			 10 00 R
			 20 00 L
			 40 00 X
			 80 00 A
			 00 01 Right
			 00 02 Left
			 00 04 Down
			 00 08 Up
			 00 10 Start
			 00 20 Select
			 00 40 Y
			 00 80 B
			*/
			string[] buttons =
			{
				"Right", "Left", "Down", "Up", "Start", "Select", "Y", "B", "R", "L", "X", "A"
			};

			for (int frame = 0; frame <= frameCount; frame++)
			{
				controllers["Reset"] = true;
				for (int player = 1; player <= controllersUsed.Length; player++)
				{
					if (!controllersUsed[player - 1])
					{
						continue;
					}

					/*
					 Each frame consists of 2 bytes per controller. So if there are 3 controllers, a frame is 6 bytes and
					 if there is only 1 controller, a frame is 2 bytes.
					*/
					byte controllerState1 = r.ReadByte();
					byte controllerState2 = r.ReadByte();

					/*
					 In the reset-recording patch, a frame that contains the value FF FF for every controller denotes a
					 reset. The reset is done through the S9xSoftReset routine.
					*/
					if (controllerState1 != 0xFF || controllerState2 != 0xFF)
					{
						controllers["Reset"] = false;
					}

					/*
					 While the meaning of controller data (for 1.51 and up) for a single standard SNES controller pad
					 remains the same, each frame of controller data can contain additional bytes if input for peripherals
					 is being recorded.
					*/
					if (version != "1.43" && player <= controllerTypes.Length)
					{
						var peripheral = "";
						switch (controllerTypes[player - 1])
						{
							case 0: // NONE
								continue;
							case 1: // JOYPAD
								break;
							case 2: // MOUSE
								peripheral = "Mouse";

								// 5*num_mouse_ports
								r.ReadBytes(5);
								break;
							case 3: // SUPERSCOPE
								peripheral = "Super Scope"; // 6*num_superscope_ports
								r.ReadBytes(6);
								break;
							case 4: // JUSTIFIER
								peripheral = "Justifier";

								// 11*num_justifier_ports
								r.ReadBytes(11);
								break;
							case 5: // MULTITAP
								peripheral = "Multitap";
								break;
						}

						if (peripheral != "" && warningMsg == "")
						{
							warningMsg = "Unable to import " + peripheral + ".";
						}
					}

					ushort controllerState = (ushort)(((controllerState1 << 4) & 0x0F00) | controllerState2);
					if (player <= BkmMnemonicConstants.Players[controllers.Definition.Name])
					{
						for (int button = 0; button < buttons.Length; button++)
						{
							controllers["P" + player + " " + buttons[button]] =
								((controllerState >> button) & 0x1) != 0;
						}
					}
					else if (warningMsg == "")
					{
						warningMsg = "Controller " + player + " not supported.";
					}
				}

				// The controller data contains <number_of_frames + 1> frames.
				if (frame == 0)
				{
					continue;
				}

				m.AppendFrame(controllers);
			}

			r.Close();
			fs.Close();

			var ss = new Snes9x.SyncSettings();
			m.SyncSettingsJson = ConfigService.SaveWithType(ss);
			Global.Config.SNES_InSnes9x = true; // This could be annoying to a user if they don't notice we set this preference, but the alternative is for the movie import to fail to load the movie
			return m;
		}
		
		// VBM file format: http://code.google.com/p/vba-rerecording/wiki/VBM
		private static BkmMovie ImportVbm(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			var m = new BkmMovie(path);
			var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			var r = new BinaryReader(fs);

			// 000 4-byte signature: 56 42 4D 1A "VBM\x1A"
			string signature = r.ReadStringFixedAscii(4);
			if (signature != "VBM\x1A")
			{
				errorMsg = "This is not a valid .VBM file.";
				r.Close();
				fs.Close();
				return null;
			}

			// 004 4-byte little-endian unsigned int: major version number, must be "1"
			uint majorVersion = r.ReadUInt32();
			if (majorVersion != 1)
			{
				errorMsg = ".VBM major movie version must be 1.";
				r.Close();
				fs.Close();
				return null;
			}

			/*
			 008 4-byte little-endian integer: movie "uid" - identifies the movie-savestate relationship, also used as the
			 recording time in Unix epoch format
			*/
			uint uid = r.ReadUInt32();

			// 00C 4-byte little-endian unsigned int: number of frames
			uint frameCount = r.ReadUInt32();

			// 010 4-byte little-endian unsigned int: rerecord count
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;

			// 014 1-byte flags: (movie start flags)
			byte flags = r.ReadByte();

			// bit 0: if "1", movie starts from an embedded "quicksave" snapshot
			bool startfromquicksave = (flags & 0x1) != 0;

			// bit 1: if "1", movie starts from reset with an embedded SRAM
			bool startfromsram = ((flags >> 1) & 0x1) != 0;

			// other: reserved, set to 0
			// (If both bits 0 and 1 are "1", the movie file is invalid)
			if (startfromquicksave && startfromsram)
			{
				errorMsg = "This is not a valid .VBM file.";
				r.Close();
				fs.Close();
				return null;
			}

			if (startfromquicksave)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			if (startfromsram)
			{
				errorMsg = "Movies that begin with SRAM are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			// 015 1-byte flags: controller flags
			byte controllerFlags = r.ReadByte();
			/*
			 * bit 0: controller 1 in use
			 * bit 1: controller 2 in use (SGB games can be 2-player multiplayer)
			 * bit 2: controller 3 in use (SGB games can be 3- or 4-player multiplayer with multitap)
			 * bit 3: controller 4 in use (SGB games can be 3- or 4-player multiplayer with multitap)
			*/
			bool[] controllersUsed = new bool[4];
			for (int controller = 1; controller <= controllersUsed.Length; controller++)
			{
				controllersUsed[controller - 1] = ((controllerFlags >> (controller - 1)) & 0x1) != 0;
			}

			if (!controllersUsed[0])
			{
				errorMsg = "Controller 1 must be in use.";
				r.Close();
				fs.Close();
				return null;
			}

			// other: reserved
			// 016 1-byte flags: system flags (game always runs at 60 frames/sec)
			flags = r.ReadByte();

			// bit 0: if "1", movie is for the GBA system
			bool isGBA = (flags & 0x1) != 0;

			// bit 1: if "1", movie is for the GBC system
			bool isGBC = ((flags >> 1) & 0x1) != 0;

			// bit 2: if "1", movie is for the SGB system
			bool isSGB = ((flags >> 2) & 0x1) != 0;

			// other: reserved, set to 0

			// (At most one of bits 0, 1, 2 can be "1")
			////if (!(is_gba ^ is_gbc ^ is_sgb) && (is_gba || is_gbc || is_sgb)) //TODO: adelikat: this doesn't do what the comment above suggests it is trying to check for, it is always false!
			////{
			////errorMsg = "This is not a valid .VBM file.";
			////r.Close();
			////fs.Close();
			////return null;
			////}

			// (If all 3 of these bits are "0", it is for regular GB.)
			string platform = "GB";
			if (isGBA)
			{
				platform = "GBA";
			}

			if (isGBC)
			{
				platform = "GBC";
			}

			if (isSGB)
			{
				m.Comments.Add(SUPERGAMEBOYMODE + " True");
			}

			m.Header[HeaderKeys.PLATFORM] = platform;

			// 017 1-byte flags: (values of some boolean emulator options)
			flags = r.ReadByte();
			/*
			 * bit 0: (useBiosFile) if "1" and the movie is of a GBA game, the movie was made using a GBA BIOS file.
			 * bit 1: (skipBiosFile) if "0" and the movie was made with a GBA BIOS file, the BIOS intro is included in the
			 * movie.
			 * bit 2: (rtcEnable) if "1", the emulator "real time clock" feature was enabled.
			 * bit 3: (unsupported) must be "0" or the movie file is considered invalid (legacy).
			*/
			if (((flags >> 3) & 0x1) != 0)
			{
				errorMsg = "This is not a valid .VBM file.";
				r.Close();
				fs.Close();
				return null;
			}

			/*
			 * bit 4: (lagReduction) if "0" and the movie is of a GBA game, the movie was made using the old excessively
			 * laggy GBA timing.
			 * bit 5: (gbcHdma5Fix) if "0" and the movie is of a GBC game, the movie was made using the old buggy HDMA5
			 * timing.
			 * bit 6: (echoRAMFix)  if "1" and the movie is of a GB, GBC, or SGB game, the movie was made with Echo RAM
			 * Fix on, otherwise it was made with Echo RAM Fix off.
			 * bit 7: reserved, set to 0.
			*/
			/*
			 018 4-byte little-endian unsigned int: theApp.winSaveType (value of that emulator option)
			 01C 4-byte little-endian unsigned int: theApp.winFlashSize (value of that emulator option)
			 020 4-byte little-endian unsigned int: gbEmulatorType (value of that emulator option)
			*/
			r.ReadBytes(12);
			/*
			 024 12-byte character array: the internal game title of the ROM used while recording, not necessarily
			 null-terminated (ASCII?)
			*/
			string gameName = NullTerminated(r.ReadStringFixedAscii(12));
			m.Header[HeaderKeys.GAMENAME] = gameName;

			// 030 1-byte unsigned char: minor version/revision number of current VBM version, the latest is "1"
			byte minorVersion = r.ReadByte();
			m.Comments.Add(MOVIEORIGIN + " .VBM version " + majorVersion + "." + minorVersion);
			m.Comments.Add(EMULATIONORIGIN + " Visual Boy Advance");

			// 031 1-byte unsigned char: the internal CRC of the ROM used while recording
			r.ReadByte();
			/*
			 032 2-byte little-endian unsigned short: the internal Checksum of the ROM used while recording, or a
			 calculated CRC16 of the BIOS if GBA
			*/
			ushort checksumCRC16 = r.ReadUInt16();
			/*
			 034 4-byte little-endian unsigned int: the Game Code of the ROM used while recording, or the Unit Code if not
			 GBA
			*/
			uint gameCodeUnitCode = r.ReadUInt32();
			if (platform == "GBA")
			{
				m.Header[CRC16] = checksumCRC16.ToString();
				m.Header[GAMECODE] = gameCodeUnitCode.ToString();
			}
			else
			{
				m.Header[INTERNALCHECKSUM] = checksumCRC16.ToString();
				m.Header[UNITCODE] = gameCodeUnitCode.ToString();
			}

			// 038 4-byte little-endian unsigned int: offset to the savestate or SRAM inside file, set to 0 if unused
			r.ReadBytes(4);

			// 03C 4-byte little-endian unsigned int: offset to the controller data inside file
			uint firstFrameOffset = r.ReadUInt32();

			// After the header is 192 bytes of text. The first 64 of these 192 bytes are for the author's name (or names).
			string author = NullTerminated(r.ReadStringFixedAscii(64));
			m.Header[HeaderKeys.AUTHOR] = author;

			// The following 128 bytes are for a description of the movie. Both parts must be null-terminated.
			string movieDescription = NullTerminated(r.ReadStringFixedAscii(128));
			m.Comments.Add(COMMENT + " " + movieDescription);
			r.BaseStream.Position = firstFrameOffset;
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition() };
			controllers.Definition.Name = platform != "GBA"
				? "Gameboy Controller"
				: "GBA Controller";

			/*
			 * 01 00 A
			 * 02 00 B
			 * 04 00 Select
			 * 08 00 Start
			 * 10 00 Right
			 * 20 00 Left
			 * 40 00 Up
			 * 80 00 Down
			 * 00 01 R
			 * 00 02 L
			*/
			string[] buttons = { "A", "B", "Select", "Start", "Right", "Left", "Up", "Down", "R", "L" };
			/*
			 * 00 04 Reset (old timing)
			 * 00 08 Reset (new timing since version 1.1)
			 * 00 10 Left motion sensor
			 * 00 20 Right motion sensor
			 * 00 40 Down motion sensor
			 * 00 80 Up motion sensor
			*/
			string[] other =
			{
				"Reset (old timing)", "Reset (new timing since version 1.1)", "Left motion sensor",
				"Right motion sensor", "Down motion sensor", "Up motion sensor"
			};
			for (int frame = 1; frame <= frameCount; frame++)
			{
				/*
				 A stream of 2-byte bitvectors which indicate which buttons are pressed at each point in time. They will
				 come in groups of however many controllers are active, in increasing order.
				*/
				ushort controllerState = r.ReadUInt16();
				for (int button = 0; button < buttons.Length; button++)
				{
					controllers[buttons[button]] = ((controllerState >> button) & 0x1) != 0;
					if (((controllerState >> button) & 0x1) != 0 && button > 7)
					{
						continue;
					}
				}

				// TODO: Handle the other buttons.
				if (warningMsg == "")
				{
					for (int button = 0; button < other.Length; button++)
					{
						if (((controllerState >> (button + 10)) & 0x1) != 0)
						{
							warningMsg = "Unable to import " + other[button] + " at frame " + frame + ".";
							break;
						}
					}
				}

				// TODO: Handle the additional controllers.
				for (int player = 2; player <= controllersUsed.Length; player++)
				{
					if (controllersUsed[player - 1])
					{
						r.ReadBytes(2);
					}
				}

				m.AppendFrame(controllers);
			}

			r.Close();
			fs.Close();
			return m;
		}

		// VMV file format: http://tasvideos.org/VMV.html
		private static BkmMovie ImportVmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			BkmMovie m = new BkmMovie(path);
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader r = new BinaryReader(fs);

			// 000 12-byte signature: "VirtuaNES MV"
			string signature = r.ReadStringFixedAscii(12);
			if (signature != "VirtuaNES MV")
			{
				errorMsg = "This is not a valid .VMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			m.Header[HeaderKeys.PLATFORM] = "NES";

			// 00C 2-byte little-endian integer: movie version 0x0400
			ushort version = r.ReadUInt16();
			m.Comments.Add(MOVIEORIGIN + " .VMV version " + version);
			m.Comments.Add(EMULATIONORIGIN + " VirtuaNES");

			// 00E 2-byte little-endian integer: record version
			ushort recordVersion = r.ReadUInt16();
			m.Comments.Add(COMMENT + " Record version " + recordVersion);

			// 010 4-byte flags (control byte)
			uint flags = r.ReadUInt32();
			/*
			 * bit 0: controller 1 in use
			 * bit 1: controller 2 in use
			 * bit 2: controller 3 in use
			 * bit 3: controller 4 in use
			*/
			bool[] controllersUsed = new bool[4];
			for (int controller = 1; controller <= controllersUsed.Length; controller++)
			{
				controllersUsed[controller - 1] = ((flags >> (controller - 1)) & 0x1) != 0;
			}

			bool fourscore = controllersUsed[2] || controllersUsed[3];
			m.Header[HeaderKeys.FOURSCORE] = fourscore.ToString();
			/*
			 bit 6: 1=reset-based, 0=savestate-based (movie version <= 0x300 is always savestate-based)
			 If the movie version is < 0x400, or the "from-reset" flag is not set, a savestate is loaded from the movie.
			 Otherwise, the savestate is ignored.
			*/
			if (version < 0x400 || ((flags >> 6) & 0x1) == 0)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			/*
			 bit 7: disable rerecording
			 For the other control bytes, if a key from 1P to 4P (whichever one) is entirely ON, the following 4 bytes
			 becomes the controller data. TODO: Figure out what this means.
			 Other bits: reserved, set to 0
			*/

			// 014 DWORD   Ext0;				   // ROM:program CRC  FDS:program ID
			r.ReadBytes(4);

			// 018 WORD	Ext1;				   // ROM:unused,0	 FDS:maker ID
			r.ReadBytes(2);

			// 01A WORD	Ext2;				   // ROM:unused,0	 FDS:disk no.
			r.ReadBytes(2);

			// 01C 4-byte little-endian integer: rerecord count
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;
			/*
			020 BYTE	RenderMethod
			0=POST_ALL,1=PRE_ALL
			2=POST_RENDER,3=PRE_RENDER
			4=TILE_RENDER
			*/
			r.ReadByte();

			// 021 BYTE	IRQtype				 // IRQ type
			r.ReadByte();

			// 022 BYTE	FrameIRQ				// FrameIRQ not allowed
			r.ReadByte();

			// 023 1-byte flag: 0=NTSC (60 Hz), 1="PAL" (50 Hz)
			bool pal = r.ReadByte() == 1;
			m.Header[HeaderKeys.PAL] = pal.ToString();

			// 024 8-bytes: reserved, set to 0
			r.ReadBytes(8);

			// 02C 4-byte little-endian integer: save state start offset
			r.ReadBytes(4);

			// 030 4-byte little-endian integer: save state end offset
			r.ReadBytes(4);

			// 034 4-byte little-endian integer: movie data offset
			uint firstFrameOffset = r.ReadUInt32();

			// 038 4-byte little-endian integer: movie frame count
			uint frameCount = r.ReadUInt32();

			// 03C 4-byte little-endian integer: CRC (CRC excluding this data(to prevent cheating))
			int crc32 = r.ReadInt32();
			m.Header[CRC32] = crc32.ToString();
			if (!controllersUsed[0] && !controllersUsed[1] && !controllersUsed[2] && !controllersUsed[3])
			{
				warningMsg = "No input recorded.";
				r.Close();
				fs.Close();
				return m;
			}

			r.BaseStream.Position = firstFrameOffset;
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition { Name = "NES Controller" } };
			/*
			 * 01 A
			 * 02 B
			 * 04 Select
			 * 08 Start
			 * 10 Up
			 * 20 Down
			 * 40 Left
			 * 80 Right
			*/
			string[] buttons = { "A", "B", "Select", "Start", "Up", "Down", "Left", "Right" };
			for (int frame = 1; frame <= frameCount; frame++)
			{
				/*
				 Each frame consists of 1 or more bytes. Controller 1 takes 1 byte, controller 2 takes 1 byte, controller
				 3 takes 1 byte, and controller 4 takes 1 byte. If all four exist, the frame is 4 bytes. For example, if
				 the movie only has controller 1 data, a frame is 1 byte.
				*/
				controllers["Reset"] = false;
				for (int player = 1; player <= controllersUsed.Length; player++)
				{
					if (!controllersUsed[player - 1])
					{
						continue;
					}

					byte controllerState = r.ReadByte();
					if (controllerState >= 0xF0)
					{
						if (controllerState == 0xF0)
						{
							ushort command = r.ReadUInt16();
							string commandName = "";
							if ((command & 0xFF00) == 0)
							{
								switch (command & 0x00FF)
								{
									case 0: // NESCMD_NONE
										break;
									case 1: // NESCMD_HWRESET
										controllers["Reset"] = true;
										break;
									case 2: // NESCMD_SWRESET
										controllers["Reset"] = true;
										break;
									case 3: // NESCMD_EXCONTROLLER
										commandName = "NESCMD_EXCONTROLLER, 0";
										break;
									case 4: // NESCMD_DISK_THROTTLE_ON
										commandName = "NESCMD_DISK_THROTTLE_ON, 0";
										break;
									case 5: // NESCMD_DISK_THROTTLE_OFF
										commandName = "NESCMD_DISK_THROTTLE_OFF, 0";
										break;
									case 6: // NESCMD_DISK_EJECT
										commandName = "NESCMD_DISK_EJECT, 0";
										break;
									case 7: // NESCMD_DISK_0A
										commandName = "NESCMD_DISK_0A, 0";
										break;
									case 8: // NESCMD_DISK_0B
										commandName = "NESCMD_DISK_0B, 0";
										break;
									case 9: // NESCMD_DISK_1A
										commandName = "NESCMD_DISK_1A, 0";
										break;
									case 10: // NESCMD_DISK_1B
										commandName = "NESCMD_DISK_1B, 0";
										break;
									default:
										commandName = "invalid command";
										break;
								}
							}
							else
							{
								commandName = "NESCMD_EXCONTROLLER, " + (command & 0xFF00);
							}

							if (commandName != "" && warningMsg == "")
							{
								warningMsg = "Unable to run command \"" + commandName + "\".";
							}
						}
						else if (controllerState == 0xF3)
						{
							uint dwdata = r.ReadUInt32();

							// TODO: Make a clearer warning message.
							if (warningMsg == "")
							{
								warningMsg = "Unable to run SetSyncExData(" + dwdata + ").";
							}
						}

						controllerState = r.ReadByte();
					}

					for (int button = 0; button < buttons.Length; button++)
					{
						controllers["P" + player + " " + buttons[button]] = ((controllerState >> button) & 0x1) != 0;
					}
				}

				m.AppendFrame(controllers);
			}

			r.Close();
			fs.Close();
			return m;
		}

		// YMV file format: https://code.google.com/p/yabause-rr/wiki/YMVfileformat
		private static BkmMovie ImportYmv(string path, out string errorMsg, out string warningMsg)
		{
			return ImportText(path, out errorMsg, out warningMsg);
		}

		// ZMV file format: http://tasvideos.org/ZMV.html
		private static BkmMovie ImportZmv(string path, out string errorMsg, out string warningMsg)
		{
			errorMsg = warningMsg = "";
			BkmMovie m = new BkmMovie(path);
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			BinaryReader r = new BinaryReader(fs);

			// 000 3-byte signature: 5A 4D 56 "ZMV"
			string signature = r.ReadStringFixedAscii(3);
			if (signature != "ZMV")
			{
				errorMsg = "This is not a valid .ZMV file.";
				r.Close();
				fs.Close();
				return null;
			}

			m.Header[HeaderKeys.PLATFORM] = "SNES";

			// 003 2-byte little-endian unsigned int: zsnes version number
			short version = r.ReadInt16();
			m.Comments.Add(EMULATIONORIGIN + " ZSNES version " + version);
			m.Comments.Add(MOVIEORIGIN + " .ZMV");

			// 005 4-byte little-endian integer: CRC32 of the ROM
			int crc32 = r.ReadInt32();
			m.Header[CRC32] = crc32.ToString();

			// 009 4-byte little-endian unsigned int: number of frames
			uint frameCount = r.ReadUInt32();

			// 00D 4-byte little-endian unsigned int: number of rerecords
			uint rerecordCount = r.ReadUInt32();
			m.Rerecords = rerecordCount;

			// 011 4-byte little-endian unsigned int: number of frames removed by rerecord
			r.ReadBytes(4);

			// 015 4-byte little-endian unsigned int: number of frames advanced step by step
			r.ReadBytes(4);

			// 019 1-byte: average recording frames per second
			r.ReadByte();

			// 01A 4-byte little-endian unsigned int: number of key combos
			r.ReadBytes(4);

			// 01E 2-byte little-endian unsigned int: number of internal chapters
			ushort internalChaptersCount = r.ReadUInt16();

			// 020 2-byte little-endian unsigned int: length of the author name field in bytes
			ushort authorSize = r.ReadUInt16();

			// 022 3-byte little-endian unsigned int: size of an uncompressed save state in bytes
			r.ReadBytes(3);

			// 025 1-byte: reserved
			r.ReadByte();
			/*
			 026 1-byte flags: initial input configuration
				 bit 7: first input enabled
				 bit 6: second input enabled
				 bit 5: third input enabled
				 bit 4: fourth input enabled
				 bit 3: fifth input enabled
				 bit 2: mouse in first port
				 bit 1: mouse in second port
				 bit 0: super scope in second port
			*/
			byte controllerFlags = r.ReadByte();
			string peripheral = "";
			bool superScope = (controllerFlags & 0x1) != 0;
			if (superScope)
			{
				peripheral = "Super Scope";
			}

			controllerFlags >>= 1;
			bool secondMouse = (controllerFlags & 0x1) != 0;
			if (secondMouse && peripheral == "")
			{
				peripheral = "Second Mouse";
			}

			controllerFlags >>= 1;
			bool firstMouse = (controllerFlags & 0x1) != 0;
			if (firstMouse && peripheral == "")
			{
				peripheral = "First Mouse";
			}

			if (peripheral != "")
			{
				warningMsg = "Unable to import " + peripheral + ".";
			}

			// 027 1-byte flags:
			byte movieFlags = r.ReadByte();
			byte begins = (byte)(movieFlags & 0xC0);
			/*
			 bits 7,6:
				 if "00", movie begins from savestate
			*/
			if (begins == 0x00)
			{
				errorMsg = "Movies that begin with a savestate are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			// if "10", movie begins from reset
			// if "01", movie begins from power-on
			if (begins == 0x40)
			{
				errorMsg = "Movies that begin with SRAM are not supported.";
				r.Close();
				fs.Close();
				return null;
			}

			// if "11", movie begins from power-on with SRAM clear
			// bit 5: if "0", movie is NTSC (60 fps); if "1", movie is PAL (50 fps)
			bool pal = ((movieFlags >> 5) & 0x1) != 0;
			m.Header[HeaderKeys.PAL] = pal.ToString();

			// other: reserved, set to 0
			/*
			 028 3-byte little-endian unsigned int: initial save state size, highest bit specifies compression, next 23
			 specifies size
			*/
			uint savestateSize = (uint)((r.ReadByte() | (r.ReadByte() << 8) | (r.ReadByte() << 16)) & 0x7FFFFF);

			// Next follows a ZST format savestate.
			r.ReadBytes((int)savestateSize);
			SimpleController controllers = new SimpleController { Definition = new ControllerDefinition { Name = "SNES Controller" } };
			/*
			 * bit 11: A
			 * bit 10: X
			 * bit 9:  L
			 * bit 8:  R
			 * bit 7:  B
			 * bit 6:  Y
			 * bit 5:  Select
			 * bit 4:  Start
			 * bit 3:  Up
			 * bit 2:  Down
			 * bit 1:  Left
			 * bit 0:  Right
			*/
			string[] buttons =
			{
				"Right", "Left", "Down", "Up", "Start", "Select", "Y", "B", "R", "L", "X", "A"
			};
			int frames = 1;
			int internalChapters = 1;
			while (frames <= frameCount || internalChapters <= internalChaptersCount)
			{
				/*
				 000 1-byte flags:
					 bit 7: "1" if controller 1 changed, "0" otherwise
					 bit 6: "1" if controller 2 changed, "0" otherwise
					 bit 5: "1" if controller 3 changed, "0" otherwise
					 bit 4: "1" if controller 4 changed, "0" otherwise
					 bit 3: "1" if controller 5 changed, "0" otherwise
					 bit 2: "1" if this is a "chapter" update, "0" if it's a "controller" update
					 bit 1: "1" if this is RLE data instead of any of the above
					 bit 0: "1" if this is command data instead any of the above
				*/
				byte flag = r.ReadByte();
				if ((flag & 0x1) != 0)
				{
					/*
					 If the event is a command, the other seven bits define the command. The command could be "reset now"
					 or similar things.
					*/
					flag >>= 1;
					if (flag == 0x0)
					{
						controllers["Reset"] = true;
						m.AppendFrame(controllers);
						controllers["Reset"] = false;
					}

					/*TODO: Other commands.*/
				}
				else if (((flag >> 1) & 0x1) != 0)
				{
					// If the event is RLE data, next follows 4 bytes which is the frame to repeat current input till.
					uint frame = r.ReadUInt32();
					if (frame > frameCount)
					{
						throw new ArgumentException("RLE data repeats for frames beyond the total frame count.");
					}

					for (; frames <= frame; frames++)
					{
						m.AppendFrame(controllers);
					}
				}
				else if (((flag >> 2) & 0x1) != 0)
				{
					/*
					 If the event is a "chapter" update, the packet follows with a ZST format savestate. Using a header of:
						000 3-byte little endian unsigned int: save state size in format defined above
					*/
					savestateSize = (uint)((r.ReadByte() | (r.ReadByte() << 8) | (r.ReadByte() << 16)) & 0x7FFFFF);

					// 001 above size: save state
					r.ReadBytes((int)savestateSize);

					// above size+001 4-byte little endian unsigned int: frame number save state loads to
					r.ReadBytes(4);

					// above size+005 2-byte: controller status bit field, see below
					r.ReadBytes(2);

					// above size+007 9-byte: previous controller input bits
					r.ReadBytes(9);
					internalChapters++;
				}
				else
				{
					flag >>= 3;
					/*
					 If the event is a "controller" update, next comes the controller data for each changed controller, 12
					 bits per controller, or 20 bits in the case of the super scope, zeropadding up to full bytes. The
					 minimum length of the controller data is 2 bytes, and the maximum length is 9 bytes.
					*/
					bool leftOver = false;
					byte leftOverValue = 0x0;
					for (int player = 1; player <= 5; player++)
					{
						// If the controller has changed:
						if (((flag >> (5 - player)) & 0x1) != 0)
						{
							/*
							 For example, if this frame contained input for controllers 1 and 2:
								 byte 1:
									 bit 7:  P1 B
									 bit 6:  P1 Y
									 bit 5:  P1 Select
									 bit 4:  P1 Start
									 bit 3:  P1 Up
									 bit 2:  P1 Down
									 bit 1:  P1 Left
									 bit 0:  P1 Right
								 byte 2:
									 bit 7:  P2 B
									 bit 6:  P2 Y
									 bit 5:  P2 Select
									 bit 4:  P2 Start
									 bit 3:  P1 A
									 bit 2:  P1 X
									 bit 1:  P1 L
									 bit 0:  P1 R
								 byte 3:
									 bit 7:  P2 Up
									 bit 6:  P2 Down
									 bit 5:  P2 Left
									 bit 4:  P2 Right
									 bit 3:  P2 A
									 bit 2:  P2 X
									 bit 1:  P2 L
									 bit 0:  P2 R
							*/
							byte controllerState1 = r.ReadByte();
							uint controllerState;
							if (!leftOver)
							{
								byte controllerState2 = r.ReadByte();
								if (player == 2 && superScope)
								{
									byte controllerState3 = r.ReadByte();
									controllerState = (uint)((controllerState1 | (controllerState2 << 8) |
										(controllerState3 << 12)) & 0x0FFFFF);
									leftOverValue = (byte)((controllerState3 >> 4) & 0x0F);
								}
								else
								{
									controllerState = (uint)((controllerState1 | (controllerState2 << 4)) & 0x0FFF);
									leftOverValue = (byte)((controllerState2 >> 4) & 0x0F);
								}
							}
							else
							{
								controllerState = (uint)((controllerState1 >> 4) | (leftOverValue << 4) |
									((controllerState1 << 8) & 0x0F00));
								if (player == 2 && superScope)
								{
									byte controllerState2 = r.ReadByte();
									controllerState |= (uint)(controllerState2 << 12);
								}
							}

							leftOver = !leftOver;
							if (player <= BkmMnemonicConstants.Players[controllers.Definition.Name])
							{
								if (player != 2 || !superScope)
								{
									for (int button = 0; button < buttons.Length; button++)
									{
										controllers["P" + player + " " + buttons[button]] = 
											((controllerState >> button) & 0x1) != 0;
									}
								}
							}
							else if (warningMsg == "")
							{
								warningMsg = "Controller " + player + " not supported.";
							}
						}
					}

					m.AppendFrame(controllers);
					frames++;
				}
			}

			r.BaseStream.Position = r.BaseStream.Length - authorSize;

			// Last in the file comes the author name field, which is an UTF-8 encoded text string.
			var author = Encoding.UTF8.GetString(r.ReadBytes(authorSize));
			m.Header[HeaderKeys.AUTHOR] = author;
			return m;
		}
	}
}
