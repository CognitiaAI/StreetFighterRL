﻿using System.Collections.Generic;
using System.Drawing;

using BizHawk.Emulation.Common;

namespace BizHawk.Client.EmuHawk
{
	[Schema("AppleII")]
	public class AppleIISchema : IVirtualPadSchema
	{
		public IEnumerable<PadSchema> GetPadSchemas(IEmulator core)
		{
			yield return Keyboard();
			yield return DiskSelection();
		}

		private static PadSchema Keyboard()
		{
			return new PadSchema
			{
				DisplayName = "Keyboard",
				IsConsole = false,
				DefaultSize = new Size(390, 150),
				Buttons = new[]
				{
					/************************** Row 1 **************************/
					new PadSchema.ButtonSchema
					{
						Name = "Escape",
						DisplayName = "Esc",
						Location = new Point(10, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "1",
						DisplayName = "1",
						Location = new Point(46, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "2",
						DisplayName = "2",
						Location = new Point(70, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "3",
						DisplayName = "3",
						Location = new Point(94, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "4",
						DisplayName = "4",
						Location = new Point(118, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "5",
						DisplayName = "5",
						Location = new Point(142, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "6",
						DisplayName = "6",
						Location = new Point(166, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "7",
						DisplayName = "7",
						Location = new Point(190, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "8",
						DisplayName = "8",
						Location = new Point(214, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "9",
						DisplayName = "9",
						Location = new Point(238, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "0",
						DisplayName = "0",
						Location = new Point(262, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "-",
						DisplayName = "-",
						Location = new Point(286, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "=",
						DisplayName = "=",
						Location = new Point(307, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Delete",
						DisplayName = "Delete",
						Location = new Point(331, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					
					/************************** Row 2 **************************/
					new PadSchema.ButtonSchema
					{
						Name = "Tab",
						DisplayName = " Tab ",
						Location = new Point(10, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Q",
						DisplayName = "Q",
						Location = new Point(52, 42),
						Type = PadSchema.PadInputType.Boolean
					},

					new PadSchema.ButtonSchema
					{
						Name = "W",
						DisplayName = "W",
						Location = new Point(78, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "E",
						DisplayName = "E",
						Location = new Point(106, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "R",
						DisplayName = "R",
						Location = new Point(130, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "T",
						DisplayName = "T",
						Location = new Point(156, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Y",
						DisplayName = "Y",
						Location = new Point(180, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "U",
						DisplayName = "U",
						Location = new Point(204, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "I",
						DisplayName = "I",
						Location = new Point(230, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "O",
						DisplayName = "O",
						Location = new Point(250, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "P",
						DisplayName = "P",
						Location = new Point(276, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "[",
						DisplayName = "[",
						Location = new Point(302, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "]",
						DisplayName = "]",
						Location = new Point(325, 42),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "\\",
						DisplayName = " \\ ",
						Location = new Point(349, 42),
						Type = PadSchema.PadInputType.Boolean
					},

					/************************** Row 3 **************************/
					new PadSchema.ButtonSchema
					{
						Name = "Control",
						DisplayName = " Control ",
						Location = new Point(10, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "A",
						DisplayName = "A",
						Location = new Point(66, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "S",
						DisplayName = "S",
						Location = new Point(90, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "D",
						DisplayName = "D",
						Location = new Point(114, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "F",
						DisplayName = "F",
						Location = new Point(140, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "G",
						DisplayName = "G",
						Location = new Point(164, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "H",
						DisplayName = "H",
						Location = new Point(190, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "J",
						DisplayName = "J",
						Location = new Point(216, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "K",
						DisplayName = "K",
						Location = new Point(238, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "L",
						DisplayName = "L",
						Location = new Point(262, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = ";",
						DisplayName = ";",
						Location = new Point(286, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "'",
						DisplayName = "'",
						Location = new Point(307, 66),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Return",
						DisplayName = "Return",
						Location = new Point(328, 66),
						Type = PadSchema.PadInputType.Boolean
					},

					/************************** Row 4 **************************/
					new PadSchema.ButtonSchema
					{
						Name = "Shift",
						DisplayName = "     Shift     ",
						Location = new Point(10, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Z",
						DisplayName = "Z",
						Location = new Point(80, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "X",
						DisplayName = "X",
						Location = new Point(106, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "C",
						DisplayName = "C",
						Location = new Point(130, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "V",
						DisplayName = "V",
						Location = new Point(154, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "B",
						DisplayName = "B",
						Location = new Point(178, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "N",
						DisplayName = "N",
						Location = new Point(202, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "M",
						DisplayName = "M",
						Location = new Point(226, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = ",",
						DisplayName = ",",
						Location = new Point(252, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = ".",
						DisplayName = ".",
						Location = new Point(272, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "/",
						DisplayName = "/",
						Location = new Point(292, 90),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Shift",
						DisplayName = "    Shift    ",
						Location = new Point(315, 90),
						Type = PadSchema.PadInputType.Boolean
					},

					/************************** Row 5 **************************/

					new PadSchema.ButtonSchema
					{
						Name = "Caps Lock",
						DisplayName = "Caps",
						Location = new Point(10, 114),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "`",
						DisplayName = "~",
						Location = new Point(52, 114),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "White Apple",
						DisplayName = "<",
						Location = new Point(96, 114),
						Type = PadSchema.PadInputType.Boolean
					},

					new PadSchema.ButtonSchema
					{
						Name = "Space",
						DisplayName = "                Space                ",
						Location = new Point(120, 114),
						Type = PadSchema.PadInputType.Boolean
					},

					new PadSchema.ButtonSchema
					{
						Name = "Black Apple",
						DisplayName = ">",
						Location = new Point(265, 114),
						Type = PadSchema.PadInputType.Boolean
					},

					new PadSchema.ButtonSchema
					{
						Name = "Left",
						DisplayName = "",
						Icon = Properties.Resources.Back,
						Location = new Point(289, 114),
						Type = PadSchema.PadInputType.Boolean
					},

					new PadSchema.ButtonSchema
					{
						Name = "Right",
						DisplayName = "",
						Icon = Properties.Resources.Forward,
						Location = new Point(311, 114),
						Type = PadSchema.PadInputType.Boolean
					},

					new PadSchema.ButtonSchema
					{
						Name = "Down",
						DisplayName = "",
						Icon = Properties.Resources.BlueDown,
						Location = new Point(333, 114),
						Type = PadSchema.PadInputType.Boolean
					},

					new PadSchema.ButtonSchema
					{
						Name = "Up",
						DisplayName = "",
						Icon = Properties.Resources.BlueUp,
						Location = new Point(355, 114),
						Type = PadSchema.PadInputType.Boolean
					}
				}
			};
		}

		private static PadSchema DiskSelection()
		{
			return new PadSchema
			{
				DisplayName = "Disk Selection",
				IsConsole = false,
				DefaultSize = new Size(120, 50),
				Buttons = new[]
				{
					new PadSchema.ButtonSchema
					{
						Name = "Next Disk",
						DisplayName = "Next",
						Location = new Point(10, 18),
						Type = PadSchema.PadInputType.Boolean
					},
					new PadSchema.ButtonSchema
					{
						Name = "Previous Disk",
						DisplayName = "Previous",
						Location = new Point(50, 18),
						Type = PadSchema.PadInputType.Boolean
					},
				}
			};
		}
	}
}
