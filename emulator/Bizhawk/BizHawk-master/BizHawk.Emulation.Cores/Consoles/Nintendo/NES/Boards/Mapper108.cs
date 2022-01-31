﻿using BizHawk.Common;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	// Meikyuu Jiin Dababa (FDS Conversion)
	public sealed class Mapper108 : NES.NESBoardBase
	{
		private int prg;

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			switch (Cart.board_type)
			{
				case "MAPPER108":
					break;
				default:
					return false;
			}
			AssertPrg(128);
			AssertChr(0);
			Cart.vram_size = 8;
			AssertWram(0);
			SetMirrorType(Cart.pad_h, Cart.pad_v);

			return true;
		}

		public override void WritePRG(int addr, byte value)
		{
			if (addr < 0xFFF
				|| addr >= 0x7000) // hack ported from FCEUX to support Bubble Bobble (FDS Conversion, Kaiser Hacked) (Unl) [p1][!]
			{
				prg = value & 15;
			}
		}

		public override byte ReadPRG(int addr)
		{
			return ROM[addr | 0x18000];
		}

		public override byte ReadWRAM(int addr)
		{
			return ROM[addr | prg << 13];
		}

		public override void SyncState(Serializer ser)
		{
			base.SyncState(ser);
			ser.Sync("prg", ref prg);
		}
	}
}
