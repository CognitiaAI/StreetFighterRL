﻿using System;
using System.Runtime.InteropServices;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Nintendo.Gameboy
{
	/// <summary>
	/// static bindings into libgambatte.dll
	/// </summary>
	public static class LibGambatte
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns>opaque state pointer</returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr gambatte_create();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_destroy(IntPtr core);

		[Flags]
		public enum LoadFlags : uint
		{
			/// <summary>Treat the ROM as not having CGB support regardless of what its header advertises</summary>
			FORCE_DMG = 1,
			/// <summary>Use GBA intial CPU register values when in CGB mode.</summary>
			GBA_CGB = 2,
			/// <summary>Use heuristics to detect and support some multicart MBCs disguised as MBC1.</summary>
			MULTICART_COMPAT = 4
		}

		public enum CDLog_AddrType : int
		{
			ROM, HRAM, WRAM, CartRAM
		}

		[Flags]
		public enum CDLog_Flags : int
		{
			ExecFirst = 1,
			ExecOperand = 2,
			Data = 4
		}

		/// <summary>
		/// Load ROM image.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="romdata">the rom data, can be disposed of once this function returns</param>
		/// <param name="length">length of romdata in bytes</param>
		/// <param name="now">RTC time when the rom is loaded</param>
		/// <param name="flags">ORed combination of LoadFlags.</param>
		/// <returns>0 on success, negative value on failure.</returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int gambatte_load(IntPtr core, byte[] romdata, uint length, byte[] biosdata, uint bioslength, long now, LoadFlags flags);

		/// <summary>
		/// Emulates until at least 'samples' stereo sound samples are produced in the supplied buffer,
		/// or until a video frame has been drawn.
		/// 
		/// There are 35112 stereo sound samples in a video frame.
		/// May run for up to 2064 stereo samples too long.
		/// A stereo sample consists of two native endian 2s complement 16-bit PCM samples,
		/// with the left sample preceding the right one.
		/// 
		/// Returns early when a new video frame has finished drawing in the video buffer,
		/// such that the caller may update the video output before the frame is overwritten.
		/// The return value indicates whether a new video frame has been drawn, and the
		/// exact time (in number of samples) at which it was drawn.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="soundbuf">buffer with space >= samples + 2064</param>
		/// <param name="samples">in: number of stereo samples to produce, out: actual number of samples produced</param>
		/// <returns>sample number at which the video frame was produced. -1 means no frame was produced.</returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int gambatte_runfor(IntPtr core, short[] soundbuf, ref uint samples);
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		unsafe public static extern int gambatte_runfor(IntPtr core, short* soundbuf, ref uint samples);

		/// <summary>
		/// blit from internal framebuffer to provided framebuffer
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="videobuf"></param>
		/// <param name="pitch">in pixels</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		unsafe public static extern void gambatte_blitto(IntPtr core, int* videobuf, int pitch);
		/// <summary>
		/// blit from internal framebuffer to provided framebuffer
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="videobuf"></param>
		/// <param name="pitch">in pixels</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_blitto(IntPtr core, int[] videobuf, int pitch);

		/// <summary>
		/// Reset to initial state.
		/// Equivalent to reloading a ROM image, or turning a Game Boy Color off and on again.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="now">RTC time when the reset occurs</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_reset(IntPtr core, long now);

		/// <summary>
		/// palette type for gambatte_setdmgpalettecolor
		/// </summary>
		public enum PalType : uint
		{
			BG_PALETTE = 0,
			SP1_PALETTE = 1,
			SP2_PALETTE = 2
		};

		/// <summary>
		/// 
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="palnum">in [0, 2]: One of BG_PALETTE, SP1_PALETTE and SP2_PALETTE.</param>
		/// <param name="colornum">in [0, 3]</param>
		/// <param name="rgb32"></param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setdmgpalettecolor(IntPtr core, PalType palnum, uint colornum, uint rgb32);

		/// <summary>
		/// set cgb palette lookup
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="lut">uint32[32768], input color (r,g,b) is at lut[r | g &lt;&lt; 5 | b &lt;&lt; 10]</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setcgbpalette(IntPtr core, int[] lut);

		/// <summary>
		/// combination of button flags used by the input callback
		/// </summary>
		[Flags]
		public enum Buttons: uint
		{
			A = 0x01,
			B = 0x02,
			SELECT = 0x04,
			START = 0x08,
			RIGHT = 0x10,
			LEFT = 0x20,
			UP = 0x40,
			DOWN = 0x80
		}

		/// <summary>
		/// type of the callback for input state
		/// </summary>
		/// <returns>bitfield combination of pressed buttons</returns>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate Buttons InputGetter();

		/// <summary>
		/// Sets the callback used for getting input state.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="getinput"></param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setinputgetter(IntPtr core, InputGetter getinput);

		/// <summary>
		/// type of the read\write memory callbacks
		/// </summary>
		/// <param name="address">the address which the cpu is read\writing</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void MemoryCallback(uint address);

		/// <summary>
		/// type of the CDLogger callback
		/// </summary>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void CDCallback(int addr, CDLog_AddrType addrtype, CDLog_Flags flags);

		/// <summary>
		/// set a callback to occur immediately BEFORE EVERY cpu read, except for opcode first byte fetches
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="callback">null to clear</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setreadcallback(IntPtr core, MemoryCallback callback);

		/// <summary>
		/// set a callback to occur immediately AFTER EVERY cpu write
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="callback">null to clear</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setwritecallback(IntPtr core, MemoryCallback callback);

		/// <summary>
		/// set a callback to occur immediately BEFORE EVERY cpu opcode (first byte) fetch
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="callback">null to clear</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setexeccallback(IntPtr core, MemoryCallback callback);

		/// <summary>
		/// set a callback whicih enables CD Logger feedback
		/// </summary>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setcdcallback(IntPtr core, CDCallback callback);

		/// <summary>
		/// type of the cpu trace callback
		/// </summary>
		/// <param name="data">cpu state</param>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void TraceCallback(IntPtr state);

		/// <summary>
		/// set a callback to occur immediately BEFORE each opcode is executed
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="callback">null to clear</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_settracecallback(IntPtr core, TraceCallback callback);

		/// <summary>
		/// sets layers to be rendered
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="mask">layermask, 1=BG, 2=OBJ, 4=WINDOW</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setlayers(IntPtr core, int mask);

		/// <summary>
		/// type of the scanline callback
		/// </summary>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ScanlineCallback();

		/// <summary>
		/// set a callback to occur when ly reaches a particular scanline (so at the beginning of the scanline).
		/// when the LCD is active, typically 145 will be the first callback after the beginning of frame advance,
		/// and 144 will be the last callback right before frame advance returns
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="callback">null to clear</param>
		/// <param name="sl">0-153 inclusive</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setscanlinecallback(IntPtr core, ScanlineCallback callback, int sl);

		/// <summary>
		/// type of the RTC callback
		/// </summary>
		/// <returns>what time is it, unixy</returns>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate uint RTCCallback();

		/// <summary>
		/// sets RTC callback.  probably mandatory.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="callback">the callback</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setrtccallback(IntPtr core, RTCCallback callback);
		
		/// <summary>
		/// type of the link data sent callback
		/// </summary>
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void LinkCallback();

		/// <summary>
		/// sets the Link data sent callback.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="callback">the callback</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_setlinkcallback(IntPtr core, LinkCallback callback);

		/// <summary>
		/// Returns true if the currently loaded ROM image is treated as having CGB support.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <returns></returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool gambatte_iscgb(IntPtr core);

		/// <summary>
		/// Returns true if a ROM image is loaded.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <returns></returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool gambatte_isloaded(IntPtr core);

		/// <summary>
		/// Get persistant cart memory.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="dest">byte buffer to write into.  gambatte_savesavedatalength() bytes will be written</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_savesavedata(IntPtr core, byte[] dest);

		/// <summary>
		/// restore persistant cart memory.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="data">byte buffer to read from.  gambatte_savesavedatalength() bytes will be read</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_loadsavedata(IntPtr core, byte[] data);

		/// <summary>
		/// get the size of the persistant cart memory block.  this value DEPENDS ON THE PARTICULAR CART LOADED
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <returns>length in bytes.  0 means no internal persistant cart memory</returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int gambatte_savesavedatalength(IntPtr core);

		/// <summary>
		/// new savestate method
		/// </summary>
		/// <param name="core"></param>
		/// <returns></returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int gambatte_newstatelen(IntPtr core);

		/// <summary>
		/// new savestate method
		/// </summary>
		/// <param name="core"></param>
		/// <param name="data"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool gambatte_newstatesave(IntPtr core, byte[] data, int len);

		/// <summary>
		/// new savestate method
		/// </summary>
		/// <param name="core"></param>
		/// <param name="data"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool gambatte_newstateload(IntPtr core, byte[] data, int len);

		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_newstatesave_ex(IntPtr core, ref TextStateFPtrs ff);

		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_newstateload_ex(IntPtr core, ref TextStateFPtrs ff);

		/// <summary>
		/// ROM header title of currently loaded ROM image.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="data">enough room for 16 ascii chars plus terminator</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_romtitle(IntPtr core, byte[] data);

		/// <summary>
		/// memory areas that gambatte_getmemoryarea() can return
		/// </summary>
		public enum MemoryAreas : int
		{
			vram = 0,
			rom = 1,
			wram = 2,
			cartram = 3,
			oam = 4,
			hram = 5,
			// these last two aren't returning native memory area data, but instead converted RGB32 colors
			bgpal = 6,
			sppal = 7
		}

		/// <summary>
		/// get pointer to internal memory areas, for debugging purposes
		/// so long as you don't write to it, you should be completely sync-safe
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="which">which memory area to access</param>
		/// <param name="data">pointer to the start of the area</param>
		/// <param name="length">valid length of the area, in bytes</param>
		/// <returns>success</returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool gambatte_getmemoryarea(IntPtr core, MemoryAreas which, ref IntPtr data, ref int length);

		/// <summary>
		/// read a single byte from the cpu bus.  this includes all ram, rom, mmio, etc, as it is visible to the cpu (including mappers).
		/// while there is no cycle cost to these reads, there may be other side effects!  use at your own risk.
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="addr">system bus address</param>
		/// <returns>byte read</returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern byte gambatte_cpuread(IntPtr core, ushort addr);

		/// <summary>
		/// write a single byte to the cpu bus.  while there is no cycle cost to these writes, there can be quite a few side effects.
		/// use at your own risk.
		/// </summary>
		/// <param name="core">opaque state pointe</param>
		/// <param name="addr">system bus address</param>
		/// <param name="val">byte to write</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_cpuwrite(IntPtr core, ushort addr, byte val);

		/// <summary>
		/// link cable stuff; never touch for normal operation
		/// </summary>
		/// <param name="core">opaque state pointe</param>
		/// <param name="which">todo</param>
		/// <returns>todo</returns>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int gambatte_linkstatus(IntPtr core, int which);

		/// <summary>
		/// get reg and flag values
		/// </summary>
		/// <param name="core">opaque state pointer</param>
		/// <param name="dest">length of at least 10, please</param>
		[DllImport("libgambatte.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern void gambatte_getregs(IntPtr core, int[] dest);

		public enum RegIndicies : int
		{
			PC, SP, A, B, C, D, E, F, H, L
		}
	}
}
