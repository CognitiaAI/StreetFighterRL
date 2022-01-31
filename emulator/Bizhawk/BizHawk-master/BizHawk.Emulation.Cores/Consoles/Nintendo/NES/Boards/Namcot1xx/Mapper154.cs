﻿using BizHawk.Common;
using BizHawk.Common.NumberExtensions;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	// http://wiki.nesdev.com/w/index.php/INES_Mapper_154
	public sealed class Mapper154 : Namcot108Board_Base
	{
		//configuration
		int chr_bank_mask_1k;

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			//analyze board type
			switch (Cart.board_type)
			{
				case "NAMCOT-3453":
				case "MAPPER154":
					break;
				default:
					return false;
			}

			BaseSetup();
			SetMirrorType(EMirrorType.OneScreenA);

			chr_bank_mask_1k = Cart.chr_size - 1;

			return true;
		}

		int RewireCHR(int addr)
		{
			int bank_1k = mapper.Get_CHRBank_1K(addr);
			bank_1k &= 0x3F;
			if (addr >= 0x1000)
				bank_1k |= 0x40;
			bank_1k &= chr_bank_mask_1k;
			int ofs = addr & ((1 << 10) - 1);
			addr = (bank_1k << 10) + ofs;
			return addr;
		}

		public override byte ReadPPU(int addr)
		{
			if (addr < 0x2000) return VROM[RewireCHR(addr)];
			else return base.ReadPPU(addr);
		}
		public override void WritePPU(int addr, byte value)
		{
			if (addr < 0x2000) { }
			else base.WritePPU(addr, value);
		}

		public override void WritePRG(int addr, byte value)
		{
			if (value.Bit(6))
			{
				SetMirrorType(EMirrorType.OneScreenB);
			}
			else
			{
				SetMirrorType(EMirrorType.OneScreenA);
			}

			base.WritePRG(addr, value);
		}
	}
}
