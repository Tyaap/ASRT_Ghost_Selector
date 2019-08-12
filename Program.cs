using System;
using System.Configuration;
using System.Windows.Forms;

namespace GhostSelector
{
    public class Program
    {
        private static Configuration configFile;
        public static ProgramConfigSection Config;

        static void Main()
        {
            Application.EnableVisualStyles();

            try
            {
                configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                Config = configFile.GetSection("ProgramConfig") as ProgramConfigSection;
            }
            catch (Exception Error)
            {
                MessageBox.Show("The following error occured when loading configuration from: GhostSelector.exe.config" +
                                "\n\n"
                                + Error.Message +
                                "\n\nPlease fix this error, or replace with a working config file.", "Could not load configuration.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            using (MainForm MainForm = new MainForm())
            {
                DialogResult result = MainForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    configFile.Save();

                    if (GameMemory.GetHandle())
                    {
                        GameMemory.LoadSettings();
                        MessageBox.Show(
                            "Your ghost settings have been applied!" +
                            "\n\nReturn to the main menu to ensure that all settings are applied.",
                            "Ghost Selector", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show(
                            "Your ghost settings have not been applied, but they have been saved." +
                            "\n\nTo apply your settings, you need to start the game first.",
                            "Ghost Selector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        } 
    }
}