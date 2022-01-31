using System;
using System.Linq;
using System.Xml;
using System.IO;

using BizHawk.Common.BufferExtensions;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Components.W65816;

// TODO - add serializer (?)

// http://wiki.superfamicom.org/snes/show/Backgrounds

// TODO 
// libsnes needs to be modified to support multiple instances - THIS IS NECESSARY - or else loading one game and then another breaks things
// edit - this is a lot of work
// wrap dll code around some kind of library-accessing interface so that it doesnt malfunction if the dll is unavailablecd
namespace BizHawk.Emulation.Cores.Nintendo.SNES
{
	[Core(
		"BSNES",
		"byuu",
		isPorted: true,
		isReleased: true,
		portedVersion: "v87",
		portedUrl: "http://byuu.org/")]
	[ServiceNotApplicable(typeof(IDriveLight))]
	public unsafe partial class LibsnesCore : IEmulator, IVideoProvider, ISaveRam, IStatable, IInputPollable, IRegionable, ICodeDataLogger,
		IDebuggable, ISettable<LibsnesCore.SnesSettings, LibsnesCore.SnesSyncSettings>
	{
		public LibsnesCore(GameInfo game, byte[] romData, byte[] xmlData, CoreComm comm, object settings, object syncSettings)
		{
			var ser = new BasicServiceProvider(this);
			ServiceProvider = ser;

			_tracer = new TraceBuffer
			{
				Header = "65816: PC, mnemonic, operands, registers (A, X, Y, S, D, DB, flags (NVMXDIZC), V, H)"
			};

			ser.Register<IDisassemblable>(new W65816_DisassemblerService());

			_game = game;
			CoreComm = comm;
			byte[] sgbRomData = null;

			if (game["SGB"])
			{
				if ((romData[0x143] & 0xc0) == 0xc0)
				{
					throw new CGBNotSupportedException();
				}

				sgbRomData = CoreComm.CoreFileProvider.GetFirmware("SNES", "Rom_SGB", true, "SGB Rom is required for SGB emulation.");
				game.FirmwareHash = sgbRomData.HashSHA1();
			}

			_settings = (SnesSettings)settings ?? new SnesSettings();
			_syncSettings = (SnesSyncSettings)syncSettings ?? new SnesSyncSettings();

			// TODO: pass profile here
			Api = new LibsnesApi(CoreComm.CoreFileProvider.DllPath())
			{
				ReadHook = ReadHook,
				ExecHook = ExecHook,
				WriteHook = WriteHook
			};

			ScanlineHookManager = new MyScanlineHookManager(this);

			_controllerDeck = new LibsnesControllerDeck(_syncSettings);
			_controllerDeck.NativeInit(Api);
			
			Api.CMD_init();

			Api.QUERY_set_path_request(snes_path_request);

			_scanlineStartCb = new LibsnesApi.snes_scanlineStart_t(snes_scanlineStart);
			_tracecb = new LibsnesApi.snes_trace_t(snes_trace);

			_soundcb = new LibsnesApi.snes_audio_sample_t(snes_audio_sample);

			// start up audio resampler
			InitAudio();
			ser.Register<ISoundProvider>(_resampler);

			// strip header
			if ((romData?.Length & 0x7FFF) == 512)
			{
				var newData = new byte[romData.Length - 512];
				Array.Copy(romData, 512, newData, 0, newData.Length);
				romData = newData;
			}

			if (game["SGB"])
			{
				IsSGB = true;
				SystemId = "SNES";
				ser.Register<IBoardInfo>(new SGBBoardInfo());

				_currLoadParams = new LoadParams()
				{
					type = LoadParamType.SuperGameBoy,
					rom_xml = null,
					rom_data = sgbRomData,
					rom_size = (uint)sgbRomData.Length,
					dmg_data = romData,
				};

				if (!LoadCurrent())
				{
					throw new Exception("snes_load_cartridge_normal() failed");
				}
			}
			else
			{
				// we may need to get some information out of the cart, even during the following bootup/load process
				if (xmlData != null)
				{
					_romxml = new XmlDocument();
					_romxml.Load(new MemoryStream(xmlData));

					// bsnes wont inspect the xml to load the necessary sfc file.
					// so, we have to do that here and pass it in as the romData :/
					if (_romxml["cartridge"]?["rom"] != null)
					{
						romData = File.ReadAllBytes(CoreComm.CoreFileProvider.PathSubfile(_romxml["cartridge"]["rom"].Attributes["name"].Value));
					}
					else
					{
						throw new Exception("Could not find rom file specification in xml file. Please check the integrity of your xml file");
					}
				}

				SystemId = "SNES";
				_currLoadParams = new LoadParams
				{
					type = LoadParamType.Normal,
					xml_data = xmlData,
					rom_data = romData
				};

				if (!LoadCurrent())
				{
					throw new Exception("snes_load_cartridge_normal() failed");
				}
			}

			if (Api.Region == LibsnesApi.SNES_REGION.NTSC)
			{
				// similar to what aviout reports from snes9x and seems logical from bsnes first principles. bsnes uses that numerator (ntsc master clockrate) for sure.
				VsyncNumerator = 21477272;
				VsyncDenominator = 4 * 341 * 262;
			}
			else
			{
				// http://forums.nesdev.com/viewtopic.php?t=5367&start=19
				VsyncNumerator = 21281370;
				VsyncDenominator = 4 * 341 * 312;
			}

			Api.CMD_power();

			SetupMemoryDomains(romData, sgbRomData);

			if (CurrentProfile == "Compatibility")
			{
				ser.Register<ITraceable>(_tracer);
			}

			Api.QUERY_set_path_request(null);
			Api.QUERY_set_video_refresh(snes_video_refresh);
			Api.QUERY_set_input_poll(snes_input_poll);
			Api.QUERY_set_input_state(snes_input_state);
			Api.QUERY_set_input_notify(snes_input_notify);
			Api.QUERY_set_audio_sample(_soundcb);
			Api.Seal();
			RefreshPalette();
		}

		private readonly GameInfo _game;
		private readonly LibsnesControllerDeck _controllerDeck;
		private readonly ITraceable _tracer;
		private readonly XmlDocument _romxml;
		private readonly LibsnesApi.snes_scanlineStart_t _scanlineStartCb;
		private readonly LibsnesApi.snes_trace_t _tracecb;
		private readonly LibsnesApi.snes_audio_sample_t _soundcb;

		private IController _controller;
		private LoadParams _currLoadParams;
		private SpeexResampler _resampler;
		private int _timeFrameCounter;
		private bool _disposed;

		public bool IsSGB { get; }

		private class SGBBoardInfo : IBoardInfo
		{
			public string BoardName => "SGB";
		}

		public string CurrentProfile => "Compatibility"; // We no longer support performance, and accuracy isn't worth the effort so we shall just hardcode this one

		public LibsnesApi Api { get; }

		public SnesColors.ColorType CurrPalette { get; private set; }

		public MyScanlineHookManager ScanlineHookManager { get; }

		public class MyScanlineHookManager : ScanlineHookManager
		{
			private readonly LibsnesCore _core;

			public MyScanlineHookManager(LibsnesCore core)
			{
				_core = core;
			}

			protected override void OnHooksChanged()
			{
				_core.OnScanlineHooksChanged();
			}
		}

		private void OnScanlineHooksChanged()
		{
			if (_disposed)
			{
				return;
			}

			Api.QUERY_set_scanlineStart(ScanlineHookManager.HookCount == 0 ? null : _scanlineStartCb);
		}

		private void snes_scanlineStart(int line)
		{
			ScanlineHookManager.HandleScanline(line);
		}

		private string snes_path_request(int slot, string hint)
		{
			// every rom requests msu1.rom... why? who knows.
			// also handle msu-1 pcm files here
			bool isMsu1Rom = hint == "msu1.rom";
			bool isMsu1Pcm = Path.GetExtension(hint).ToLower() == ".pcm";
			if (isMsu1Rom || isMsu1Pcm)
			{
				// well, check if we have an msu-1 xml
				if (_romxml?["cartridge"]?["msu1"] != null)
				{
					var msu1 = _romxml["cartridge"]["msu1"];
					if (isMsu1Rom && msu1["rom"]?.Attributes["name"] != null)
					{
						return CoreComm.CoreFileProvider.PathSubfile(msu1["rom"].Attributes["name"].Value);
					}

					if (isMsu1Pcm)
					{
						// return @"D:\roms\snes\SuperRoadBlaster\SuperRoadBlaster-1.pcm";
						// return "";
						int wantsTrackNumber = int.Parse(hint.Replace("track-", "").Replace(".pcm", ""));
						wantsTrackNumber++;
						string wantsTrackString = wantsTrackNumber.ToString();
						foreach (var child in msu1.ChildNodes.Cast<XmlNode>())
						{
							if (child.Name == "track" && child.Attributes["number"].Value == wantsTrackString)
							{
								return CoreComm.CoreFileProvider.PathSubfile(child.Attributes["name"].Value);
							}
						}
					}
				}

				// not found.. what to do? (every rom will get here when msu1.rom is requested)
				return "";
			}

			// not MSU-1.  ok.
			string firmwareId;

			switch (hint)
			{
				case "cx4.rom": firmwareId = "CX4"; break;
				case "dsp1.rom": firmwareId = "DSP1"; break;
				case "dsp1b.rom": firmwareId = "DSP1b"; break;
				case "dsp2.rom": firmwareId = "DSP2"; break;
				case "dsp3.rom": firmwareId = "DSP3"; break;
				case "dsp4.rom": firmwareId = "DSP4"; break;
				case "st010.rom": firmwareId = "ST010"; break;
				case "st011.rom": firmwareId = "ST011"; break;
				case "st018.rom": firmwareId = "ST018"; break;
				default:
					CoreComm.ShowMessage($"Unrecognized SNES firmware request \"{hint}\".");
					return "";
			}

			string ret;
			var data = CoreComm.CoreFileProvider.GetFirmware("SNES", firmwareId, false, "Game may function incorrectly without the requested firmware.");
			if (data != null)
			{
				ret = hint;
				Api.AddReadonlyFile(data, hint);
			}
			else
			{
				ret = "";
			}

			Console.WriteLine("Served libsnes request for firmware \"{0}\"", hint);

			// return the path we built
			return ret;
		}

		private void snes_trace(uint which, string msg)
		{
			// TODO: get them out of the core split up and remove this hackery
			const string splitStr = "A:";

			if (which == (uint)LibsnesApi.eTRACE.CPU)
			{
				var split = msg.Split(new[] { splitStr }, 2, StringSplitOptions.None);

				_tracer.Put(new TraceInfo
				{
					Disassembly = split[0].PadRight(34),
					RegisterInfo = splitStr + split[1]
				});
			}
			else if (which == (uint)LibsnesApi.eTRACE.SMP)
			{
				int idx = msg.IndexOf("YA:");
				string dis = msg.Substring(0,idx).TrimEnd();
				string regs = msg.Substring(idx);
				_tracer.Put(new TraceInfo
				{
					Disassembly = dis,
					RegisterInfo = regs
				});
			}
			else if (which == (uint)LibsnesApi.eTRACE.GB)
			{
				int idx = msg.IndexOf("AF:");
				string dis = msg.Substring(0,idx).TrimEnd();
				string regs = msg.Substring(idx);
				_tracer.Put(new TraceInfo
				{
					Disassembly = dis,
					RegisterInfo = regs
				});
			}
		}

		private void SetPalette(SnesColors.ColorType pal)
		{
			CurrPalette = pal;
			int[] tmp = SnesColors.GetLUT(pal);
			fixed (int* p = &tmp[0])
				Api.QUERY_set_color_lut((IntPtr)p);
		}

		private void ReadHook(uint addr)
		{
			MemoryCallbacks.CallReads(addr);
			// we RefreshMemoryCallbacks() after the trigger in case the trigger turns itself off at that point
			// EDIT: for now, theres some IPC re-entrancy problem
			// RefreshMemoryCallbacks();
		}

		private void ExecHook(uint addr)
		{
			MemoryCallbacks.CallExecutes(addr);
			// we RefreshMemoryCallbacks() after the trigger in case the trigger turns itself off at that point
			// EDIT: for now, theres some IPC re-entrancy problem
			// RefreshMemoryCallbacks();
		}

		private void WriteHook(uint addr, byte val)
		{
			MemoryCallbacks.CallWrites(addr);
			// we RefreshMemoryCallbacks() after the trigger in case the trigger turns itself off at that point
			// EDIT: for now, theres some IPC re-entrancy problem
			// RefreshMemoryCallbacks();
		}

		private enum LoadParamType
		{
			Normal, SuperGameBoy
		}

		private struct LoadParams
		{
			public LoadParamType type;
			public byte[] xml_data;

			public string rom_xml;
			public byte[] rom_data;
			public uint rom_size;
			public byte[] dmg_data;
		}

		private bool LoadCurrent()
		{
			bool result = _currLoadParams.type == LoadParamType.Normal
				? Api.CMD_load_cartridge_normal(_currLoadParams.xml_data, _currLoadParams.rom_data)
				: Api.CMD_load_cartridge_super_game_boy(_currLoadParams.rom_xml, _currLoadParams.rom_data, _currLoadParams.rom_size, _currLoadParams.dmg_data);

			_mapper = Api.Mapper;

			return result;
		}

		/// <summary>
		/// </summary>
		/// <param name="port">0 or 1, corresponding to L and R physical ports on the snes</param>
		/// <param name="device">LibsnesApi.SNES_DEVICE enum index specifying type of device</param>
		/// <param name="index">meaningless for most controllers.  for multitap, 0-3 for which multitap controller</param>
		/// <param name="id">button ID enum; in the case of a regular controller, this corresponds to shift register position</param>
		/// <returns>for regular controllers, one bit D0 of button status.  for other controls, varying ranges depending on id</returns>
		private short snes_input_state(int port, int device, int index, int id)
		{
			return _controllerDeck.CoreInputState(_controller, port, device, index, id);
		}

		private void snes_input_poll()
		{
			// this doesn't actually correspond to anything in the underlying bsnes;
			// it gets called once per frame with video_refresh() and has nothing to do with anything
		}

		private void snes_input_notify(int index)
		{
			// gets called with the following numbers:
			// 4xxx : lag frame related
			// 0: signifies latch bit going to 0.  should be reported as oninputpoll
			// 1: signifies latch bit going to 1.  should be reported as oninputpoll
			if (index >= 0x4000)
			{
				IsLagFrame = false;
			}
		}

		private void snes_video_refresh(int* data, int width, int height)
		{
			bool doubleSize = _settings.AlwaysDoubleSize;
			bool lineDouble = doubleSize, dotDouble = doubleSize;

			_videoWidth = width;
			_videoHeight = height;

			int yskip = 1, xskip = 1;

			// if we are in high-res mode, we get double width. so, lets double the height here to keep it square.
			if (width == 512)
			{
				_videoHeight *= 2;
				yskip = 2;

				lineDouble = true;

				// we dont dot double here because the user wanted double res and the game provided double res
				dotDouble = false;
			}
			else if (lineDouble)
			{
				_videoHeight *= 2;
				yskip = 2;
			}

			int srcPitch = 1024;
			int srcStart = 0;

			bool interlaced = height == 478 || height == 448;
			if (interlaced)
			{
				// from bsnes in interlaced mode we have each field side by side
				// so we will come in with a dimension of 512x448, say
				// but the fields are side by side, so it's actually 1024x224.
				// copy the first scanline from row 0, then the 2nd scanline from row 0 (offset 512)
				// EXAMPLE: yu yu hakushu legal screens
				// EXAMPLE: World Class Service Super Nintendo Tester (double resolution vertically but not horizontally, in character test the stars should shrink)
				lineDouble = false;
				srcPitch = 512;
				yskip = 1;
				_videoHeight = height;
			}

			if (dotDouble)
			{
				_videoWidth *= 2;
				xskip = 2;
			}

			if (_settings.CropSGBFrame && IsSGB)
			{
				_videoWidth = 160;
				_videoHeight = 144;
			}

			int size = _videoWidth * _videoHeight;
			if (_videoBuffer.Length != size)
			{
				_videoBuffer = new int[size];
			}

			if (_settings.CropSGBFrame && IsSGB)
			{
				int di = 0;
				for (int y = 0; y < 144; y++)
				{
					int si = ((y+39) * srcPitch) + 48;
					for(int x=0;x<160;x++)
						_videoBuffer[di++] = data[si++];
				}
				return;
			}

			for (int j = 0; j < 2; j++)
			{
				if (j == 1 && !dotDouble)
				{
					break;
				}

				int xbonus = j;
				for (int i = 0; i < 2; i++)
				{
					// potentially do this twice, if we need to line double
					if (i == 1 && !lineDouble)
					{
						break;
					}

					int bonus = (i * _videoWidth) + xbonus;
					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x++)
						{
							int si = (y * srcPitch) + x + srcStart;
							int di = y * _videoWidth * yskip + x * xskip + bonus;
							int rgb = data[si];
							_videoBuffer[di] = rgb;
						}
					}
				}
			}

