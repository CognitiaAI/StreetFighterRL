﻿using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Atari.Atari2600
{
	public partial class Atari2600 : IEmulator
	{
		public IEmulatorServiceProvider ServiceProvider { get; }

		public ControllerDefinition ControllerDefinition => _controllerDeck.Definition;

		public void FrameAdvance(IController controller, bool render, bool rendersound)
		{
			_controller = controller;

			_frame++;
			_islag = true;

			// Handle all the console controls here
			if (_controller.IsPressed("Power"))
			{
				HardReset();
			}

			if (_controller.IsPressed("Toggle Left Difficulty") && !_leftDifficultySwitchHeld)
			{
				_leftDifficultySwitchPressed ^= true;
				_leftDifficultySwitchHeld = true;
			}
			else if (!_controller.IsPressed("Toggle Left Difficulty"))
			{
				_leftDifficultySwitchHeld = false;
			}

			if (_controller.IsPressed("Toggle Right Difficulty") && !_rightDifficultySwitchHeld)
			{
				_rightDifficultySwitchPressed ^= true;
				_rightDifficultySwitchHeld = true;
			}
			else if (!_controller.IsPressed("Toggle Right Difficulty"))
			{
				_rightDifficultySwitchHeld = false;
			}

			while (!_tia.New_Frame)
			{
				Cycle();
			}

			_tia.New_Frame = false;

			if (rendersound == false)
			{
				_tia.AudioClocks = 0; // we need this here since the async sound provider won't check in this case
			}

			if (_islag)
			{
				_lagcount++;
			}

			_tia.LineCount = 0;
		}

		public int Frame => _frame;

		public string SystemId => "A26";

		public bool DeterministicEmulation => true;

		public CoreComm CoreComm { get; }

		public void ResetCounters()
		{
			_frame = 0;
			_lagcount = 0;
			_islag = false;
		}

		public void Dispose()
		{
		}
	}
}
