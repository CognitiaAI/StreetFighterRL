﻿using System;
using System.ComponentModel;

using Newtonsoft.Json;

using BizHawk.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Nintendo.Gameboy
{
	public partial class Gameboy : ISettable<Gameboy.GambatteSettings, Gameboy.GambatteSyncSettings>
	{
		public GambatteSettings GetSettings()
		{
			return _settings.Clone();
		}

		public bool PutSettings(GambatteSettings o)
		{
			_settings = o;
			if (IsCGBMode())
			{
				SetCGBColors(_settings.CGBColors);
			}
			else
			{
				ChangeDMGColors(_settings.GBPalette);
			}

			return false;
		}

		public GambatteSyncSettings GetSyncSettings()
		{
			return _syncSettings.Clone();
		}

		public bool PutSyncSettings(GambatteSyncSettings o)
		{
			bool ret = GambatteSyncSettings.NeedsReboot(_syncSettings, o);
			_syncSettings = o;
			return ret;
		}

		private GambatteSettings _settings;
		private GambatteSyncSettings _syncSettings;

		public class GambatteSettings
		{
			private static readonly int[] DefaultPalette =
			{
				10798341, 8956165, 1922333, 337157,
				10798341, 8956165, 1922333, 337157,
				10798341, 8956165, 1922333, 337157
			};

			public int[] GBPalette;
			public GBColors.ColorType CGBColors;
			public bool DisplayBG = true, DisplayOBJ = true, DisplayWindow = true;

			/// <summary>
			/// true to mute all audio
			/// </summary>
			public bool Muted;

			public GambatteSettings()
			{
				GBPalette = (int[])DefaultPalette.Clone();
				CGBColors = GBColors.ColorType.gambatte;
			}


			public GambatteSettings Clone()
			{
				var ret = (GambatteSettings)MemberwiseClone();
				ret.GBPalette = (int[])GBPalette.Clone();
				return ret;
			}
		}

		public class GambatteSyncSettings
		{
			[DisplayName("Enable BIOS: WARNING: File must exist!")]
			[Description("Boots game using system BIOS. Should be used for TASing")]
			[DefaultValue(false)]
			public bool EnableBIOS { get; set; }

			public enum ConsoleModeType
			{
				Auto,
				GB,
				GBC
			}

			[DisplayName("Console Mode")]
			[Description("Pick which console to run, 'Auto' chooses from ROM header, 'GB' and 'GBC' chooses the respective system")]
			[DefaultValue(ConsoleModeType.Auto)]
			public ConsoleModeType ConsoleMode { get; set; }

			[DisplayName("CGB in GBA")]
			[Description("Emulate GBA hardware running a CGB game, instead of CGB hardware.  Relevant only for titles that detect the presense of a GBA, such as Shantae.")]
			[DefaultValue(false)]
			public bool GBACGB { get; set; }

			[DisplayName("Multicart Compatibility")]
			[Description("Use special compatibility hacks for certain multicart games.  Relevant only for specific multicarts.")]
			[DefaultValue(false)]
			public bool MulticartCompat { get; set; }

			[DisplayName("Realtime RTC")]
			[Description("If true, the real time clock in MBC3 games will reflect real time, instead of emulated time.  Ignored (treated as false) when a movie is recording.")]
			[DefaultValue(false)]
			public bool RealTimeRTC { get; set; }

			[DisplayName("RTC Initial Time")]
			[Description("Set the initial RTC time in terms of elapsed seconds.  Only used when RealTimeRTC is false.")]
			[DefaultValue(0)]
			public int RTCInitialTime
			{
				get { return _RTCInitialTime; }
				set { _RTCInitialTime = Math.Max(0, Math.Min(1024 * 24 * 60 * 60, value)); }
			}

			[JsonIgnore]
			private int _RTCInitialTime;

			[DisplayName("Equal Length Frames")]
			[Description("When false, emulation frames sync to vblank.  Only useful for high level TASing.")]
			[DefaultValue(true)]
			public bool EqualLengthFrames
			{
				get { return _equalLengthFrames; }
				set { _equalLengthFrames = value; }
			}

			[JsonIgnore]
			[DeepEqualsIgnore]
			private bool _equalLengthFrames;

			public GambatteSyncSettings()
			{
				SettingsUtil.SetDefaultValues(this);
			}

			public GambatteSyncSettings Clone()
			{
				return (GambatteSyncSettings)MemberwiseClone();
			}

			public static bool NeedsReboot(GambatteSyncSettings x, GambatteSyncSettings y)
			{
				return !DeepEquality.DeepEquals(x, y);
			}
		}
	}
}
