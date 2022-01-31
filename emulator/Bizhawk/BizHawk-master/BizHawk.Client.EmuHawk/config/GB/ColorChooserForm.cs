﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using BizHawk.Client.Common;
using BizHawk.Emulation.Cores.Nintendo.Gameboy;

namespace BizHawk.Client.EmuHawk
{
	public partial class ColorChooserForm : Form
	{
		private ColorChooserForm()
		{
			InitializeComponent();
		}

		private readonly Color[] _colors = new Color[12];

		// gambatte's default dmg colors
		private static readonly int[] DefaultCGBColors =
		{
			0x00ffffff, 0x00aaaaaa, 0x00555555, 0x00000000,
			0x00ffffff, 0x00aaaaaa, 0x00555555, 0x00000000,
			0x00ffffff, 0x00aaaaaa, 0x00555555, 0x00000000
		};

		// bsnes's default dmg colors with slight tweaking
		private static readonly int[] DefaultDMGColors =
		{
			10798341, 8956165, 1922333, 337157,
			10798341, 8956165, 1922333, 337157,
			10798341, 8956165, 1922333, 337157
		};

		private void RefreshAllBackdrops()
		{
			panel1.BackColor = _colors[0];
			panel2.BackColor = _colors[1];
			panel3.BackColor = _colors[2];
			panel4.BackColor = _colors[3];
			panel5.BackColor = _colors[4];
			panel6.BackColor = _colors[5];
			panel7.BackColor = _colors[6];
			panel8.BackColor = _colors[7];
			panel9.BackColor = _colors[8];
			panel10.BackColor = _colors[9];
			panel11.BackColor = _colors[10];
			panel12.BackColor = _colors[11];
		}

		private Color Betweencolor(Color left, Color right, double pos)
		{
			int r = (int)(right.R * pos + left.R * (1.0 - pos) + 0.5);
			int g = (int)(right.G * pos + left.G * (1.0 - pos) + 0.5);
			int b = (int)(right.B * pos + left.B * (1.0 - pos) + 0.5);
			int a = (int)(right.A * pos + left.A * (1.0 - pos) + 0.5);

			return Color.FromArgb(a, r, g, b);
		}

		private void InterpolateColors(int firstindex, int lastindex)
		{
			for (int i = firstindex + 1; i < lastindex; i++)
			{
				double pos = (i - firstindex) / (double)(lastindex - firstindex);
				_colors[i] = Betweencolor(_colors[firstindex], _colors[lastindex], pos);
			}

			RefreshAllBackdrops();
		}

		private void Button3_Click(object sender, EventArgs e)
		{
			InterpolateColors(0, 3);
		}

		private void Button4_Click(object sender, EventArgs e)
		{
			InterpolateColors(4, 7);
		}

		private void Button5_Click(object sender, EventArgs e)
		{
			InterpolateColors(8, 11);
		}

		private void Panel12_DoubleClick(object sender, EventArgs e)
		{
			Panel psender = (Panel)sender;

			int i;
			if (psender == panel1)
				i = 0;
			else if (psender == panel2)
				i = 1;
			else if (psender == panel3)
				i = 2;
			else if (psender == panel4)
				i = 3;
			else if (psender == panel5)
				i = 4;
			else if (psender == panel6)
				i = 5;
			else if (psender == panel7)
				i = 6;
			else if (psender == panel8)
				i = 7;
			else if (psender == panel9)
				i = 8;
			else if (psender == panel10)
				i = 9;
			else if (psender == panel11)
				i = 10;
			else if (psender == panel12)
				i = 11;
			else
				return; // i = -1;

			using (var dlg = new ColorDialog())
			{
				dlg.AllowFullOpen = true;
				dlg.AnyColor = true;
				dlg.Color = _colors[i];

				// custom colors are ints, not Color structs?
				// and they don't work right unless the alpha bits are set to 0
				// and the rgb order is switched
				int[] customs = new int[12];
				for (int j = 0; j < customs.Length; j++)
				{
					customs[j] = _colors[j].R | _colors[j].G << 8 | _colors[j].B << 16;
				}

				dlg.CustomColors = customs;
				dlg.FullOpen = true;

				var result = dlg.ShowDialog(this);

				if (result == DialogResult.OK)
				{
					if (_colors[i] != dlg.Color)
					{
						_colors[i] = dlg.Color;
						psender.BackColor = _colors[i];
					}
				}
			}
		}

		// ini keys for gambatte palette file
		private static readonly string[] Paletteinikeys =
		{
			"Background0",
			"Background1",
			"Background2",
			"Background3",
			"Sprite%2010",
			"Sprite%2011",
			"Sprite%2012",
			"Sprite%2013",
			"Sprite%2020",
			"Sprite%2021",
			"Sprite%2022",
			"Sprite%2023"
		};

