﻿using System;
using System.Windows.Forms;

using BizHawk.Emulation.Cores.Sony.PSX;
using BizHawk.Client.Common;

namespace BizHawk.Client.EmuHawk
{
	public partial class PSXControllerConfigNew : Form
	{
		public PSXControllerConfigNew()
		{
			InitializeComponent();
		}

		private void PSXControllerConfigNew_Load(object sender, EventArgs e)
		{
			// populate combo boxes
			foreach (var combo in new[] { combo_1_1, combo_1_2, combo_1_3, combo_1_4, combo_2_1, combo_2_2, combo_2_3, combo_2_4 })
			{
				combo.Items.Add("-Nothing-");
				combo.Items.Add("Gamepad");
				combo.Items.Add("Dual Shock");
				combo.Items.Add("Dual Analog");
				combo.Items.Add("neGcon");
				combo.SelectedIndex = 0;
			}

			var psxSettings = ((Octoshock)Global.Emulator).GetSyncSettings();
			GuiFromUserConfig(psxSettings.FIOConfig);

			RefreshLabels();
		}

		private void GuiFromUserConfig(OctoshockFIOConfigUser user)
		{
			cbMemcard_1.Checked = user.Memcards[0];
			cbMemcard_2.Checked = user.Memcards[1];
			cbMultitap_1.Checked = user.Multitaps[0];
			cbMultitap_2.Checked = user.Multitaps[1];

			var combos = new[] { combo_1_1, combo_1_2, combo_1_3, combo_1_4, combo_2_1, combo_2_2, combo_2_3, combo_2_4 };
			for (int i = 0; i < 8; i++)
			{
				var combo = combos[i];
				if (user.Devices8[i] == OctoshockDll.ePeripheralType.None) combo.SelectedIndex = 0;
				if (user.Devices8[i] == OctoshockDll.ePeripheralType.Pad) combo.SelectedIndex = 1;
				if (user.Devices8[i] == OctoshockDll.ePeripheralType.DualShock) combo.SelectedIndex = 2;
				if (user.Devices8[i] == OctoshockDll.ePeripheralType.DualAnalog) combo.SelectedIndex = 3;
				if (user.Devices8[i] == OctoshockDll.ePeripheralType.NegCon) combo.SelectedIndex = 4;
			}
		}

		private OctoshockFIOConfigUser UserConfigFromGui()
		{
			var uc = new OctoshockFIOConfigUser
			{
				Memcards = { [0] = cbMemcard_1.Checked, [1] = cbMemcard_2.Checked },
				Multitaps = { [0] = cbMultitap_1.Checked, [1] = cbMultitap_2.Checked }
			};

			var combos = new[] { combo_1_1, combo_1_2, combo_1_3, combo_1_4, combo_2_1, combo_2_2, combo_2_3, combo_2_4 };
			for (int i = 0; i < 8; i++)
			{
				var combo = combos[i];
				if (combo.SelectedIndex == 0) uc.Devices8[i] = OctoshockDll.ePeripheralType.None;
				if (combo.SelectedIndex == 1) uc.Devices8[i] = OctoshockDll.ePeripheralType.Pad;
				if (combo.SelectedIndex == 2) uc.Devices8[i] = OctoshockDll.ePeripheralType.DualShock;
				if (combo.SelectedIndex == 3) uc.Devices8[i] = OctoshockDll.ePeripheralType.DualAnalog;
				if (combo.SelectedIndex == 4) uc.Devices8[i] = OctoshockDll.ePeripheralType.NegCon;
			}

			return uc;
		}

		private void RefreshLabels()
		{
			var uc = UserConfigFromGui();

			bool b1 = uc.Multitaps[0];
			lbl_1_1.Visible = b1;
			lbl_1_2.Visible = b1;
			lbl_1_3.Visible = b1;
			lbl_1_4.Visible = b1;
			combo_1_2.Enabled = b1;
			combo_1_3.Enabled = b1;
			combo_1_4.Enabled = b1;
			lbl_p_1_2.Visible = b1;
			lbl_p_1_3.Visible = b1;
			lbl_p_1_4.Visible = b1;

			bool b2 = uc.Multitaps[1];
			lbl_2_1.Visible = b2;
			lbl_2_2.Visible = b2;
			lbl_2_3.Visible = b2;
			lbl_2_4.Visible = b2;
			combo_2_2.Enabled = b2;
			combo_2_3.Enabled = b2;
			combo_2_4.Enabled = b2;
			lbl_p_2_2.Visible = b2;
			lbl_p_2_3.Visible = b2;
			lbl_p_2_4.Visible = b2;

			var lc = uc.ToLogical();

			var pLabels = new[] { lbl_p_1_1, lbl_p_1_2, lbl_p_1_3, lbl_p_1_4, lbl_p_2_1, lbl_p_2_2, lbl_p_2_3, lbl_p_2_4 };
			for (int i = 0; i < 8; i++)
			{
				var lbl = pLabels[i];
				if (lc.PlayerAssignments[i] == -1)
				{
					lbl.Visible = false;
				}
				else
				{
					lbl.Text = "P" + lc.PlayerAssignments[i];
					lbl.Visible = true;
				}
			}
		}

		private void Cb_Changed(object sender, EventArgs e)
		{
			RefreshLabels();
		}

		private void Combo_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshLabels();
		}

		private void BtnOk_Click(object sender, EventArgs e)
		{
			var psxSettings = ((Octoshock)Global.Emulator).GetSyncSettings();

			psxSettings.FIOConfig = UserConfigFromGui();
			GlobalWin.MainForm.PutCoreSyncSettings(psxSettings);
			
			DialogResult = DialogResult.OK;
			
			Close();
		}
	}
}
