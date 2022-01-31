﻿using System.ComponentModel;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Computers.AppleII
{
	public partial class AppleII : ISettable<AppleII.Settings, object>
	{
		private Settings _settings;

		public class Settings
		{
			[DefaultValue(false)]
			[Description("Choose a monochrome monitor.")]
			public bool Monochrome { get; set; }

			public Settings Clone()
			{
				return (Settings)MemberwiseClone();
			}
		}

		public Settings GetSettings()
		{
			return _settings.Clone();
		}

		public object GetSyncSettings()
		{
			return null;
		}

		public bool PutSettings(Settings o)
		{
			_settings = o;
			_machine.Video.IsMonochrome = _settings.Monochrome;

			SetCallbacks();

			return false;
		}

		public bool PutSyncSettings(object o)
		{
			return false;
		}
	}
}
