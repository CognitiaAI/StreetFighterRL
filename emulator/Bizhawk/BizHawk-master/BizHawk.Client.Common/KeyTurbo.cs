﻿namespace BizHawk.Client.Common
{
	public class TurboKey
	{
		private int _upTime, _downTime, _timer;

		public bool Value { get; set; }

		public void Reset(int downTime, int upTime)
		{
			Value = false;
			_timer = 0;
			_upTime = upTime;
			_downTime = downTime;
		}

		public void Tick(bool down)
		{
			if (!down)
			{
				Reset(_downTime, _upTime);
				return;
			}

			_timer++;

			Value = true;
			if (_timer > _downTime)
			{
				Value = false;
			}

			if (_timer > _upTime + _downTime)
			{
				_timer = 0;
				Value = true;
			}
		}
	}
}