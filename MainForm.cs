using System;
using System.Windows.Forms;

namespace GhostSelector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            CheckBoxPositionSelector.Checked = !Program.Config.FastestPlayerSelector.Enabled;
            NumericUpDownPosition.Value = Program.Config.PositionSelector.SelectedPosition;

            CheckBoxFastestPlayerSelector.Checked = Program.Config.FastestPlayerSelector.Enabled;
            foreach (PlayerElement Player in Program.Config.FastestPlayerSelector.Players)
                ListViewPlayers.Items.Add(new ListViewItem(new string[] { Player.Name, Player.SteamId.ToString() }) { Checked = Player.Enabled });

            CheckedListBoxGraphics.SetItemChecked(0, Program.Config.Graphics.HideNameTags);
            CheckedListBoxGraphics.SetItemChecked(1, Program.Config.Graphics.HideGhostCars);
            CheckedListBoxGraphics.SetItemChecked(2, Program.Config.Graphics.HidePBGhost);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Config.FastestPlayerSelector.Enabled = CheckBoxFastestPlayerSelector.Checked;
            Program.Config.PositionSelector.SelectedPosition = (uint)NumericUpDownPosition.Value;

            Program.Config.FastestPlayerSelector.Players.Clear();
            foreach (ListViewItem Player in ListViewPlayers.Items)
                Program.Config.FastestPlayerSelector.Players.Add(new PlayerElement()
                {
                    Enabled = Player.Checked,
                    Name = Player.SubItems[0].Text,
                    SteamId = long.Parse(Player.SubItems[1].Text)
                });

            Program.Config.Graphics.HideNameTags = (CheckedListBoxGraphics.GetItemCheckState(0) == CheckState.Checked);
            Program.Config.Graphics.HideGhostCars = (CheckedListBoxGraphics.GetItemCheckState(1) == CheckState.Checked);
            Program.Config.Graphics.HidePBGhost = (CheckedListBoxGraphics.GetItemCheckState(2) == CheckState.Checked);
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in ListViewPlayers.SelectedItems)
                ListViewPlayers.Items.Remove(Item);
        }

        private void CheckBoxPositionSelector_Click(object sender, EventArgs e)
        {
            CheckBoxFastestPlayerSelector.Checked = !CheckBoxPositionSelector.Checked;
        }

        private void CheckBoxFastestPlayerSelector_Click(object sender, EventArgs e)
        {
            CheckBoxPositionSelector.Checked = !CheckBoxFastestPlayerSelector.Checked;
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (DialogBoxPlayer DialogBox = new DialogBoxPlayer())
            {
                DialogBox.Text = "Add Player";
                DialogBox.ButtonConfirm.Text = "Add";
                DialogBox.TextBoxPlayerName.Select();

                while (true)
                {
                    DialogResult Result = DialogBox.ShowDialog();

                    try
                    {
                        if (Result == DialogResult.OK)
                        {
                            long.Parse(DialogBox.TextBoxPlayerSteamID.Text);
                            ListViewPlayers.Items.Add(new ListViewItem(
                                new string[] { DialogBox.TextBoxPlayerName.Text, DialogBox.TextBoxPlayerSteamID.Text })
                            { Checked = DialogBox.CheckBoxPlayerEnabled.Checked });
                        }

                        break;
                    }
                    catch
                    {
                        MessageBox.Show("The Steam ID is invalid.", "Could not add player.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in ListViewPlayers.SelectedItems)
            {
                using (DialogBoxPlayer DialogBox = new DialogBoxPlayer())
                {
                    int index = Item.Index;

                    DialogBox.Text = "Edit Player";
                    DialogBox.ButtonConfirm.Text = "Confirm";
                    DialogBox.TextBoxPlayerName.Select();
                    DialogBox.TextBoxPlayerName.Text = ListViewPlayers.Items[index].SubItems[0].Text;
                    DialogBox.TextBoxPlayerSteamID.Text = ListViewPlayers.Items[index].SubItems[1].Text;
                    DialogBox.CheckBoxPlayerEnabled.Checked = ListViewPlayers.Items[index].Checked;

                    while (true)
                    {
                        DialogResult Result = DialogBox.ShowDialog();

                        try
                        {
                            if (Result == DialogResult.OK)
                            {
                                long.Parse(DialogBox.TextBoxPlayerSteamID.Text);
                                ListViewPlayers.Items[index].SubItems[0].Text = DialogBox.TextBoxPlayerName.Text;
                                ListViewPlayers.Items[index].SubItems[1].Text = DialogBox.TextBoxPlayerSteamID.Text;
                                ListViewPlayers.Items[index].Checked = DialogBox.CheckBoxPlayerEnabled.Checked;
                            }

                            break;
                        }
                        catch
                        {
                            MessageBox.Show("The Steam ID is invalid.", "Could not edit player.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
