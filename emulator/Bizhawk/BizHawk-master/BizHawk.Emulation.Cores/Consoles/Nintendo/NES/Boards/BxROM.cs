﻿using BizHawk.Common;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	//AKA half of mapper 034 (the other half is AVE_NINA_001 which is entirely different..)
	public sealed class BxROM : NES.NESBoardBase
	{
		//configuration
		int prg_bank_mask_32k;
		int chr_bank_mask_8k;

		//state
		int prg_bank_32k;
		int chr_bank_8k;

		public override void SyncState(Serializer ser)
		{
			base.SyncState(ser);
			ser.Sync("prg_bank_32k", ref prg_bank_32k);
			ser.Sync("chr_bank_8k", ref chr_bank_8k);
		}

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			switch (Cart.board_type)
			{
				case "AVE-NINA-07": // wally bear and the gang
					// it's not the NINA_001 but something entirely different; actually a colordreams with VRAM
					// this actually works
					AssertPrg(32,128); AssertChr(0,16); AssertWram(0); AssertVram(0,8);
					break;

				case "IREM-BNROM": //Mashou (J).nes
				case "NES-BNROM": //Deadly Towers (U)
					AssertPrg(128,256); AssertChr(0); AssertWram(0,8); AssertVram(8);
					break;

				default:
					return false;
			}

			prg_bank_mask_32k = Cart.prg_size / 32 - 1;
			chr_bank_mask_8k = Cart.chr_size / 8 - 1;

			SetMirrorType(Cart.pad_h, Cart.pad_v);

			return true;
		}

		public override byte ReadPRG(int addr)
		{
			addr |= (prg_bank_32k << 15);
			return ROM[addr];
		}

		public override void WritePRG(int addr, byte value)
		{
			value = HandleNormalPRGConflict(addr, value);
			prg_bank_32k = value & prg_bank_mask_32k;
			chr_bank_8k = ((value >> 4) & 0xF) & chr_bank_mask_8k;
		}

		public override byte ReadPPU(int addr)
		{
			if (addr<0x2000)
			{
				if (VRAM != null)
				{
					return VRAM[addr];
				}
				else
				{
					return VROM[addr | (chr_bank_8k << 13)];
				}
			}
			else
			{
				return base.ReadPPU(addr);
			}
		}

	}
}
