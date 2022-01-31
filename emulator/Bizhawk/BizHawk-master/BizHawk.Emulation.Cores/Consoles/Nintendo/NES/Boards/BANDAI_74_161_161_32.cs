﻿using BizHawk.Common;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	public sealed class BANDAI_74_161_161_32 : NES.NESBoardBase
	{
		//Mapper 70
		//Example Games:
		//--------------------------
		//Family Trainer - Manhattan Police
		//Family Trainer - Meiro Daisakusen
		//Kamen Rider Club
		//Space Shadow

		int chr;
		int prg_bank_mask_16k;
		byte prg_bank_16k;
		ByteBuffer prg_banks_16k = new ByteBuffer(2);

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			switch (Cart.board_type)
			{
				case "MAPPER070":
					break;
				case "BANDAI-74*161/161/32":
					break;
				default:
					return false;
			}
			SetMirrorType(Cart.pad_h, Cart.pad_v);
			prg_bank_mask_16k = (Cart.prg_size / 16) - 1;
			prg_banks_16k[1] = 0xFF;
			return true;
		}

		public override void Dispose()
		{
			prg_banks_16k.Dispose();
			base.Dispose();
		}

		public override void SyncState(Serializer ser)
		{
			base.SyncState(ser);
			ser.Sync("chr", ref chr);
			ser.Sync("prg_bank_mask_16k", ref prg_bank_mask_16k);
			ser.Sync("prg_bank_16k", ref prg_bank_16k);
			ser.Sync("prg_banks_16k", ref prg_banks_16k);
		}

		void SyncPRG()
		{
			prg_banks_16k[0] = prg_bank_16k;
		}

		public override void WritePRG(int addr, byte value)
		{
			prg_bank_16k = (byte)((value >> 4) & 15);
			chr = value & 15;
			SyncPRG();
		}

		public override byte ReadPRG(int addr)
		{
			int bank_16k = addr >> 14;
			int ofs = addr & ((1 << 14) - 1);
			bank_16k = prg_banks_16k[bank_16k];
			bank_16k &= prg_bank_mask_16k;
			addr = (bank_16k << 14) | ofs;
			return ROM[addr];
		}

		public override byte ReadPPU(int addr)
		{
			if (addr < 0x2000)
				return VROM[(addr & 0x1FFF) + (chr * 0x2000)];
			else
				return base.ReadPPU(addr);
		}
	}
}
