﻿using System;
using System.IO;
using BizHawk.Common;
using BizHawk.Emulation.Common;

namespace BizHawk.Emulation.Cores.Components.CP1610
{
	public sealed partial class CP1610
	{
		private const ushort RESET = 0x1000;
		private const ushort INTERRUPT = 0x1004;

		internal bool FlagS, FlagC, FlagZ, FlagO, FlagI, FlagD, IntRM, BusRq, BusAk, Interruptible, Interrupted;
		//private bool MSync;
		internal ushort[] Register = new ushort[8];
		private ushort RegisterSP { get { return Register[6]; } set { Register[6] = value; } }
		private ushort RegisterPC { get { return Register[7]; } set { Register[7] = value; } }

		public string TraceHeader
		{
			get
			{
				return "CP1610: PC, machine code, mnemonic, operands, flags (SCZOID)";
			}
		}

		public Action<TraceInfo> TraceCallback;
		public IMemoryCallbackSystem MemoryCallbacks { get; set; }

		public ushort ReadMemoryWrapper(ushort addr, bool peek)
		{
			if (MemoryCallbacks != null && !peek)
			{
				MemoryCallbacks.CallReads(addr);
			}

			return ReadMemory(addr, peek);
		}

		public void WriteMemoryWrapper(ushort addr, ushort value, bool poke)
		{
			if (MemoryCallbacks != null && !poke)
			{
				MemoryCallbacks.CallWrites(addr);
			}

			WriteMemory(addr, value, poke);
		}

		public int TotalExecutedCycles;
		public int PendingCycles;

		public Func<ushort, bool, ushort> ReadMemory;
		public Func<ushort, ushort, bool, bool> WriteMemory;

		private static bool Logging = false;
		private static readonly StreamWriter Log;

		public void SyncState(Serializer ser)
		{
			ser.BeginSection("CP1610");

			ser.Sync("Register", ref Register, false);
			ser.Sync("FlagS", ref FlagS);
			ser.Sync("FlagC", ref FlagC);
			ser.Sync("FlagZ", ref FlagZ);
			ser.Sync("FlagO", ref FlagO);
			ser.Sync("FlagI", ref FlagI);
			ser.Sync("FlagD", ref FlagD);
			ser.Sync("IntRM", ref IntRM);
			ser.Sync("BusRq", ref BusRq);
			ser.Sync("BusAk", ref BusAk);
			ser.Sync("BusRq", ref BusRq);
			ser.Sync("Interruptible", ref Interruptible);
			ser.Sync("Interrupted", ref Interrupted);
			ser.Sync("Toal_executed_cycles", ref TotalExecutedCycles);
			ser.Sync("Pending_Cycles", ref PendingCycles);


			ser.EndSection();
		}

	static CP1610()
		{
			if (Logging)
			{
				Log = new StreamWriter("log_CP1610.txt");
			}
		}

		public void Reset()
		{
			BusAk = true;
			Interruptible = false;
			FlagS = FlagC = FlagZ = FlagO = FlagI = FlagD = false;
			for (int register = 0; register <= 6; register++)
			{
				Register[register] = 0;
			}
			RegisterPC = RESET;
			PendingCycles = 0;
		}

		public bool GetBusAk()
		{
			return BusAk;
		}

		public void SetIntRM(bool value)
		{
			IntRM = value;
			if (IntRM)
			{
				Interrupted = false;
			}
		}

		public void SetBusRq(bool value)
		{
			BusRq = !value;
		}

		public int GetPendingCycles()
		{
			return PendingCycles;
		}

		public void AddPendingCycles(int cycles)
		{
			PendingCycles += cycles;
		}

		public void LogData()
		{
			if (!Logging)
			{
				return;
			}
			Log.WriteLine("Total Executed Cycles = {0}", TotalExecutedCycles);
			for (int register = 0; register <= 5; register++)
			{
				Log.WriteLine("R{0:d} = {1:X4}", register, Register[register]);
			}
			Log.WriteLine("SP = {0:X4}", RegisterSP);
			Log.WriteLine("PC = {0:X4}", RegisterPC);
			Log.WriteLine("S = {0}", FlagS);
			Log.WriteLine("C = {0}", FlagC);
			Log.WriteLine("Z = {0}", FlagZ);
			Log.WriteLine("O = {0}", FlagO);
			Log.WriteLine("I = {0}", FlagI);
			Log.WriteLine("D = {0}", FlagD);
			Log.WriteLine("INTRM = {0}", IntRM);
			Log.WriteLine("BUSRQ = {0}", BusRq);
			Log.WriteLine("BUSAK = {0}", BusAk);
			// Log.WriteLine("MSYNC = {0}", MSync);
			Log.Flush();
		}

		
	}
}
