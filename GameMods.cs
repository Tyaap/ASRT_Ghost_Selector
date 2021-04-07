using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static MemoryHelper;

namespace GhostSelector
{
    static class GameMods
    {
        // New Code
        static readonly byte[] fixLeaderboardRank = new byte[] { 0x89, 0x93, 0xD4, 0x06, 0x00, 0x00, 0xC7, 0x83, 0xC8, 0x06, 0x00, 0x00 };

        static readonly byte[] setLeaderboardRange = new byte[] { 0xB2, 0x01, 0x90 };
        static readonly byte[] selectFirstDownloadedEntry = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
        static readonly byte[] changeLeaderboardDownload1 = new byte[] { 0x8B, 0x52, 0x74, 0x6A, 0x01, 0xEB, 0xA9, 0x90, 0x90 };
        static readonly byte[] changeLeaderboardDownload2 = new byte[] { 0x68, 0x17, 0x2E, 0xAF, 0x00, 0xEB, 0x52 };
        static readonly byte[] skipUGCRetry = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };

        static readonly byte[] hidePBGhost = new byte[] { 0x00 };
        static readonly byte[] dontHideRival = new byte[] { 0x00 };

        // Original Code
        static readonly byte[] fixLeaderboardRank_disable = new byte[] { 0x8B, 0x44, 0x24, 0x18, 0x89, 0x93, 0xD4, 0x06, 0x00, 0x00, 0x89, 0x83, 0xC8, 0x06, 0x00, 0x00 };

        static readonly byte[] setLeaderboardRange_disable = new byte[] { 0x8A, 0x55, 0x18 };
        static readonly byte[] selectFirstDownloadedEntry_disable = new byte[] { 0x0F, 0x82, 0x05, 0xFF, 0xFF, 0xFF };
        static readonly byte[] changeLeaderboardDownload1_disable = new byte[] { 0x8B, 0x52, 0x70, 0x6A, 0x00, 0x6A, 0x00, 0x6A, 0x01 };
        static readonly byte[] changeLeaderboardDownload2_disable = new byte[] { 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC };
        static readonly byte[] skipUGCRetry_disable = new byte[] { 0x0F, 0x84, 0xD0, 0x00, 0x00, 0x00 };

        static readonly byte[] hidePBGhost_disable = new byte[] { 0x01 };
        static readonly byte[] dontHideRival_disable = new byte[] { 0x01 };

        // Addresses
        const int addressFixLeaderboardPosition = 0x4EDC43;
        const int offsetPosition = 12;

        const int offsetPlayerCount = 4;
        const int addressPlayerListStart = 0xAF2E17;

        const int addressSetLeaderboardRange = 0x4ED74E;
        const int addressChangeLeaderboardDownload1 = 0x4EDB65;
        const int addressChangeLeaderboardDownload2 = 0x4EDB15;
        const int addressSelectFirstDownloadedEntry = 0x4EDCF5;
        const int addressSkipUGCRetry = 0x875987;

        const int addressHidePBGhost = 0x899D71;
        const int addressDontHideRival = 0x79B098;

        public static bool Initialise()
        {
            Process[] processList = Process.GetProcessesByName("ASN_App_PcDx9_Final");
            return processList.Length > 0 && MemoryHelper.Initialise(processList[0].Id);
        }

