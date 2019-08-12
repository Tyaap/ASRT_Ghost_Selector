using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GhostSelector
{
    static class GameMemory
    {
        [DllImport("kernel32")]
        private static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("kernel32")]
        private static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        private static extern int GetLastError();

        // New Code
        static readonly byte[] fixLeaderboardPosition = new byte[] { 0x89, 0x93, 0xD4, 0x06, 0x00, 0x00, 0xC7, 0x83, 0xC8, 0x06, 0x00, 0x00 };

        static readonly byte[] setLeaderboardRange = new byte[] { 0xB2, 0x01, 0x90 };
        static readonly byte[] selectFirstDownloadedEntry = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
        static readonly byte[] changeLeaderboardDownload1 = new byte[] { 0x8B, 0x52, 0x74, 0x6A, 0x01, 0xEB, 0xA9, 0x90, 0x90 };
        static readonly byte[] changeLeaderboardDownload2 = new byte[] { 0x68, 0x17, 0x2E, 0xAF, 0x00, 0xEB, 0x52 };

        static readonly byte[] hideGhostCar = new byte[] { 0x90, 0x90, 0x90 };
        static readonly byte[] hideNameTags = new byte[] { 0xEB };
        static readonly byte[] hidePBGhost = new byte[] { 0x00 };

        // Old Code
        static readonly byte[] fixLeaderboardPosition_disable = new byte[] { 0x8B, 0x44, 0x24, 0x18, 0x89, 0x93, 0xD4, 0x06, 0x00, 0x00, 0x89, 0x83, 0xC8, 0x06, 0x00, 0x00 };
        static readonly byte[] hideGhostCar_disable = new byte[] { 0x89, 0x73, 0x0C };
        static readonly byte[] hideNameTags_disable = new byte[] { 0x75 };
        static readonly byte[] hidePBGhost_disable = new byte[] { 0x01 };

        // Addresses
        static readonly int addressFixLeaderboardPosition = 0x4EDC43;
        static readonly int offsetPosition = 12;

        static readonly int offsetPlayerCount = 4;
        static readonly int addressPlayerListStart = 0xAF2E17;
        static readonly long steamId = 76561198024504670;

        static readonly int addressSetLeaderboardRange = 0x4ED74E;
        static readonly int addressChangeLeaderboardDownload1 = 0x4EDB65;
        static readonly int addressChangeLeaderboardDownload2 = 0x4EDB15;
        static readonly int addressSelectFirstDownloadedEntry = 0x4EDCF5;

        static readonly int addressHideNametags = 0x888FF8;
        static readonly int addressHideGhostCar = 0x8D2A0C;
        static readonly int addressHidePBGhost = 0x899D71;

        // Game handle
        static int processHandle;
        static Process[] processList;

        public static bool GetHandle()
        {
            processList = Process.GetProcessesByName("ASN_App_PcDx9_Final");
            if (processList.Length > 0)
            {
                processHandle = OpenProcess(0x28, 0, processList[0].Id);
                return true;
            }
            else
                return false;

        }

        public static void LoadSettings()
        {
            WriteProcessMemory(processHandle, addressSetLeaderboardRange, setLeaderboardRange, setLeaderboardRange.Length, 0);
            WriteProcessMemory(processHandle, addressChangeLeaderboardDownload1, changeLeaderboardDownload1, changeLeaderboardDownload1.Length, 0);
            WriteProcessMemory(processHandle, addressChangeLeaderboardDownload2, changeLeaderboardDownload2, changeLeaderboardDownload2.Length, 0);
            WriteProcessMemory(processHandle, addressSelectFirstDownloadedEntry, selectFirstDownloadedEntry, selectFirstDownloadedEntry.Length, 0);

            if (Program.Config.FastestPlayerSelector.Enabled)
            {
                // Disable fixed position
                WriteProcessMemory(processHandle, addressFixLeaderboardPosition, fixLeaderboardPosition_disable, fixLeaderboardPosition_disable.Length, 0);

                // Load players into memory
                int playerCount = 0;
                foreach (PlayerElement Player in Program.Config.FastestPlayerSelector.Players)
                    if (Player.Enabled)
                    {
                        WriteProcessMemory(processHandle, addressPlayerListStart + playerCount * 8, System.BitConverter.GetBytes(Player.SteamId) , 8, 0);
                        playerCount++;
                    }

                WriteProcessMemory(processHandle, addressChangeLeaderboardDownload1 + offsetPlayerCount, new byte[] { (byte) playerCount }, 1, 0);
            }

            else
            {
                // Disable player selector
                WriteProcessMemory(processHandle, addressPlayerListStart, System.BitConverter.GetBytes(steamId), 8, 0);
                WriteProcessMemory(processHandle, addressChangeLeaderboardDownload1 + offsetPlayerCount, new byte[] { 0x01 }, 1, 0);
               
                // Fix the position
                WriteProcessMemory(processHandle, addressFixLeaderboardPosition, fixLeaderboardPosition, fixLeaderboardPosition.Length, 0);
                if (Program.Config.PositionSelector.SelectedPosition > 0)
                    WriteProcessMemory(processHandle, addressFixLeaderboardPosition + offsetPosition, System.BitConverter.GetBytes(Program.Config.PositionSelector.SelectedPosition), 4, 0);
                else
                    WriteProcessMemory(processHandle, addressFixLeaderboardPosition + offsetPosition, System.BitConverter.GetBytes(0xFFFFFFF), 4, 0);
            }

            if (Program.Config.Graphics.HideGhostCars)
                WriteProcessMemory(processHandle, addressHideGhostCar, hideGhostCar, hideGhostCar.Length, 0);
            else
                WriteProcessMemory(processHandle, addressHideGhostCar, hideGhostCar_disable, hideGhostCar_disable.Length, 0);

            if (Program.Config.Graphics.HideNameTags)
                WriteProcessMemory(processHandle, addressHideNametags, hideNameTags, hideNameTags.Length, 0);
            else
                WriteProcessMemory(processHandle, addressHideNametags, hideNameTags_disable, hideNameTags_disable.Length, 0);

            if (Program.Config.Graphics.HidePBGhost)
                WriteProcessMemory(processHandle, addressHidePBGhost, hidePBGhost, hidePBGhost.Length, 0);
            else
                WriteProcessMemory(processHandle, addressHidePBGhost, hidePBGhost_disable, hidePBGhost_disable.Length, 0);
        }
    }
}
