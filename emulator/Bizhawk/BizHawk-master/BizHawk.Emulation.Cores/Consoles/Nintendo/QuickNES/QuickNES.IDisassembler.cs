﻿using System;
using System.Collections.Generic;
using System.Linq;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Components.M6502;

namespace BizHawk.Emulation.Cores.Consoles.Nintendo.QuickNES
{
	public partial class QuickNES : IDisassemblable
	{
		public string Cpu
		{
			get
			{
				return "6502";
			}
			set
			{
			}
		}

		public string PCRegisterName
		{
			get { return "PC"; }
		}

		public IEnumerable<string> AvailableCpus
		{
			get { yield return "6502"; }
		}

		public string Disassemble(MemoryDomain m, uint addr, out int length)
		{
			return MOS6502X.Disassemble((ushort)addr, out length, (a) => m.PeekByte(a));
		}
	}
}
