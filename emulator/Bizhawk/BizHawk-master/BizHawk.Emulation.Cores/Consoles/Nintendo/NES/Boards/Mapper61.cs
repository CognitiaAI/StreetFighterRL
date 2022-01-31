﻿using BizHawk.Common;
using BizHawk.Common.NumberExtensions;

namespace BizHawk.Emulation.Cores.Nintendo.NES
{
	public sealed class Mapper61 : NES.NESBoardBase
	{
		/*
		* Here are Disch's original notes:  
		========================
		=  Mapper 061          =
		========================
 
		Example Game:
		--------------------------
		20-in-1
 
 
		Registers:
		---------------------------
 
		$8000-FFFF:  A~[.... .... M.LO HHHH]
		H = High 4 bits of PRG Reg
		L = Low bit of PRG Reg
		O = PRG Mode
		M = Mirroring (0=Vert, 1=Horz)
 
 
		PRG Setup:
		---------------------------
 
		PRG Reg is 5 bits -- combination of 'H' and 'L' bits.
 
					  $8000   $A000   $C000   $E000  
					+-------------------------------+
		PRG Mode 0: |            <$8000>            |
					+-------------------------------+
		PRG Mode 1: |     $8000     |     $8000     |
					+---------------+---------------+
		*/

		public int prg_page;
		public bool prg_mode;
		public int prg_byte_mask;

		public override bool Configure(NES.EDetectionOrigin origin)
		{
			switch (Cart.board_type)
			{
				case "MAPPER061":
					break;
				default:
					return false;
			}
			SetMirrorType(EMirrorType.Vertical);
			prg_page = 0;
			prg_mode = false;
			prg_byte_mask = Cart.prg_size * 1024 - 1;
			return true;
		}

		public override void SyncState(Serializer ser)
		{
			ser.Sync("prg_page", ref prg_page);
			ser.Sync("prg_mode", ref prg_mode);
			base.SyncState(ser);
		}

		public override void WritePRG(int addr, byte value)
		{
			if (addr.Bit(7))
			{
				SetMirrorType(EMirrorType.Horizontal);
			}
			else
			{
				SetMirrorType(EMirrorType.Vertical);
			}

			prg_mode = addr.Bit(4);
			prg_page = ((addr & 0x0F) << 1) | ((addr & 0x20) >> 5);
		}

		public override byte ReadPRG(int addr)
		{
			if (prg_mode == false)
			{
				return ROM[(((prg_page >> 1) * 0x8000) + addr) & prg_byte_mask];
			}
			else
			{
				return ROM[((prg_page * 0x4000) + (addr & 0x03FFF)) & prg_byte_mask];
			}
		}
	}
}
