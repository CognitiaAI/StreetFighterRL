﻿using BizHawk.Common;
using BizHawk.Common.NumberExtensions;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	public sealed class Taito_X1_017 : NES.NESBoardBase
	{
		/*
		ines Mapper 82
		http://wiki.nesdev.com/w/index.php/INES_Mapper_082

		 *  Example Games:
		 --------------------------
		 SD Keiji - Blader
		
 
 
		 Notes:
		 ---------------------------
		 Regs appear at $7EFx, I'm unsure whether or not PRG-RAM can exist at $6000-7FFF
 
 
		 Registers:
		 ---------------------------
 
		   $7EF0-7EF5:  CHR Regs
 
		   $7EF6:  [.... ..CM]  CHR Mode/Mirroring
			 C = CHR Mode select
			 M = Mirroring:
				0 = Horz
				1 = Vert
 
		   $7EFA:  [PPPP PP..]  PRG Reg 0 (8k @ $8000)
		   $7EFB:  [PPPP PP..]  PRG Reg 1 (8k @ $A000)
		   $7EFC:  [PPPP PP..]  PRG Reg 2 (8k @ $C000)
 
 
		 CHR Setup:
		 ---------------------------
 
						 $0000   $0400   $0800   $0C00   $1000   $1400   $1800   $1C00 
					   +---------------+---------------+-------+-------+-------+-------+
		 CHR Mode 0:   |    <$7EF0>    |    <$7EF1>    | $7EF2 | $7EF3 | $7EF4 | $7EF5 |
					   +---------------+---------------+---------------+---------------+
		 CHR Mode 1:   | $7EF2 | $7EF3 | $7EF4 | $7EF5 |    <$7EF0>    |    <$7EF1>    |
					   +-------+-------+-------+-------+---------------+---------------+
 
 
		 PRG Setup:
		 ---------------------------
 
			   $8000   $A000   $C000   $E000  
			 +-------+-------+-------+-------+
			 | $7EFA | $7EFB | $7EFC | { -1} |
			 +-------+-------+-------+-------+
 
		 Note:  remember that the low 2 bits are not used (right-shift written values by 2)
		*/

		// config
		int prg_bank_mask, chr_bank_mask;
		// state
		ByteBuffer prg_regs_8k = new ByteBuffer(4);
		ByteBuffer chr_regs_1k = new ByteBuffer(8);
		bool ChrMode;
		bool[] wramenable = new bool[3];

		public override void Dispose()
		{
			base.Dispose();
			chr_regs_1k.Dispose();
			prg_regs_8k.Dispose();
		}

		public override void SyncState(Serializer ser)
		{
			base.SyncState(ser);
			ser.Sync("prg_regs_8k", ref prg_regs_8k);
			ser.Sync("chr_regs_1k", ref chr_regs_1k);
			ser.Sync("ChrMode", ref ChrMode);
			for (int i = 0; i < wramenable.Length; i++)
				ser.Sync("wramenable_" + i, ref wramenable[i]);
		}

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			//configure
			switch (Cart.board_type)
			{
				case "MAPPER082":
					break;
				case "TAITO-X1-017":
					break;
				default:
					return false;
			}

			// actually internal to the mapper
			Cart.wram_size = 5;

			SetMirrorType(EMirrorType.Vertical);
			chr_bank_mask = Cart.chr_size / 1 - 1;
			prg_bank_mask = Cart.prg_size / 8 - 1;
			prg_regs_8k[3] = 0xFF;
			return true;
		}

		public override byte ReadWRAM(int addr)
		{
			if (addr < 0x1400 && wramenable[addr >> 11])
				return WRAM[addr];
			else
				return NES.DB;
		}

		public override void WriteWRAM(int addr, byte value)
		{
			if (addr < 0x1400)
			{
				if (wramenable[addr >> 11])
					WRAM[addr] = value;
				return;
			}

			switch (addr)
			{
				case 0x1EF0:
					chr_regs_1k[0] = (byte)(value & ~1);
					chr_regs_1k[1] = (byte)(value | 1);
					break;
				case 0x1EF1:
					chr_regs_1k[2] = (byte)(value & ~1);
					chr_regs_1k[3] = (byte)(value | 1);
					break;

				case 0x1EF2:
					chr_regs_1k[4] = value;
					break;
				case 0x1EF3:
					chr_regs_1k[5] = value;
					break;
				case 0x1EF4:
					chr_regs_1k[6] = value;
					break;
				case 0X1EF5:
					chr_regs_1k[7] = value;
					break;

				case 0x1EF6:
					ChrMode = value.Bit(1);
					if (value.Bit(0))
						SetMirrorType(EMirrorType.Vertical);
					else
						SetMirrorType(EMirrorType.Horizontal);
					break;

				case 0x1EF7: wramenable[0] = value == 0xca; break;
				case 0x1EF8: wramenable[1] = value == 0x69; break;
				case 0x1EF9: wramenable[2] = value == 0x84; break;

				case 0x1EFA:
					prg_regs_8k[0] = (byte)(value >> 2);
					break;
				case 0x1EFB:
					prg_regs_8k[1] = (byte)(value >> 2);
					break;
				case 0x1EFC:
					prg_regs_8k[2] = (byte)(value >> 2);
					break;
			}
		}

		public override byte ReadPRG(int addr)
		{
			int bank_8k = addr >> 13;
			int ofs = addr & ((1 << 13) - 1);
			bank_8k = prg_regs_8k[bank_8k];
			bank_8k &= prg_bank_mask;
			addr = (bank_8k << 13) | ofs;
			return ROM[addr];
		}

		public override byte ReadPPU(int addr)
		{
			if (addr < 0x2000)
			{
				if (ChrMode)
					addr ^= 1 << 12;
				int bank_1k = addr >> 10;
				int ofs = addr & ((1 << 10) - 1);
				bank_1k = chr_regs_1k[bank_1k];
				bank_1k &= chr_bank_mask;
				addr = (bank_1k << 10) | ofs;
				return VROM[addr];
			}
			else
			{
				return base.ReadPPU(addr);
			}
		}
	}
}
