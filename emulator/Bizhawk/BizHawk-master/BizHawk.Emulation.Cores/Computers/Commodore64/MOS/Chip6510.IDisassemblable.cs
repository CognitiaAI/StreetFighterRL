﻿using System.Collections.Generic;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Components.M6502;

namespace BizHawk.Emulation.Cores.Computers.Commodore64.MOS
{
	public sealed partial class Chip6510 : IDisassemblable
	{
		public IEnumerable<string> AvailableCpus
		{
			get { yield return "6510"; }
		}

		public string Cpu
		{
			get
			{
				return "6510";
			}

			set
			{
			}
		}

		public string PCRegisterName => "PC";

		public string Disassemble(MemoryDomain m, uint addr, out int length)
		{
			return MOS6502X.Disassemble((ushort)addr, out length, CpuPeek);
		}
	}
}
