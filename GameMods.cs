﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using static MemoryHelper;

namespace GhostSelector
{
    static class GameMods
    {
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

        static readonly int addressHideGhostCar = 0x8D2A0C;
        static readonly int addressHidePBGhost = 0x899D71;

        public static bool Initialise()
        {
            Process[] processList = Process.GetProcessesByName("ASN_App_PcDx9_Final");
            return processList.Length > 0 && MemoryHelper.Initialise(processList[0].Id);
        }

        public static void LoadSettings()
        {
            Write(addressSetLeaderboardRange, setLeaderboardRange);
            Write(addressChangeLeaderboardDownload1, changeLeaderboardDownload1);
            Write(addressChangeLeaderboardDownload2, changeLeaderboardDownload2);
            Write(addressSelectFirstDownloadedEntry, selectFirstDownloadedEntry);

            if (Program.Config.FastestPlayerSelector.Enabled)
            {
                // Disable fixed position
                Write(addressFixLeaderboardPosition, fixLeaderboardPosition_disable);

                // Load players into memory
                int playerCount = 0;
                foreach (PlayerElement Player in Program.Config.FastestPlayerSelector.Players)
                {
                    if (Player.Enabled)
                    {
                        Write(addressPlayerListStart + playerCount * 8, Player.SteamId);
                        playerCount++;
                    }
                }
                Write(addressChangeLeaderboardDownload1 + offsetPlayerCount, (byte)playerCount);
            }

            else
            {
                // Disable player selector
                Write(addressPlayerListStart, steamId);
                Write(addressChangeLeaderboardDownload1 + offsetPlayerCount, (byte)0x01);

                // Fix the position
                Write(addressFixLeaderboardPosition, fixLeaderboardPosition);
                if (Program.Config.PositionSelector.SelectedPosition > 0)
                {
                    Write(addressFixLeaderboardPosition + offsetPosition, Program.Config.PositionSelector.SelectedPosition);
                }
                else
                {
                    Write(addressFixLeaderboardPosition + offsetPosition, 0xFFFFFFF);
                }
            }

            if (Program.Config.Graphics.HideGhostCars)
            {
                Write(addressHideGhostCar, hideGhostCar);
            }
            else
            {
                Write(addressHideGhostCar, hideGhostCar_disable);
            }

            if (Program.Config.Graphics.HidePBGhost)
            {
                Write(addressHidePBGhost, hidePBGhost);
            }
            else
            {
                Write(addressHidePBGhost, hidePBGhost_disable);
            }

            SetNameTagOpacity(Program.Config.Graphics.NameTagOpacity);
            SetGhostAppearance(0xFF00FF00, 0xFF0000FF, 1.0f);
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

        public static void SetGhostAppearance(uint pbColour, uint onlineColour, float opacity)
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

                // [codeAddress - 20] = PB ghost identifier
                // [codeAddress - 16] = online ghost identifier
                // [codeAddress - 12] = PB ghost colour
                // [codeAddress - 8]  = online ghost colour
                // [codeAddress - 4]  = ghost opacity

                // Code part 1 - save PB ghost identifier #1
                int codeAddress1 = codeAddress;
                myCode.AddRange(new byte[] { 0x8D, 0x8D, 0x90, 0x4B, 0x00, 0x00 });         // lea ecx, [ebp+4B90]
                myCode.AddRange(new byte[] { 0x89, 0x0D });                                 // mov dword [...], ecx
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 20));                   // ... codeAddress - 16
                myCode.Add(0xC3);                                                           // ret

                // Code part 2 - save online ghost identifier
                int codeAddress2 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0x8D, 0x8D, 0xE0, 0xC7, 0x01, 0x00 });         // lea ecx, [ebp+1C7E0]
                myCode.AddRange(new byte[] { 0x89, 0x0D });                                 // mov dword [...], ecx
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 16));                   // ... codeAddress - 12
                myCode.Add(0xC3);                                                           // ret

                // Code part 3 - save PB ghost identifier #2
                int codeAddress3 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0x81, 0xC1, 0x90, 0x4B, 0x00, 0x00 });         // add ecx, 4B90
                myCode.AddRange(new byte[] { 0x89, 0x0D });                                 // mov dword [...], ecx
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 20));                   // ... codeAddress - 16
                myCode.Add(0xC3);                                                           // ret

                // Code part 4 - backup original colour, replace it
                int codeAddress4 = codeAddress + myCode.Count;
                myCode.AddRange(new byte[] { 0x8B, 0x98, 0x0C, 0x02, 0x00, 0x00 });         // mov ebx, dword [eax+0x20c]
                myCode.AddRange(new byte[] { 0x39, 0x35 });                                 // cmp dword [...], esi
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 20));                   // ... codeAddress - 20
                myCode.AddRange(new byte[] { 0x75, 0xC });                                  // jne (next instruction) + 12
                myCode.AddRange(new byte[] { 0x8b, 0x15 });                                 // mov edx, dword [...]
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 12));                    // ... codeAddress - 12
                myCode.AddRange(new byte[] { 0x89, 0x90, 0x0C, 0x02, 0x00, 0x00 });         // mov dword [eax+0x20c], edx
                myCode.AddRange(new byte[] { 0x39, 0x35 });                                 // cmp dword [...], esi
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 16));                   // ... = codeAddress - 16
                myCode.AddRange(new byte[] { 0x75, 0xC });                                  // jne (next instruction) + 12
                myCode.AddRange(new byte[] { 0x8b, 0x15 });                                 // mov edx, dword [...]
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 8));                    // ... codeAddress - 8
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
                myCode.AddRange(BitConverter.GetBytes(codeAddress - 4));                    // ... codeAddress - 4
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
            // Set the appearance
            Write(codeAddress - 12, pbColour);
            Write(codeAddress - 8, onlineColour);
            Write(codeAddress - 4, opacity);
        }
    }
}