        public static void LoadSettings()
        {
            // Reset ghost selector to default
            Write(0x872EA9, (byte)0x74);
            Write(addressSetLeaderboardRange, setLeaderboardRange_disable);
            Write(addressSelectFirstDownloadedEntry, selectFirstDownloadedEntry_disable);
            Write(addressChangeLeaderboardDownload1, changeLeaderboardDownload1_disable);
            Write(addressChangeLeaderboardDownload2, changeLeaderboardDownload2_disable);
            Write(addressSkipUGCRetry, skipUGCRetry_disable);
            Write(addressFixLeaderboardPosition, fixLeaderboardRank_disable);
            Write(0xC5308C, 0); // Clear any existing loaded ghost
            Write(0x798C0E, (byte)0x74); // enable memory deallocation

            switch (Program.Config.GhostSelectors.Choice)
            {
                case GhostSelector.Disabled:
                    Write(0x872EA9, (byte)0xEB);
                    break;
                case GhostSelector.Default:
                    break;
                case GhostSelector.LeaderboardRank:
                    Write(addressSetLeaderboardRange, setLeaderboardRange);
                    Write(addressSelectFirstDownloadedEntry, selectFirstDownloadedEntry);
                    Write(addressSkipUGCRetry, skipUGCRetry);
                    Write(addressFixLeaderboardPosition, fixLeaderboardRank);
                    Write(addressFixLeaderboardPosition + offsetPosition, Program.Config.GhostSelectors.LeaderboardRank.Rank);
                    break;
                case GhostSelector.FastestPlayer:
                    Write(addressSetLeaderboardRange, setLeaderboardRange);
                    Write(addressSelectFirstDownloadedEntry, selectFirstDownloadedEntry);
                    Write(addressSkipUGCRetry, skipUGCRetry);
                    Write(addressChangeLeaderboardDownload1, changeLeaderboardDownload1);
                    Write(addressChangeLeaderboardDownload2, changeLeaderboardDownload2);
                    // Load players into memory
                    int address = addressPlayerListStart - 8;
                    foreach (PlayerElement Player in Program.Config.GhostSelectors.FastestPlayer)
                    {
                        if (Player.Enabled)
                        {
                            Write(address += 8, Player.SteamId);
                        }
                    }
                    Write(addressChangeLeaderboardDownload1 + offsetPlayerCount, (byte)Program.Config.GhostSelectors.FastestPlayer.Count);
                    break;
                case GhostSelector.FromFile:
                    Write(0x872EA9, (byte)0xEB); // disable online ghost loading
                    Write(0xC530B4, Program.Config.GhostSelectors.FromFile.NameTag);
                    try
                    {
                        byte[] ghostData = File.ReadAllBytes(Program.Config.GhostSelectors.FromFile.File);
                        int pGhostData = Allocate(0, ghostData.Length);
                        Write(pGhostData, ghostData);
                        Write(0xC5308C, pGhostData);
                        Write(0x798C0E, (byte)0xEB); // prevent memory deallocation
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Could not load ghost data file!\n" +
                            "Rival ghost will be disabled.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;
            }

            // Graphics
            if (Program.Config.Graphics.PBGhost.Hide)
            {
                Write(addressHidePBGhost, hidePBGhost);
            }
            else
            {
                Write(addressHidePBGhost, hidePBGhost_disable);
            }
            if (Program.Config.Graphics.RivalGhost.Hide)
            {
                Write(addressDontHideRival, dontHideRival_disable);
            }
            else
            {
                Write(addressDontHideRival, dontHideRival);
            }
            SetNameTagOpacity(Program.Config.Graphics.Nametag.Opacity);
            CustomGhostAppearance(
                Program.Config.Graphics.PBGhost.ChangeColour ? ToRGBA(Program.Config.Graphics.PBGhost.Colour) : 0,
                Program.Config.Graphics.RivalGhost.ChangeColour ? ToRGBA(Program.Config.Graphics.RivalGhost.Colour) : 0,
                1);

            // Ghost saver
            if (Program.Config.GhostSaver.Enabled)
            {
                Write(0x89ABFD, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 });
                string filePath = Program.Config.GhostSaver.Folder + "\\%s_%s_%.3f.ghost";
                byte[] filePathBytes = Encoding.UTF8.GetBytes(filePath + (char)0);
                int pFilePath = Allocate(0, filePathBytes.Length);
                Write(pFilePath, filePathBytes);
                Write(0x89AC79, pFilePath);
                Write(0x60C61C, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 } ); // don't append .// to start of file path
                Write(0x60C629, (byte)0xEB); // (continued)
                Write(0x89AC6C, (byte)0x0C); // use track file path names
            }
            else
            {
                Write(0x89ABFD, new byte[] { 0x0F, 0x84, 0x49, 0x01, 0x00, 0x00 });
            }
        }

        public static void SetNameTagOpacity(float opacity)
        {
            int codeAddress;
            if (ReadByte(0x889E32) == 0xE8)
            {
                // code is already present, get the address
                codeAddress = ReadInt(0x889E33) + 0x889E37;
            }
            else
            {
                // code not present, load it
                codeAddress = Allocate(0, 17);
                List<byte> myCode = new List<byte>();
                myCode.AddRange(new byte[] { 0x74, 0x08 });                         // je (next instruction) + 8
                myCode.AddRange(new byte[] { 0xD9, 0x05 });                         // fld st(0) dword ...
                myCode.AddRange(BitConverter.GetBytes(codeAddress + 13));           // ... [codeAddress + 13]
                myCode.AddRange(new byte[] { 0xEB, 0x02 });                         // jmp (next instruction) + 2
                myCode.AddRange(new byte[] { 0xD9, 0xEE });                         // fldz
                myCode.Add(0xC3);                                                   // ret
                Write(codeAddress, myCode.ToArray());

                List<byte> jumpCode = new List<byte>();
                jumpCode.Add(0xE8);                                                 // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress - 0x889E37));   // ... codeAddress
                jumpCode.Add(0x90);                                                 // nop
                jumpCode.Add(0x90);                                                 // nop
                jumpCode.Add(0x90);                                                 // nop
                Write(0x889E32, jumpCode.ToArray());
            }
            // Set the opacity
            Write(codeAddress + 13, opacity);
        }

        public static void CustomGhostAppearance(int pbColour, int onlineColour, float opacity)
        {
            int codeAddress;
            if (ReadByte(0x8999EE) == 0xE8)
            {
                // code is already present, get the address
                codeAddress = ReadInt(0x8999EF) + 0x8999EE + 5;
            }
            else
            {
                // code not present, load it
                codeAddress = Allocate(0, 100) + 20;
                List<byte> myCode = new List<byte>();

                // [codeAddress - 0x14] = PB ghost pointer
                // [codeAddress - 0x10] = online ghost pointer
                // [codeAddress - 0xC]  = PB ghost colour
                // [codeAddress - 0x8]  = online ghost colour
                // [codeAddress - 0x4]  = ghost opacity

                // Code part 1 - save PB ghost pointer #1
                int codeAddress1 = codeAddress;
                myCode.AddRange(new byte[] { 0x8D, 0x8D, 0x90, 0x4B, 0x00, 0x00 });         // lea ecx, [ebp+4B90]
                myCode.AddRange(new byte[] { 0x89, 0x0D });                                 // mov dword [...], ecx
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0x14));                   // ... codeAddress - 0x14
                myCode.Add(0xC3);                                                           // ret

                // Code part 2 - save online ghost pointer
                int codeAddress2 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0x8D, 0x8D, 0xE0, 0xC7, 0x01, 0x00 });         // lea ecx, [ebp+1C7E0]
                myCode.AddRange(new byte[] { 0x89, 0x0D });                                 // mov dword [...], ecx
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0x10));                 // ... codeAddress - 0x10
                myCode.Add(0xC3);                                                           // ret

                // Code part 3 - save PB ghost pointer #2
                int codeAddress3 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0x81, 0xC1, 0x90, 0x4B, 0x00, 0x00 });         // add ecx, 4B90
                myCode.AddRange(new byte[] { 0x89, 0x0D });                                 // mov dword [...], ecx
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0x14));                 // ... codeAddress - 0x14
                myCode.Add(0xC3);                                                           // ret

                // Code part 4 - backup original colour, replace it
                int codeAddress4 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0x8B, 0x98, 0x0C, 0x02, 0x00, 0x00 });         // mov ebx, dword [eax+0x20c]
                myCode.AddRange(new byte[] { 0x39, 0x35 });                                 // cmp dword [...], esi
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0x14));                 // ... codeAddress - 0x14
                myCode.AddRange(new byte[] { 0x75, 0xC });                                  // jne (next instruction) + 0xC
                myCode.AddRange(new byte[] { 0x8b, 0x15 });                                 // mov edx, dword [...]
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0xC));                  // ... codeAddress - 0xC
                myCode.AddRange(new byte[] { 0x89, 0x90, 0x0C, 0x02, 0x00, 0x00 });         // mov dword [eax+0x20c], edx
                myCode.AddRange(new byte[] { 0x39, 0x35 });                                 // cmp dword [...], esi
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0x10));                 // ... = codeAddress - 0x10
                myCode.AddRange(new byte[] { 0x75, 0xC });                                  // jne (next instruction) + 0xC
                myCode.AddRange(new byte[] { 0x8b, 0x15 });                                 // mov edx, dword [...]
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0x8));                  // ... codeAddress - 0x8
                myCode.AddRange(new byte[] { 0x89, 0x90, 0x0C, 0x02, 0x00, 0x00 });         // mov dword [eax+0x20c], edx
                myCode.AddRange(new byte[] { 0x0F, 0xB6, 0x90, 0x0C, 0x02, 0x00, 0x00 });   // movzx edx, byte [eax+0x20c]
                myCode.Add(0xC3);                                                           // ret

                // Code part 5 - restore original colour
                int codeAddress5 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0x0F, 0xB6, 0x90, 0x0E, 0x02, 0x00, 0x00 });   // movzx edx, byte [eax+0x20e]
                myCode.AddRange(new byte[] { 0x89, 0x98, 0x0C, 0x02, 0x00, 0x00 });         // mov dword [eax+0x20c],ebx
                myCode.AddRange(new byte[] { 0x89, 0xD0 });                                 // mov eax, edx
                myCode.Add(0xC3);                                                           // ret

                // Code part 6 - opacity
                int codeAddress6 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0xD9, 0x05 });                                 // fld st0, dword [...]
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 0x4));                  // ... codeAddress - 0x4
                myCode.AddRange(new byte[] { 0xD9, 0x58, 0x0C });                           // fstp dword [eax+0xC], st0
                myCode.Add(0xC3);                                                           // ret

                Write(codeAddress, myCode.ToArray());

                // Jump 1
                List<byte> jumpCode = new List<byte>();
                jumpCode.Add(0xE8);                                                         // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress1 - 0x8999EE - 5));      // ... codeAddress1
                jumpCode.Add(0x90);                                                         // nop
                Write(0x8999EE, jumpCode.ToArray());

                // Jump 2
                jumpCode.Clear();
                jumpCode.Add(0xE8);                                                         // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress2 - 0x899A33 - 5));      // ... codeAddress2
                jumpCode.Add(0x90);                                                         // nop
                Write(0x899A33, jumpCode.ToArray());

                // Jump 3
                jumpCode.Clear();
                jumpCode.Add(0xE8);                                                         // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress3 - 0x899A6F - 5));      // ... codeAddress3
                jumpCode.Add(0x90);                                                         // nop
                Write(0x899A6F, jumpCode.ToArray());

                // Jump 4
                jumpCode.Clear();
                jumpCode.Add(0xE8);                                                         // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress4 - 0x8D3283 - 5));      // ... codeAddress4
                jumpCode.Add(0x90);                                                         // nop
                jumpCode.Add(0x90);                                                         // nop
                Write(0x8D3283, jumpCode.ToArray());

                // Jump 5
                jumpCode.Clear();
                jumpCode.Add(0xE8);                                                         // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress5 - 0x8D32A5 - 5));      // ... codeAddress5
                jumpCode.Add(0x90);                                                         // nop
                jumpCode.Add(0x90);                                                         // nop
                Write(0x8D32A5, jumpCode.ToArray());

                // Jump 6
                jumpCode.Clear();
                jumpCode.Add(0xE8);                                                         // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress6 - 0x8D3309 - 5));      // ... codeAddress6
                Write(0x8D3309, jumpCode.ToArray());

                // Jump 7
                jumpCode.Clear();
                jumpCode.Add(0xE8);                                                         // call ...
                jumpCode.AddRange(BitConverter.GetBytes(codeAddress6 - 0x8D3353 - 5));      // ... codeAddress6
                Write(0x8D3353, jumpCode.ToArray());
            }

            int pbPtr = ReadInt(codeAddress - 0x14);
            int onlinePtr = ReadInt(codeAddress - 0x10);
            if (pbColour != 0) // 0 = use default colours
            {
                Write(codeAddress + 0x33, (byte)0x75); // code part 4: jmp -> jne
                Write(codeAddress - 0xC, pbColour); // Set colour for new ghosts
                SetGhostAppearance(pbPtr, pbColour, opacity); // Set appearance for existing ghosts
            }
            else
            {
                Write(codeAddress + 0x33, (byte)0xEB); // code part 4: jne -> jmp
                int defaultColour = GetDefaultGhostColour(pbPtr);
                if (defaultColour != 0)
                {
                    SetGhostAppearance(pbPtr, defaultColour, opacity);
                }
            }

            if (onlineColour != 0)
            {
                Write(codeAddress + 0x47, (byte)0x75); // code part 4: jmp -> jne
                Write(codeAddress - 0x8, onlineColour);
                SetGhostAppearance(onlinePtr, onlineColour, opacity);
            }
            else
            {
                Write(codeAddress + 0x47, (byte)0xEB); // code part 4: jne -> jmp
                int defaultColour = GetDefaultGhostColour(onlinePtr);
                if (defaultColour != 0)
                {
                    SetGhostAppearance(onlinePtr, defaultColour, opacity);
                }
            }

            Write(codeAddress - 0x4, opacity); // set opacity for new ghosts
        }

        static void SetGhostAppearance(int ghostPtr, int colour, float opacity)
        {
            if (ghostPtr == 0)
            {
                return;
            }
            int tmp1 = ReadInt(ghostPtr + 0x84);
            tmp1 = ReadInt(tmp1 + 0x5C);
            tmp1 = ReadInt(tmp1 + 0x24);
            tmp1 = ReadInt(tmp1 + 0x20);
            if (!readSuccess)
            {
                return;
            }
            int tmp2 = ReadInt(ghostPtr + 0x88);
            tmp2 = ReadInt(tmp2 + 0x5C);
            tmp2 = ReadInt(tmp2 + 0x24);
            tmp2 = ReadInt(tmp2 + 0x20);
            if (!readSuccess)
            {
                return;
            }
            // ghost exists, so update the appearance
            float blue = ((colour & 0x00FF0000) >> 16) * (25f / 255f);
            float green = ((colour & 0x0000FF00) >> 8) * (25f / 255f);
            float red = (colour & 0x000000FF) * (25f / 255f);
            Write(tmp1, red);
            Write(tmp2, red);
            Write(tmp1 + 0x4, green);
            Write(tmp2 + 0x4, green);
            Write(tmp1 + 0x8, blue);
            Write(tmp2 + 0x8, blue);
            Write(tmp1 + 0xC, opacity);
            Write(tmp2 + 0xC, opacity);
        }

        public static int GetDefaultGhostColour(int ghostPtr)
        {
            if (ghostPtr == 0)
            {
                return 0;
            }
            return ReadInt(ReadInt(ghostPtr + 0x80) + 0x20C);
        }

        static int ToRGBA(Color colour)
        {
            int argb = colour.R +
                       (colour.G << 8) +
                       (colour.B << 16) +
                       (colour.A << 24);
            return argb;
        }
    }
}
