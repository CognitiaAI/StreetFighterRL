﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BizHawk.Client.EmuHawk
{
	class ArgParser
		//parses command line arguments and adds the values to a class attribute
		//default values are null for strings and false for boolean
		//the last value will overwrite previously set values
		//unrecognized parameters are simply ignored or in the worst case assumed to be a ROM name [cmdRom]
	{
		public string cmdRom = null;
		public string cmdLoadSlot = null;
		public string cmdLoadState = null;
		public string cmdMovie = null;
		public string cmdDumpType = null;
		public string cmdDumpName = null;
		public HashSet<int> _currAviWriterFrameList;
		public int _autoDumpLength;
		public bool _autoCloseOnDump = false;
		// chrome is never shown, even in windowed mode
		public bool _chromeless = false;
		public bool startFullscreen = false;
		public string luaScript = null;
		public bool luaConsole = false;
		public int socket_port = 9999;
		public string socket_ip = null;
		public string run_id = null;
		public bool use_two_controllers = false;
		public int socket_port_p2 = 10000;
		public string socket_ip_p2 = "127.0.0.1";
		public int round_over_delay = 0;
		public int emulator_speed_percent = 6399;
		public bool pause_after_round = false;

		public void parseArguments(string[] args)
			
		{
			for (int i = 0; i<args.Length; i++)
			{
				// For some reason sometimes visual studio will pass this to us on the commandline. it makes no sense.
				if (args[i] == ">")
				{
					i++;
					var stdout = args[i];
					Console.SetOut(new StreamWriter(stdout));
					continue;
				}

				var arg = args[i].ToLower();
				if (arg.StartsWith("--load-slot="))
				{
					cmdLoadSlot = arg.Substring(arg.IndexOf('=') + 1);
				}

				if (arg.StartsWith("--run_id="))
				{
					run_id = arg.Substring(arg.IndexOf('=') + 1);
				}

				if (arg.StartsWith("--load-state="))
				{
					cmdLoadState = arg.Substring(arg.IndexOf('=') + 1);
				}
				else if (arg.StartsWith("--movie="))
				{
					cmdMovie = arg.Substring(arg.IndexOf('=') + 1);
				}
				else if (arg.StartsWith("--dump-type="))
				{
					cmdDumpType = arg.Substring(arg.IndexOf('=') + 1);
				}
				else if (arg.StartsWith("--dump-frames="))
				{
					var list = arg.Substring(arg.IndexOf('=') + 1);
					var items = list.Split(',');
					_currAviWriterFrameList = new HashSet<int>();
					foreach (string item in items)
					{
						_currAviWriterFrameList.Add(int.Parse(item));
					}

					// automatically set dump length to maximum frame
					_autoDumpLength = _currAviWriterFrameList.OrderBy(x => x).Last();
				}
				else if (arg.StartsWith("--dump-name="))
				{
					cmdDumpName = arg.Substring(arg.IndexOf('=') + 1);
				}
				else if (arg.StartsWith("--dump-length="))
				{
					int.TryParse(arg.Substring(arg.IndexOf('=') + 1), out _autoDumpLength);
				}
				else if (arg.StartsWith("--dump-close"))
				{
					_autoCloseOnDump = true;
				}
				else if (arg.StartsWith("--chromeless"))
				{
					_chromeless = true;
				}
				else if (arg.StartsWith("--fullscreen"))
				{
					startFullscreen = true;
				}
				else if (arg.StartsWith("--lua="))
				{
					luaScript = arg.Substring(arg.IndexOf('=') + 1);
					luaConsole = true;
				}
				else if (arg.StartsWith("--luaconsole"))
				{
					luaConsole = true;
				}
				else if (arg.StartsWith("--socket_port="))
				{
					int.TryParse(arg.Substring(arg.IndexOf('=') + 1), out socket_port);
				}
				else if (arg.StartsWith("--socket_ip="))
				{
					socket_ip = arg.Substring(arg.IndexOf('=') + 1);
				}
				else if (arg.StartsWith("--socket_port_p2="))
				{
					int.TryParse(arg.Substring(arg.IndexOf('=') + 1), out socket_port_p2);
				}
				else if (arg.StartsWith("--socket_ip_p2="))
				{
					socket_ip_p2 = arg.Substring(arg.IndexOf('=') + 1);
				}
				else if (arg.StartsWith("--use_two_controllers"))
				{
					use_two_controllers = true;
				}
				else if (arg.StartsWith("--pause_after_round"))
				{
					pause_after_round = true;
				}
				else if (arg.StartsWith("--round_over_delay="))
				{
					round_over_delay =Convert.ToInt32(arg.Substring(arg.IndexOf('=') + 1));
				}
				else if (arg.StartsWith("--emulator_speed_percent="))
				{
					emulator_speed_percent = Convert.ToInt32(arg.Substring(arg.IndexOf('=') + 1));
				}
				else
				{
					cmdRom = args[i];
				}
			}
		}
	}
}
