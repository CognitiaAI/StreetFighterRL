using System;
using Newtonsoft.Json;
using BizHawk.Common;

namespace BizHawk.Emulation.Cores.Libretro
{

	partial class LibretroCore
	{
		Settings _Settings = new Settings();
		SyncSettings _SyncSettings;

		public class SyncSettings
		{
			public SyncSettings Clone()
			{
				return JsonConvert.DeserializeObject<SyncSettings>(JsonConvert.SerializeObject(this));
			}

			public SyncSettings()
			{
			}
		}


		public class Settings
		{
			public void Validate()
			{
			}

			public Settings()
			{
				SettingsUtil.SetDefaultValues(this);
			}

			public Settings Clone()
			{
				return (Settings)MemberwiseClone();
			}
		}

		public Settings GetSettings()
		{
			return _Settings.Clone();
		}

		public SyncSettings GetSyncSettings()
		{
			return _SyncSettings.Clone();
		}

		public bool PutSettings(Settings o)
		{
			_Settings.Validate();
			_Settings = o;

			//TODO - store settings into core? or we can just keep doing it before frameadvance

			return false;
		}

		public bool PutSyncSettings(SyncSettings o)
		{
			bool reboot = false;

			//we could do it this way roughly if we need to
			//if(JsonConvert.SerializeObject(o.FIOConfig) != JsonConvert.SerializeObject(_SyncSettings.FIOConfig)

			_SyncSettings = o;

			return reboot;
		}
	}

}