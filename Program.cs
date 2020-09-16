using System;
using System.Configuration;
using System.Windows.Forms;

namespace GhostSelector
{
    public class Program
    {
        public static Configuration configFile;
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

            Application.Run(new MainForm());
        } 
    }
}