			VirtualHeight = BufferHeight;
			VirtualWidth = BufferWidth;
			if (VirtualHeight * 2 < VirtualWidth)
				VirtualHeight *= 2;
			if (VirtualHeight > 240)
				VirtualWidth = 512;
			VirtualWidth = (int)Math.Round(VirtualWidth * 1.146);
		}

		private void RefreshMemoryCallbacks(bool suppress)
		{
			var mcs = MemoryCallbacks;
			Api.QUERY_set_state_hook_exec(!suppress && mcs.HasExecutes);
			Api.QUERY_set_state_hook_read(!suppress && mcs.HasReads);
			Api.QUERY_set_state_hook_write(!suppress && mcs.HasWrites);
		}

		//public byte[] snes_get_memory_data_read(LibsnesApi.SNES_MEMORY id)
		//{
		//  var size = (int)api.snes_get_memory_size(id);
		//  if (size == 0) return new byte[0];
		//  var ret = api.snes_get_memory_data(id);
		//  return ret;
		//}

		private void InitAudio()
		{
			_resampler = new SpeexResampler((SpeexResampler.Quality)6, 64081, 88200, 32041, 44100);
		}

		private void snes_audio_sample(ushort left, ushort right)
		{
			_resampler.EnqueueSample((short)left, (short)right);
		}

		private void RefreshPalette()
		{
			SetPalette((SnesColors.ColorType)Enum.Parse(typeof(SnesColors.ColorType), _settings.Palette, false));
		}
	}
}
