﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace BizHawk.Client.Common
{
	public partial class Bk2Movie
	{
		protected IStringLog Log { get; set; }
		protected string LogKey { get; set; } = "";

		public void WriteInputLog(TextWriter writer)
		{
			writer.WriteLine("[Input]");
			WriteRawInputLog(writer);
			writer.WriteLine("[/Input]");
		}

		public string GetInputLogEntry(int frame)
		{
			if (frame < FrameCount && frame >= 0)
			{
				int getframe;

				if (LoopOffset.HasValue)
				{
					if (frame < Log.Count)
					{
						getframe = frame;
					}
					else
					{
						getframe = ((frame - LoopOffset.Value) % (Log.Count - LoopOffset.Value)) + LoopOffset.Value;
					}
				}
				else
				{
					getframe = frame;
				}

				return Log[getframe];
			}

			return "";
		}

		public virtual bool ExtractInputLog(TextReader reader, out string errorMessage)
		{
			errorMessage = "";
			int? stateFrame = null;

			// We are in record mode so replace the movie log with the one from the savestate
			if (!Global.MovieSession.MultiTrack.IsActive)
			{
				if (Global.Config.EnableBackupMovies && MakeBackup && Log.Count != 0)
				{
					SaveBackup();
					MakeBackup = false;
				}

				Log.Clear();
				while (true)
				{
					var line = reader.ReadLine();
					if (string.IsNullOrEmpty(line))
					{
						break;
					}

					// in BK2, this is part of the input log, and not involved with the core state at all
					// accordingly, this case (for special neshawk format frame numbers) is irrelevant
					// probably
					else if (line.Contains("Frame 0x")) // NES stores frame count in hex, yay
					{
						var strs = line.Split('x');
						try
						{
							stateFrame = int.Parse(strs[1], NumberStyles.HexNumber);
						}
						catch
						{
							errorMessage = "Savestate Frame number failed to parse";
							return false;
						}
					}
					else if (line.Contains("Frame "))
					{
						var strs = line.Split(' ');
						try
						{
							stateFrame = int.Parse(strs[1]);
						}
						catch
						{
							errorMessage = "Savestate Frame number failed to parse";
							return false;
						}
					}
					else if (line.StartsWith("LogKey:"))
					{
						LogKey = line.Replace("LogKey:", "");
					}
					else if (line[0] == '|')
					{
						Log.Add(line);
					}
				}
			}
			else
			{
				var i = 0;
				while (true)
				{
					var line = reader.ReadLine();
					if (line == null)
					{
						break;
					}

					if (line.Contains("Frame 0x")) // NES stores frame count in hex, yay
					{
						var strs = line.Split('x');
						try
						{
							stateFrame = int.Parse(strs[1], NumberStyles.HexNumber);
						}
						catch
						{
							errorMessage = "Savestate Frame number failed to parse";
							return false;
						}
					}
					else if (line.Contains("Frame "))
					{
						var strs = line.Split(' ');
						try
						{
							stateFrame = int.Parse(strs[1]);
						}
						catch
						{
							errorMessage = "Savestate Frame number failed to parse";
							return false;
						}
					}
					else if (line.StartsWith("LogKey:"))
					{
						LogKey = line.Replace("LogKey:", "");
					}
					else if (line.StartsWith("|"))
					{
						SetFrameAt(i, line);
						i++;
					}
				}
			}

			if (!stateFrame.HasValue)
			{
				errorMessage = "Savestate Frame number failed to parse";
			}

			var stateFramei = stateFrame ?? 0;

			if (stateFramei > 0 && stateFramei < Log.Count)
			{
				if (!Global.Config.VBAStyleMovieLoadState)
				{
					Truncate(stateFramei);
				}
			}
			else if (stateFramei > Log.Count) // Post movie savestate
			{
				if (!Global.Config.VBAStyleMovieLoadState)
				{
					Truncate(Log.Count);
				}

				Mode = Moviemode.Finished;
			}

			if (IsCountingRerecords)
			{
				Rerecords++;
			}

			return true;
		}

		public bool CheckTimeLines(TextReader reader, out string errorMessage)
		{
			// This function will compare the movie data to the savestate movie data to see if they match
			errorMessage = "";
			var newLog = new List<string>();
			var stateFrame = 0;
			while (true)
			{
				var line = reader.ReadLine();
				if (line == null)
				{
					break;
				}

				if (line.Trim() == "")
				{
					continue;
				}

				if (line.Contains("Frame 0x")) // NES stores frame count in hex, yay
				{
					var strs = line.Split('x');
					try
					{
						stateFrame = int.Parse(strs[1], NumberStyles.HexNumber);
					}
					catch
					{
						errorMessage = "Savestate Frame number failed to parse";
						return false;
					}
				}
				else if (line.Contains("Frame "))
				{
					var strs = line.Split(' ');
					try
					{
						stateFrame = int.Parse(strs[1]);
					}
					catch
					{
						errorMessage = "Savestate Frame number failed to parse";
						return false;
					}
				}
				else if (line[0] == '|')
				{
					newLog.Add(line);
				}
			}

			if (stateFrame == 0)
			{
				stateFrame = newLog.Count;  // In case the frame count failed to parse, revert to using the entire state input log
			}

			if (Log.Count < stateFrame)
			{
				if (IsFinished)
				{
					return true;
				}

				errorMessage = "The savestate is from frame "
					+ newLog.Count
					+ " which is greater than the current movie length of "
					+ Log.Count;

				return false;
			}

			for (var i = 0; i < stateFrame; i++)
			{
				if (Log[i] != newLog[i])
				{
					errorMessage = "The savestate input does not match the movie input at frame "
						+ (i + 1)
						+ ".";

					return false;
				}
			}

			if (stateFrame > newLog.Count) // stateFrame is greater than state input log, so movie finished mode
			{
				if (Mode == Moviemode.Play || Mode == Moviemode.Finished)
				{
					Mode = Moviemode.Finished;
					return true;
				}

				return false;
			}

			if (Mode == Moviemode.Finished)
			{
				Mode = Moviemode.Play;
			}

			return true;
		}

		protected void WriteRawInputLog(TextWriter writer)
		{
			var lg = new Bk2LogEntryGenerator(LogKey);
			lg.SetSource(Global.MovieOutputHardpoint);

			writer.WriteLine(lg.GenerateLogKey());

			foreach (var record in Log)
			{
				writer.WriteLine(record);
			}
		}

		/// <summary>
		/// Takes a log entry from a line in an input log,
		/// If the log key differs from the system's, it will be converted
		/// </summary>
		/// <param name="line">a log entry line of text from the input log</param>
		/// <param name="logKey">a key used to describe the mnemonic values and positions in the log</param>
		private string ConvertLogEntryFromFile(string line, string logKey)
		{
			var adapter = new Bk2LogEntryGenerator(logKey).MovieControllerAdapter;
			adapter.Definition = Global.MovieSession.MovieControllerAdapter.Definition;
			adapter.SetControllersAsMnemonic(line);

			var lg = LogGeneratorInstance();
			lg.SetSource(adapter);
			return lg.GenerateLogEntry();
		}
	}
}