		/// <summary>
		/// load gambatte-style .pal file
		/// </summary>
		/// <returns>null on failure</returns>
		public static int[] LoadPalFile(TextReader f)
		{
			var lines = new Dictionary<string, int>();

			string line;
			while ((line = f.ReadLine()) != null)
			{
				int i = line.IndexOf('=');
				if (i < 0)
				{
					continue;
				}

				try
				{
					lines.Add(line.Substring(0, i), int.Parse(line.Substring(i + 1)));
				}
				catch (FormatException)
				{
				}
			}

			int[] ret = new int[12];
			try
			{
				for (int i = 0; i < 12; i++)
				{
					ret[i] = lines[Paletteinikeys[i]];
				}
			}
			catch (KeyNotFoundException)
			{
				return null;
			}

			return ret;
		}

		// save gambatte-style palette file
		private static void SavePalFile(TextWriter f, int[] colors)
		{
			f.WriteLine("[General]");
			for (int i = 0; i < 12; i++)
			{
				f.WriteLine($"{Paletteinikeys[i]}={colors[i]}");
			}
		}

		private void SetAllColors(int[] colors)
		{
			// fix alpha to 255 in created color objects, else problems
			for (int i = 0; i < _colors.Length; i++)
			{
				_colors[i] = Color.FromArgb(255, Color.FromArgb(colors[i]));
			}

			RefreshAllBackdrops();
		}

		private static void DoColorChooserFormDialog(IWin32Window parent, Gameboy.GambatteSettings s, bool fromemu)
		{
			using (var dlg = new ColorChooserForm())
			{
				var gb = Global.Emulator as Gameboy;
				if (fromemu)
				{
					s = gb.GetSettings();
				}

				dlg.SetAllColors(s.GBPalette);

				var result = dlg.ShowDialog(parent);
				if (result == DialogResult.OK)
				{
					int[] colorints = new int[12];
					for (int i = 0; i < 12; i++)
					{
						colorints[i] = dlg._colors[i].ToArgb();
					}

					s.GBPalette = colorints;
					if (fromemu)
					{
						gb.PutSettings(s);
					}
				}
			}
		}

		public static void DoColorChooserFormDialog(IWin32Window parent, Gameboy.GambatteSettings s)
		{
			DoColorChooserFormDialog(parent, s, false);
		}

		private void LoadColorFile(string filename, bool alert)
		{
			try
			{
				using (var f = new StreamReader(filename))
				{
					int[] newcolors = LoadPalFile(f);
					if (newcolors == null)
					{
						throw new Exception();
					}

					SetAllColors(newcolors);
				}
			}
			catch
			{
				if (alert)
				{
					MessageBox.Show(this, "Error loading .pal file!");
				}
			}
		}

		private void SaveColorFile(string filename)
		{
			try
			{
				using (var f = new StreamWriter(filename))
				{
					int[] savecolors = new int[12];
					for (int i = 0; i < 12; i++)
					{
						// clear alpha because gambatte color files don't usually contain it
						savecolors[i] = _colors[i].ToArgb() & 0xffffff;
					}

					SavePalFile(f, savecolors);
				}
			}
			catch
			{
				MessageBox.Show(this, "Error saving .pal file!");
			}
		}

		private void Button6_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.InitialDirectory = PathManager.MakeAbsolutePath(Global.Config.PathEntries["GB", "Palettes"].Path, "GB");
				ofd.Filter = "Gambatte Palettes (*.pal)|*.pal|All Files|*.*";
				ofd.RestoreDirectory = true;

				var result = ofd.ShowDialog(this);
				if (result == DialogResult.OK)
				{
					LoadColorFile(ofd.FileName, true);
				}
			}
		}

		private void ColorChooserForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var files = (string[])e.Data.GetData(DataFormats.FileDrop);

				if (files.Length > 1)
				{
					return;
				}

				LoadColorFile(files[0], true);
			}
		}

		private void ColorChooserForm_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop)
				? DragDropEffects.Move
				: DragDropEffects.None;
		}

		private void Button7_Click(object sender, EventArgs e)
		{
			using (var sfd = new SaveFileDialog())
			{
				sfd.InitialDirectory = PathManager.MakeAbsolutePath(Global.Config.PathEntries["GB", "Palettes"].Path, "GB");
				sfd.FileName = Global.Game.Name + ".pal";

				sfd.Filter = "Gambatte Palettes (*.pal)|*.pal|All Files|*.*";
				sfd.RestoreDirectory = true;
				var result = sfd.ShowDialog(this);
				if (result == DialogResult.OK)
				{
					SaveColorFile(sfd.FileName);
				}
			}
		}

		private void Ok_Click(object sender, EventArgs e)
		{
		}

		private void DefaultButton_Click(object sender, EventArgs e)
		{
			SetAllColors(DefaultDMGColors);
		}

		private void DefaultButtonCGB_Click(object sender, EventArgs e)
		{
			SetAllColors(DefaultCGBColors);
		}
	}
}
