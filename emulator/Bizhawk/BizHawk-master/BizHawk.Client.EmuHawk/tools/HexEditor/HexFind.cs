﻿using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BizHawk.Client.EmuHawk
{
	public partial class HexFind : Form
	{
		public HexFind()
		{
			InitializeComponent();
			ChangeCasing();
		}

		// Hacky values to remember the Hex vs Text radio selection across searches
		public Action<bool> SearchTypeChangedCallback { get; set; }
		public bool InitialText { get; set; }

		public Point InitialLocation { get; set; }
		

		public string InitialValue
		{
			get { return FindBox.Text; }
			set { FindBox.Text = value ?? ""; }
		}

		private void HexFind_Load(object sender, EventArgs e)
		{
			if (InitialLocation.X > 0 && InitialLocation.Y > 0)
			{
				Location = InitialLocation;
			}

			if (InitialText)
			{
				TextRadio.Select();
			}

			FindBox.Focus();
			FindBox.Select();

		}

		private string GetFindBoxChars()
		{
			if (string.IsNullOrWhiteSpace(FindBox.Text))
			{
				return "";
			}
			
			if (HexRadio.Checked)
			{
				return FindBox.Text;
			}

			var bytes = GlobalWin.Tools.HexEditor.ConvertTextToBytes(FindBox.Text);

			var bytestring = new StringBuilder();
			foreach (var b in bytes)
			{
				bytestring.Append($"{b:X2}");
			}

			return bytestring.ToString();
		}

		private void Find_Prev_Click(object sender, EventArgs e)
		{
			GlobalWin.Tools.HexEditor.FindPrev(GetFindBoxChars(), false);
		}

		private void Find_Next_Click(object sender, EventArgs e)
		{
			GlobalWin.Tools.HexEditor.FindNext(GetFindBoxChars(), false);
		}

		private void ChangeCasing()
		{
			var text = FindBox.Text;
			var location = FindBox.Location;
			var size = FindBox.Size;

			Controls.Remove(FindBox);

			if (HexRadio.Checked)
			{
				FindBox = new HexTextBox
				{
					CharacterCasing = CharacterCasing.Upper,
					Nullable = HexRadio.Checked,
					Text = text,
					Size = size,
					Location = location
				};
			}
			else
			{
				FindBox = new TextBox
				{
					Text = text,
					Size = size,
					Location = location
				};
			}

			Controls.Add(FindBox);
		}

		private void HexRadio_CheckedChanged(object sender, EventArgs e)
		{
			ChangeCasing();
			SearchTypeChangedCallback?.Invoke(false);
		}

		private void TextRadio_CheckedChanged(object sender, EventArgs e)
		{
			ChangeCasing();
			SearchTypeChangedCallback?.Invoke(true);
		}

		private void FindBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Enter)
			{
				Find_Next_Click(null, null);
				e.Handled = true;
			}
		}

		private void HexFind_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				e.Handled = true;
				Close();
			}
			else if (e.KeyData == Keys.Enter)
			{
				Find_Next_Click(null, null);
				e.Handled = true;
			}
		}
	}
}
