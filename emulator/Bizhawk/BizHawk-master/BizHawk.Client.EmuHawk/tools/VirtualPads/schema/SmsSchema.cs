﻿using System.Collections.Generic;
using System.Drawing;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Sega.MasterSystem;

namespace BizHawk.Client.EmuHawk
{
	[Schema("SMS")]
	public class SmsSchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas(IEmulator core)
		{
			if (((SMS)core).IsGameGear)
			{
				yield return GGController(1);
				yield return GGConsoleButtons();
			}
			else
			{
				yield return StandardController(1);
				yield return StandardController(2);
				yield return SmsConsoleButtons();
			}
		}

		private static PadSchema StandardController(int controller)
		{
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(174, 90),
				Buttons = new[]
				{
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Up",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(14, 12),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Down",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(14, 56),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Left",
						Icon = Properties.Resources.Back,
						Location = new Point(2, 34),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Right",
						Icon = Properties.Resources.Forward,
						Location = new Point(24, 34),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " B1",
						DisplayName = "1",
						Location = new Point(122, 34),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " B2",
						DisplayName = "2",
						Location = new Point(146, 34),
						Type = PadSchema.PadInputType.Boolean
					}
				}
			};
		}

		private static PadSchema GGController(int controller)
		{
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(174, 90),
				Buttons = new[]
				{
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Up",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(14, 12),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Down",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(14, 56),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Left",
						Icon = Properties.Resources.Back,
						Location = new Point(2, 34),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Right",
						Icon = Properties.Resources.Forward,
						Location = new Point(24, 34),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " Start",
						DisplayName = "S",
						Location = new Point(134, 12),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " B1",
						DisplayName = "1",
						Location = new Point(122, 34),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P" + controller + " B2",
						DisplayName = "2",
						Location = new Point(146, 34),
						Type = PadSchema.PadInputType.Boolean
					}
				}
			};
		}

		private static PadSchema SmsConsoleButtons()
		{
			return new PadSchema
			{
				DisplayName = "Console",
				IsConsole = true,
				DefaultSize = new Size(150, 50),
				Buttons = new[]
				{
					new PadSchema.ButtonSchema
					{
						Name = "Reset",
						DisplayName = "Reset",
						Location = new Point(10, 15),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Pause",
						DisplayName = "Pause",
						Location = new Point(58, 15),
						Type = PadSchema.PadInputType.Boolean
					}
				}
			};
		}

		private static PadSchema GGConsoleButtons()
		{
			return new PadSchema
			{
				DisplayName = "Console",
				IsConsole = true,
				DefaultSize = new Size(150, 50),
				Buttons = new[]
				{
					new PadSchema.ButtonSchema
					{
						Name = "Reset",
						DisplayName = "Reset",
						Location = new Point(10, 15),
						Type = PadSchema.PadInputType.Boolean
					}
				}
			};
		}
	}
}
