using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace GhostSelector
{
    public partial class MainForm : Form
    {
        List<RadioButton> radiobuttons;
        string lastFilePath = "";
        string lastFolderPath = "";

        public MainForm()
        {
            InitializeComponent();

            // radio button auto unchecking
            radiobuttons = new List<RadioButton> { radioButtonDisable, radioButtonDefault, radioButtonLeaderboardRank, radioButtonFastestPlayer, radioButtonFromFile };
            radiobuttons.ForEach(r => r.CheckedChanged += (o, e) =>
            {
                if (r.Checked) radiobuttons.ForEach(rb => rb.Checked = rb == r);
            });

            LoadConfig();
        }

        public void LoadConfig()
        {
            radiobuttons[(int)Program.Config.GhostSelectors.Choice].Checked = true;

            NumericUpDownPosition.Value = Program.Config.GhostSelectors.LeaderboardRank.Rank;

            ListViewPlayers.Items.Clear();
            foreach (PlayerElement Player in Program.Config.GhostSelectors.FastestPlayer)
            {
                ListViewPlayers.Items.Add(new ListViewItem(new string[] { Player.Name, Player.SteamId.ToString() }) { Checked = Player.Enabled });
            }
     
            textBoxNameTag.Text = Program.Config.GhostSelectors.FromFile.NameTag;
            textBoxFile.Text = Program.Config.GhostSelectors.FromFile.File;

            TrackBarNameTagOpacity.Value = (int)(Program.Config.Graphics.Nametag.Opacity * 100);
            TrackBarGhostOpacity.Value = (int)(Program.Config.Graphics.PBGhost.Opacity * 100);
            CheckBoxHidePBGhost.Checked = Program.Config.Graphics.PBGhost.Hide;
            CheckBoxPBGhostColour.Checked = Program.Config.Graphics.PBGhost.ChangeColour;
            ButtonPBGhostColour.BackColor = Program.Config.Graphics.PBGhost.Colour;
            CheckBoxOnlineGhostColour.Checked = Program.Config.Graphics.RivalGhost.ChangeColour;
            ButtonOnlineGhostColour.BackColor = Program.Config.Graphics.RivalGhost.Colour;

            checkBoxGhostSaverEnabled.Checked = Program.Config.GhostSaver.Enabled;
            textBoxFolder.Text = Program.Config.GhostSaver.Folder;
        }

        public void SaveConfig()
        {
            Program.Config.GhostSelectors.Choice = (GhostSelector)radiobuttons.FindIndex(x => x.Checked);

            Program.Config.GhostSelectors.LeaderboardRank.Rank = (uint)NumericUpDownPosition.Value;

            Program.Config.GhostSelectors.FastestPlayer.Clear();
            foreach (ListViewItem Player in ListViewPlayers.Items)
            {
                Program.Config.GhostSelectors.FastestPlayer.Add(new PlayerElement()
                {
                    Enabled = Player.Checked,
                    Name = Player.SubItems[0].Text,
                    SteamId = ulong.Parse(Player.SubItems[1].Text)
                });
            }

            Program.Config.GhostSelectors.FromFile.NameTag = textBoxNameTag.Text;
            Program.Config.GhostSelectors.FromFile.File = textBoxFile.Text;

            Program.Config.Graphics.Nametag.Opacity = (float)TrackBarNameTagOpacity.Value / 100;
            Program.Config.Graphics.PBGhost.Opacity = Program.Config.Graphics.RivalGhost.Opacity = (float)TrackBarGhostOpacity.Value / 100;
            Program.Config.Graphics.PBGhost.Hide = CheckBoxHidePBGhost.Checked;
            Program.Config.Graphics.PBGhost.ChangeColour = CheckBoxPBGhostColour.Checked;
            Program.Config.Graphics.PBGhost.Colour = ButtonPBGhostColour.BackColor;
            Program.Config.Graphics.RivalGhost.ChangeColour = CheckBoxOnlineGhostColour.Checked;
            Program.Config.Graphics.RivalGhost.Colour = ButtonOnlineGhostColour.BackColor;

            Program.Config.GhostSaver.Enabled = checkBoxGhostSaverEnabled.Checked;
            Program.Config.GhostSaver.Folder = textBoxFolder.Text;

            Program.configFile.Save();
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

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in ListViewPlayers.SelectedItems)
                ListViewPlayers.Items.Remove(Item);
        }

        private void buttonBrowseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog() { Filter = "Ghost Files|*.ghost", Multiselect = false, Title = "Select a ghost data file", InitialDirectory = lastFilePath, CheckFileExists = true })
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxFile.Text = fileDialog.FileName;
                    lastFilePath = Path.GetDirectoryName(fileDialog.FileName);
                }
            }
        }

        private void ButtonSaveAndApply_Click(object sender, EventArgs e)
        {
            SaveConfig();

            if (GameMods.Initialise())
            {
                GameMods.LoadSettings();
                MessageBox.Show(
                    "Your settings have been saved and applied!" +
                    "\nThey will be active until you close the game." +
                    "\nReturn to the main menu to ensure the new rival ghost is loaded",
                    "Ghost Selector", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(
                    "Your settings have been saved, but were not applied." +
                    "\n\nTo apply your settings, you need to start the game first.",
                    "Ghost Selector", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonUndoChanges_Click(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void buttonBrowseFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog() { Description = "Choose an output folder", SelectedPath = lastFolderPath })
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBoxFolder.Text = folderDialog.SelectedPath;
                    lastFolderPath = folderDialog.SelectedPath;
                }
            }
        }
    }
}
