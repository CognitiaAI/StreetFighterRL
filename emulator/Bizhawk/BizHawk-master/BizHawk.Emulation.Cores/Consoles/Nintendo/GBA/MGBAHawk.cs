﻿using System;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Nintendo.GBA
{
	[Core("mGBA", "endrift", true, true, "0.6.0 (24ab81f8db7745586cef246be8957ad272aded9b)", "https://mgba.io/", false)]
	[ServiceNotApplicable(typeof(IDriveLight), typeof(IRegionable))]
	public partial class MGBAHawk : IEmulator, IVideoProvider, ISoundProvider, IGBAGPUViewable,
		ISaveRam, IStatable, IInputPollable, ISettable<MGBAHawk.Settings, MGBAHawk.SyncSettings>,
		IDebuggable
	{
		[CoreConstructor("GBA")]
		public MGBAHawk(byte[] file, CoreComm comm, SyncSettings syncSettings, Settings settings, bool deterministic, GameInfo game)
		{
			_syncSettings = syncSettings ?? new SyncSettings();
			_settings = settings ?? new Settings();
			DeterministicEmulation = deterministic;

			byte[] bios = comm.CoreFileProvider.GetFirmware("GBA", "Bios", false);
			DeterministicEmulation &= bios != null;

			if (DeterministicEmulation != deterministic)
			{
				throw new InvalidOperationException("A BIOS is required for deterministic recordings!");
			}

			if (!DeterministicEmulation && bios != null && !_syncSettings.RTCUseRealTime && !_syncSettings.SkipBios)
			{
				// in these situations, this core is deterministic even though it wasn't asked to be
				DeterministicEmulation = true;
			}

			if (bios != null && bios.Length != 16384)
			{
				throw new InvalidOperationException("BIOS must be exactly 16384 bytes!");
			}

			var skipBios = !DeterministicEmulation && _syncSettings.SkipBios;

			_core = LibmGBA.BizCreate(bios, file, file.Length, GetOverrideInfo(game), skipBios);
			if (_core == IntPtr.Zero)
			{
				throw new InvalidOperationException("BizCreate() returned NULL!  Bad BIOS? and/or ROM?");
			}

			try
			{
				CreateMemoryDomains(file.Length);
				var ser = new BasicServiceProvider(this);
				ser.Register<IDisassemblable>(new ArmV4Disassembler());
				ser.Register<IMemoryDomains>(_memoryDomains);

				ServiceProvider = ser;
				CoreComm = comm;
				CoreComm.NominalWidth = 240;
				CoreComm.NominalHeight = 160;
				PutSettings(_settings);
			}
			catch
			{
				LibmGBA.BizDestroy(_core);
				throw;
			}
		}

		public IEmulatorServiceProvider ServiceProvider { get; }

		public ControllerDefinition ControllerDefinition => GBA.GBAController;

		public void FrameAdvance(IController controller, bool render, bool rendersound = true)
		{
			Frame++;
			if (controller.IsPressed("Power"))
			{
				LibmGBA.BizReset(_core);

				// BizReset caused memorydomain pointers to change.
				WireMemoryDomainPointers();
			}

			IsLagFrame = LibmGBA.BizAdvance(
				_core,
				VBANext.GetButtons(controller),
				_videobuff,
				ref _nsamp,
				_soundbuff,
				RTCTime(),
				(short)controller.GetFloat("Tilt X"),
				(short)controller.GetFloat("Tilt Y"),
				(short)controller.GetFloat("Tilt Z"),
				(byte)(255 - controller.GetFloat("Light Sensor")));

			if (IsLagFrame)
			{
				LagCount++;
			}

			// this should be called in hblank on the appropriate line, but until we implement that, just do it here
			_scanlinecb?.Invoke();
		}

		public int Frame { get; private set; }

		public string SystemId => "GBA";

		public bool DeterministicEmulation { get; }

		public void ResetCounters()
		{
			Frame = 0;
			LagCount = 0;
			IsLagFrame = false;
		}

		public CoreComm CoreComm { get; }

		public void Dispose()
		{
			if (_core != IntPtr.Zero)
			{
				LibmGBA.BizDestroy(_core);
				_core = IntPtr.Zero;
			}
		}

		public GBAGPUMemoryAreas GetMemoryAreas()
		{
			return _gpumem;
		}

		[FeatureNotImplemented]
		public void SetScanlineCallback(Action callback, int scanline)
		{
			_scanlinecb = callback;
		}

		private readonly byte[] _saveScratch = new byte[262144];
		private IntPtr _core;

		private static LibmGBA.OverrideInfo GetOverrideInfo(GameInfo game)
		{
			if (!game.OptionPresent("mgbaNeedsOverrides"))
			{
				// the gba game db predates the mgba core in bizhawk, but was never used by the mgba core,
				// which had its own handling for overrides
				// to avoid possible regressions, we don't want to be overriding things that we already
				// know work in mgba, so unless this parameter is set, we do nothing
				return null;
			}

			var ret = new LibmGBA.OverrideInfo();
			if (game.OptionPresent("flashSize"))
			{
				switch (game.GetIntValue("flashSize"))
				{
					case 65536:
						ret.Savetype = LibmGBA.SaveType.Flash512;
						break;
					case 131072:
						ret.Savetype = LibmGBA.SaveType.Flash1m;
						break;
					default:
						throw new InvalidOperationException("Unknown flashSize");
				}
			}
			else if (game.OptionPresent("saveType"))
			{
				switch (game.GetIntValue("saveType"))
				{
					// 3 specifies either flash 512 or 1024, but in vba-over.ini, the latter will have a flashSize as well
					case 3:
						ret.Savetype = LibmGBA.SaveType.Flash512;
						break;
					case 4:
						ret.Savetype = LibmGBA.SaveType.Eeprom;
						break;
					default:
						throw new InvalidOperationException("Unknown saveType");
				}
			}

			if (game.GetInt("rtcEnabled", 0) == 1)
			{
				ret.Hardware |= LibmGBA.Hardware.Rtc;
			}

			if (game.GetInt("mirroringEnabled", 0) == 1)
			{
				throw new InvalidOperationException("Don't know what to do with mirroringEnabled!");
			}

			if (game.OptionPresent("idleLoop"))
			{
				ret.IdleLoop = (uint)game.GetHexValue("idleLoop");
			}

			return ret;
		}

		private Action _scanlinecb;

		private GBAGPUMemoryAreas _gpumem;

		private long RTCTime()
		{
			if (!DeterministicEmulation && _syncSettings.RTCUseRealTime)
			{
				return (long)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			}

			long basetime = (long)_syncSettings.RTCInitialTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			long increment = Frame * 4389L >> 18;
			return basetime + increment;
		}
	}
}
