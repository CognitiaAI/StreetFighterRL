﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using BizHawk.Emulation.Common;
using BizHawk.Common;

namespace BizHawk.Emulation.Cores.Consoles.Sega.gpgx
{
	public partial class GPGX
	{
		private IMemoryDomains MemoryDomains;

		private unsafe void SetMemoryDomains()
		{
			using (_elf.EnterExit())
			{
				var mm = new List<MemoryDomain>();
				for (int i = LibGPGX.MIN_MEM_DOMAIN; i <= LibGPGX.MAX_MEM_DOMAIN; i++)
				{
					IntPtr area = IntPtr.Zero;
					int size = 0;
					IntPtr pname = Core.gpgx_get_memdom(i, ref area, ref size);
					if (area == IntPtr.Zero || pname == IntPtr.Zero || size == 0)
						continue;
					string name = Marshal.PtrToStringAnsi(pname);
					if (name == "VRAM")
					{
						// vram pokes need to go through hook which invalidates cached tiles
						byte* p = (byte*)area;
						mm.Add(new MemoryDomainDelegate(name, size, MemoryDomain.Endian.Unknown,
							delegate (long addr)
							{
								if (addr < 0 || addr >= 65536)
									throw new ArgumentOutOfRangeException();
								using (_elf.EnterExit())
									return p[addr ^ 1];
							},
							delegate (long addr, byte val)
							{
								if (addr < 0 || addr >= 65536)
									throw new ArgumentOutOfRangeException();
								Core.gpgx_poke_vram(((int)addr) ^ 1, val);
							},
							wordSize: 2));
					}
					else
					{
						// TODO: are the Z80 domains really Swap16 in the core?  Check this
						mm.Add(new MemoryDomainIntPtrSwap16Monitor(name, MemoryDomain.Endian.Big, area, size, name != "MD CART" && name != "CD BOOT ROM", _elf));
					}
				}
				var m68Bus = new MemoryDomainDelegate("M68K BUS", 0x1000000, MemoryDomain.Endian.Big,
					delegate (long addr)
					{
						var a = (uint)addr;
						if (a >= 0x1000000)
							throw new ArgumentOutOfRangeException();
						return Core.gpgx_peek_m68k_bus(a);
					},
					delegate (long addr, byte val)
					{
						var a = (uint)addr;
						if (a >= 0x1000000)
							throw new ArgumentOutOfRangeException();
						Core.gpgx_write_m68k_bus(a, val);
					}, 2);

				mm.Add(m68Bus);

				if (IsMegaCD)
				{
					var s68Bus = new MemoryDomainDelegate("S68K BUS", 0x1000000, MemoryDomain.Endian.Big,
					delegate (long addr)
					{
						var a = (uint)addr;
						if (a >= 0x1000000)
							throw new ArgumentOutOfRangeException();
						return Core.gpgx_peek_s68k_bus(a);
					},
					delegate (long addr, byte val)
					{
						var a = (uint)addr;
						if (a >= 0x1000000)
							throw new ArgumentOutOfRangeException();
						Core.gpgx_write_s68k_bus(a, val);
					}, 2);


					mm.Add(s68Bus);
				}

				MemoryDomains = new MemoryDomainList(mm);
				MemoryDomains.SystemBus = m68Bus;

				(ServiceProvider as BasicServiceProvider).Register<IMemoryDomains>(MemoryDomains);
			}
		}
	}
}
