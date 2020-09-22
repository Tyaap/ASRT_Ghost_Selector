using System;
using System.Windows.Forms;

namespace GhostSelector
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadConfig();
        }

        public void LoadConfig()
        {
            CheckBoxPositionSelector.Checked = !Program.Config.FastestPlayerSelector.Enabled;
            NumericUpDownPosition.Value = Program.Config.PositionSelector.SelectedPosition;
            CheckBoxFastestPlayerSelector.Checked = Program.Config.FastestPlayerSelector.Enabled;

            ListViewPlayers.Items.Clear();
            foreach (PlayerElement Player in Program.Config.FastestPlayerSelector.Players)
            {
                ListViewPlayers.Items.Add(new ListViewItem(new string[] { Player.Name, Player.SteamId.ToString() }) { Checked = Player.Enabled });
            }

            TrackBarNameTagOpacity.Value = (int)(Program.Config.Graphics.Nametag.Opacity * 100);
            TrackBarGhostOpacity.Value = (int)(Program.Config.Graphics.PBGhost.Opacity * 100);
            CheckBoxHidePBGhost.Checked = Program.Config.Graphics.PBGhost.Hide;
            CheckBoxPBGhostColour.Checked = Program.Config.Graphics.PBGhost.UseCustomColour;
            ButtonPBGhostColour.BackColor = Program.Config.Graphics.PBGhost.Colour;
            CheckBoxOnlineGhostColour.Checked = Program.Config.Graphics.OnlineGhost.UseCustomColour;
            ButtonOnlineGhostColour.BackColor = Program.Config.Graphics.OnlineGhost.Colour;
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            Program.Config.FastestPlayerSelector.Enabled = CheckBoxFastestPlayerSelector.Checked;
            Program.Config.PositionSelector.SelectedPosition = (uint)NumericUpDownPosition.Value;

            Program.Config.FastestPlayerSelector.Players.Clear();
            foreach (ListViewItem Player in ListViewPlayers.Items)
                Program.Config.FastestPlayerSelector.Players.Add(new PlayerElement()
                {
                    Enabled = Player.Checked,
                    Name = Player.SubItems[0].Text,
                    SteamId = ulong.Parse(Player.SubItems[1].Text)
                });

            Program.Config.Graphics.Nametag.Opacity = (float)TrackBarNameTagOpacity.Value / 100;
            Program.Config.Graphics.PBGhost.Opacity = Program.Config.Graphics.OnlineGhost.Opacity = (float)TrackBarGhostOpacity.Value / 100;
            Program.Config.Graphics.PBGhost.Hide = CheckBoxHidePBGhost.Checked;
            Program.Config.Graphics.PBGhost.UseCustomColour = CheckBoxPBGhostColour.Checked;
            Program.Config.Graphics.PBGhost.Colour = ButtonPBGhostColour.BackColor;
            Program.Config.Graphics.OnlineGhost.UseCustomColour = CheckBoxOnlineGhostColour.Checked;
            Program.Config.Graphics.OnlineGhost.Colour = ButtonOnlineGhostColour.BackColor;

            Program.configFile.Save();

            if (GameMods.Initialise())
            {
                GameMods.LoadSettings();
                MessageBox.Show(
                    "Your settings have been applied!" +
                    "\nThey will be active until you close the game." +
                    "\nReturn to the main menu to ensure the selected ghost is downloaded.",
                    "Ghost Selector", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Your settings have not been applied, but they were saved." +
                    "\n\nTo apply your settings, you need to start the game first.",
                    "Ghost Selector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void ButtonPBGhostColour_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog() { Color = ButtonPBGhostColour.BackColor, FullOpen = true, })
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    ButtonPBGhostColour.BackColor = colorDialog.Color;
                }
            }
        }

        private void ButtonOnlineGhostColour_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog() { Color = ButtonOnlineGhostColour.BackColor, FullOpen = true, })
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    ButtonOnlineGhostColour.BackColor = colorDialog.Color;
                }
            }
        }
    }
}
