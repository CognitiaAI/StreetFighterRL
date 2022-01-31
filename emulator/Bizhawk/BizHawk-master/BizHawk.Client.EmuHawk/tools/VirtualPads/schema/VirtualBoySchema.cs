﻿using System.Collections.Generic;
using System.Drawing;

using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	[Schema("VB")]
	public class VirtualBoySchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas(IEmulator core)
		{
			yield return StandardController();
			yield return ConsoleButtons();
		}

		private static PadSchema StandardController()
		{
			return new PadSchema
			{
				IsConsole = false,
				DefaultSize = new Size(222, 103),
				Buttons = new[]
				{
					new PadSchema.ButtonSchema
					{
						Name = "L_Up",
						DisplayName = "",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(14, 36),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "L_Down",
						DisplayName = "",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(14, 80),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "L_Left",
						DisplayName = "",
						Icon = Properties.Resources.Back,
						Location = new Point(2, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "L_Right",
						DisplayName = "",
						Icon = Properties.Resources.Forward,
						Location = new Point(24, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "B",
						DisplayName = "B",
						Location = new Point(122, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "A",
						DisplayName = "A",
						Location = new Point(146, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Select",
						DisplayName = "s",
						Location = new Point(52, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Start",
						DisplayName = "S",
						Location = new Point(74, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "R_Up",
						DisplayName = "",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(188, 36),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "R_Down",
						DisplayName = "",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(188, 80),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "R_Left",
						DisplayName = "",
						Icon = Properties.Resources.Back,
						Location = new Point(176, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "R_Right",
						DisplayName = "",
						Icon = Properties.Resources.Forward,
						Location = new Point(198, 58),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "L",
						DisplayName = "L",
						Location = new Point(24, 8),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "R",
						DisplayName = "R",
						Location = new Point(176, 8),
						Type = PadSchema.PadInputType.Boolean
					},
				}
			};
		}

		private static PadSchema ConsoleButtons()
		{
			return new PadSchema
			{
				DisplayName = "Console",
				IsConsole = true,
				DefaultSize = new Size(75, 50),
				Buttons = new[]
				{
					new PadSchema.ButtonSchema
					{
						Name = "Power",
						DisplayName = "Power",
						Location = new Point(10, 15),
						Type = PadSchema.PadInputType.Boolean
					}
				}
			};
		}
	}
}
