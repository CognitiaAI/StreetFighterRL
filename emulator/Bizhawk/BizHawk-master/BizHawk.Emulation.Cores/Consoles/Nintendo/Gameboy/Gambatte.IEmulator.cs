﻿using System;
using System.Diagnostics;

using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Nintendo.Gameboy
{
	public partial class Gameboy : IEmulator, IBoardInfo
	{
		public IEmulatorServiceProvider ServiceProvider { get; }

		public ControllerDefinition ControllerDefinition => GbController;

		public void FrameAdvance(IController controller, bool render, bool rendersound)
		{
			FrameAdvancePrep(controller);
			if (_syncSettings.EqualLengthFrames)
			{
				while (true)
				{
					// target number of samples to emit: length of 1 frame minus whatever overflow
					uint samplesEmitted = TICKSINFRAME - frameOverflow;
					Debug.Assert(samplesEmitted * 2 <= _soundbuff.Length);
					if (LibGambatte.gambatte_runfor(GambatteState, _soundbuff, ref samplesEmitted) > 0)
					{
						LibGambatte.gambatte_blitto(GambatteState, VideoBuffer, 160);
					}

					// account for actual number of samples emitted
					_cycleCount += samplesEmitted;
					frameOverflow += samplesEmitted;

					if (rendersound && !Muted)
					{
						ProcessSound((int)samplesEmitted);
					}

					if (frameOverflow >= TICKSINFRAME)
					{
						frameOverflow -= TICKSINFRAME;
						break;
					}
				}
			}
			else
			{
				// target number of samples to emit: always 59.7fps
				// runfor() always ends after creating a video frame, so sync-up is guaranteed
				// when the display has been off, some frames can be markedly shorter than expected
				uint samplesEmitted = TICKSINFRAME;
				if (LibGambatte.gambatte_runfor(GambatteState, _soundbuff, ref samplesEmitted) > 0)
				{
					LibGambatte.gambatte_blitto(GambatteState, VideoBuffer, 160);
				}

				_cycleCount += samplesEmitted;
				frameOverflow = 0;
				if (rendersound && !Muted)
				{
					ProcessSound((int)samplesEmitted);
				}
			}

			if (rendersound && !Muted)
			{
				ProcessSoundEnd();
			}

			FrameAdvancePost();
		}

		public int Frame { get; private set; }

		public string SystemId => "GB";

		public string BoardName { get; }

		public bool DeterministicEmulation { get; }

		public void ResetCounters()
		{
			Frame = 0;
			LagCount = 0;
			IsLagFrame = false;

			// reset frame counters is meant to "re-zero" emulation time wherever it was
			// so these should be reset as well
			_cycleCount = 0;
			frameOverflow = 0;
		}

		public CoreComm CoreComm { get; }

		public void Dispose()
		{
			if (GambatteState != IntPtr.Zero)
			{
				LibGambatte.gambatte_destroy(GambatteState);
				GambatteState = IntPtr.Zero;
			}

			DisposeSound();
		}
	}
}
