﻿using System;
using BizHawk.Common;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	// a number of boards used by Sachen (and others?)
	// 32K prgrom blocks and 8k chrrom blocks
	// behavior gleamed from FCEUX
	// "Qi Wang - Chinese Chess (MGC-001) (Ch) [!]" and "Twin Eagle (Sachen) [!]" seem to have problems; the latter needs PAL
	public sealed class SachenSimple : NES.NESBoardBase
	{
		Action<byte> ExpWrite = null;
		Action<byte> PrgWrite = null;

		int prg = 0;
		int chr = 0;
		int prg_mask;
		int chr_mask;
		int prg_addr_mask; // some carts have 16KB prg unswappable

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			switch (Cart.board_type)
			{
				case "MAPPER146":
				case "UNL-SA-016-1M":
				case "UNIF_UNL-SA-016-1M":
					ExpWrite = SA0161M_Write;
					break;
				case "MAPPER145":
				case "UNIF_UNL-SA-72007":
					ExpWrite = SA72007_Write;
					break;
				case "MAPPER133":
				case "UNIF_UNL-SA-72008":
					ExpWrite = SA72008_Write;
					break;
				case "MAPPER160":
					ExpWrite = SA009_Write;
					break;
				case "MAPPER149":
				case "UNIF_UNL-SA-0036":
					PrgWrite = SA72007_Write;
					break;
				case "MAPPER148":
				case "UNIF_UNL-SA-0037":
					PrgWrite = SA0161M_Write;
					break;
				default:
					return false;
			}

			// adelikat: Jaau Kong 2-in-1 is 128 prg and 128 chr.  Don't know if that's a valid configuration, but the game works
			AssertPrg(16, 32, 64, 128);
			AssertChr(8, 16, 32, 64, 128);
			AssertVram(0);
			Cart.wram_size = 0;
			prg_mask = Cart.prg_size / 32 - 1;
			chr_mask = Cart.chr_size / 8 - 1;
			prg_addr_mask = Cart.prg_size * 1024 - 1;
			SetMirrorType(Cart.pad_h, Cart.pad_v);
			return true;
		}

		void SA0161M_Write(byte value)
		{
			prg = (value >> 3) & 1 & prg_mask;
			chr = value & 7 & chr_mask;
		}
		void SA72007_Write(byte value)
		{
			chr = (value >> 7) & 1 & chr_mask;
		}
		void SA009_Write(byte value)
		{
			chr = value & 1 & chr_mask;
		}
		void SA72008_Write(byte value)
		{
			prg = (value >> 2) & 1 & prg_mask;
			chr = value & 3 & chr_mask;
		}

		public override void WriteEXP(int addr, byte value)
		{
			if (ExpWrite != null && (addr & 0x100) != 0)
				ExpWrite(value);
		}

		public override void WritePRG(int addr, byte value)
		{
			if (PrgWrite != null)
				PrgWrite(value);
		}

		public override byte ReadPRG(int addr)
		{
			return ROM[(addr & prg_addr_mask) + (prg << 15)];
		}
		public override byte ReadPPU(int addr)
		{
			if (addr < 0x2000)
				return VROM[addr + (chr << 13)];
			else
				return base.ReadPPU(addr);
		}

		public override void SyncState(Serializer ser)
		{
			base.SyncState(ser);
			ser.Sync("chr", ref chr);
			ser.Sync("prg", ref prg);
		}
	}
}
