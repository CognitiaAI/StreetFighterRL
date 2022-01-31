﻿using System;

namespace BizHawk.Emulation.Common
{
	/// <summary>
	/// This object facilitates communications between client and core
	/// and is used by the IEmulator interface
	/// The primary use is to provide a client => core communication, such as providing client-side callbacks for a core to use
	/// Any communications that can be described as purely a Core -> Client system, should be provided as an <seealso cref="IEmulatorService"/> instead
	/// </summary>
	/// <seealso cref="IEmulator" />
	public class CoreComm
	{
		public CoreComm(Action<string> showMessage, Action<string> notifyMessage)
		{
			ShowMessage = showMessage;
			Notify = notifyMessage;
		}

		public ICoreFileProvider CoreFileProvider { get; set; }

		// a core should set these if you wish to provide rom status information yourself. otherwise it will be calculated by the frontend in a way you may not like, using RomGame-related concepts.
		public string RomStatusAnnotation { get; set; }
		public string RomStatusDetails { get; set; }

		public int ScreenLogicalOffsetX { get; set; }
		public int ScreenLogicalOffsetY { get; set; }

		// size hint to a/v out resizer.  this probably belongs in VideoProvider?  but it's somewhat different than VirtualWidth...
		public int NominalWidth { get; set; } = 640;
		public int NominalHeight { get; set; } = 480;

		// I know we want to get rid of CoreComm, but while it's still here, I'll use it for this
		public string LaunchLibretroCore { get; set; }

		/// <summary>
		/// Gets a message to show. reasonably annoying (dialog box), shouldn't be used most of the time
		/// </summary>
		public Action<string> ShowMessage { get; private set; }

		/// <summary>
		/// Gets a message to show. less annoying (OSD message). Should be used for ignorable helpful messages
		/// </summary>
		public Action<string> Notify { get; private set; }

		public Func<int, int, bool, object> RequestGLContext { get; set; }
		public Action<object> ReleaseGLContext { get; set; }
		public Action<object> ActivateGLContext { get; set; }
		public Action DeactivateGLContext { get; set; } // this shouldnt be necessary.. frontend should be changing context before it does anything.. but for now..
	}
}
