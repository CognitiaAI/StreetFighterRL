﻿using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.PCEngine
{
	public sealed partial class PCEngine : IEmulator
	{
		public IEmulatorServiceProvider ServiceProvider { get; private set; }

		public ControllerDefinition ControllerDefinition => _controllerDeck.Definition;

		public void FrameAdvance(IController controller, bool render, bool rendersound)
		{
			_controller = controller;
			_lagged = true;
			DriveLightOn = false;
			Frame++;
			CheckSpriteLimit();
			PSG.BeginFrame(Cpu.TotalExecutedCycles);

			Cpu.Debug = Tracer.Enabled;

			if (SuperGrafx)
			{
				VPC.ExecFrame(render);
			}
			else
			{
				VDC1.ExecFrame(render);
			}

			PSG.EndFrame(Cpu.TotalExecutedCycles);

			if (_lagged)
			{
				_lagCount++;
				_isLag = true;
			}
			else
			{
				_isLag = false;
			}
		}

		public int Frame
		{
			get { return _frame; }
			set { _frame = value; }
		}

		public string SystemId { get; }

		public bool DeterministicEmulation => true;

		public void ResetCounters()
		{
			// this should just be a public setter instead of a new method.
			Frame = 0;
			_lagCount = 0;
			_isLag = false;
		}

		public CoreComm CoreComm { get; }

		public void Dispose()
		{
			disc?.Dispose();
		}
	}
}
