﻿namespace BizHawk.Emulation.Common
{
	/// <summary>
	/// A generic implementation of ITraceable that can be used by any core
	/// </summary>
	/// <seealso cref="ITraceable" />
	public class TraceBuffer : ITraceable
	{
		public TraceBuffer()
		{
			Header = "Instructions";
		}

		public string Header { get; set; }

		public ITraceSink Sink { private get; set; }

		public bool Enabled => Sink != null;

		public void Put(TraceInfo info)
		{
			Sink.Put(info);
		}
	}
}
