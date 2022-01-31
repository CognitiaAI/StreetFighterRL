﻿using BizHawk.Common;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	public sealed class Mapper241 : NES.NESBoardBase
	{
		//163 is for nanjing games

		//configuration
		int prg_bank_mask_32k;

		//state
		ByteBuffer prg_banks_32k = new ByteBuffer(1);

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			//configure
			switch (Cart.board_type)
			{
				case "MAPPER241":
					break;
				default:
					return false;
			}
			prg_bank_mask_32k = (Cart.prg_size / 32) - 1;

			prg_banks_32k[0] = 0;

			SetMirrorType(Cart.pad_h, Cart.pad_v);

			return true;
		}

		public override void Dispose()
		{
			prg_banks_32k.Dispose();
			base.Dispose();
		}

		public override byte ReadEXP(int addr)
		{
			//some kind of magic number..
			return 0x50;
		}

		public override void WritePRG(int addr, byte value)
		{
			prg_banks_32k[0] = value;
			ApplyMemoryMapMask(prg_bank_mask_32k, prg_banks_32k);
		}

		public override byte ReadPRG(int addr)
		{
			addr = ApplyMemoryMap(15, prg_banks_32k, addr);
			return ROM[addr];
		}

		public override void SyncState(Serializer ser)
		{
			base.SyncState(ser);
			ser.Sync("prg_banks_32k", ref prg_banks_32k);
		}
	}
}
