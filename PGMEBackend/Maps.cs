// Awesome Map Editor
// A map editor for GBA Pokémon games.

// Copyright (C) 2015 Diegoisawesome

// This file is part of Awesome Map Editor.
// Awesome Map Editor is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// Awesome Map Editor is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with Awesome Map Editor. If not, see <http://www.gnu.org/licenses/>.

using Nintenlord.ROMHacking.GBA;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace PGMEBackend
{
    public class Map
    {
        public string name
        {
            get { return "[" + currentBank.ToString("X2") + ", " + currentMap.ToString("X2") + "] " + Program.mapNames[mapNameIndex]; }
        }

        public byte[] rawHeaderOrig;
        public byte[] rawHeader;
        public byte[] rawEntityHeader;

        public int offset;
        public byte currentBank;
        public byte currentMap;

        public int mapDataPointer;
        public int connectionsDataPointer;
        public short mapLayoutIndex;
        public byte mapNameIndex;
        public byte mapType;

        public MapLayout layout;

        public GBAROM originROM;

        public Map()
        {
        }

        public Map(int Offset, GBAROM ROM, int CurrentBank, int CurrentMap)
        {
            offset = Offset;
            originROM = ROM;
            currentBank = (byte)CurrentBank;
            currentMap = (byte)CurrentMap;
            rawHeaderOrig = originROM.GetData(offset, 0x1C);
            rawHeader = (byte[])rawHeaderOrig.Clone();

            LoadMapHeaderFromRaw();

            if (Program.mapLayouts.ContainsKey(mapLayoutIndex))
                layout = Program.mapLayouts[mapLayoutIndex];
            else
            {
                Program.mapLayoutNotFoundCount++;
                if (mapLayoutIndex > Program.maxLayout)
                {
                    Program.maxLayout = mapLayoutIndex;
                }
            }
        }

        public void LoadMapHeaderFromRaw()
        {
            mapDataPointer = BitConverter.ToInt32(rawHeader, 0x0) - 0x8000000;
            connectionsDataPointer = BitConverter.ToInt32(rawHeader, 0xC) - 0x8000000;
            mapLayoutIndex = BitConverter.ToInt16(rawHeader, 0x12);
            mapNameIndex = rawHeader[0x14];
            mapType = rawHeader[0x17];
        }
    }

    class MapBank
    {
        public Dictionary<int, Map> Bank;
        public MapBank()
        {
            Bank = new Dictionary<int, Map>();
        }

        public void AddMap(int number, Map map)
        {
            Bank.Add(number, map);
        }

        public Dictionary<int, Map> GetBank()
        {
            return Bank;
        }
    }

    public class MapLayout
    {
        public byte[] rawHeaderOrig;
        public byte[] rawHeader;

        public string name
        {
            get { return "[" + layoutIndex.ToString("X4") + "] Layout"; }
        }

        public short layoutIndex;

        public int layoutWidth;
        public int layoutHeight;

        public int borderBlocksPointer;
        public int mapDataPointer;
        public int globalTilesetPointer;
        public int localTilesetPointer;

        public byte borderWidth;
        public byte borderHeight;

        public byte buffer1;
        public byte buffer2;

        public MapTileset globalTileset;
        public MapTileset localTileset;

        public byte[] rawLayoutOrig;
        public byte[] rawLayout;
        public short[] layout;

        public byte[] rawBorderOrig;
        public byte[] rawBorder;
        public short[] border;

        public GBAROM originROM;

        public MapLayout()
        {
        }

        public MapLayout(int index, int offset, GBAROM ROM)
        {
            layoutIndex = (byte)index;
            originROM = ROM;
            if (Program.currentGame.RomType == "FRLG")
                rawHeaderOrig = originROM.GetData(offset, 0x1C);
            else
                rawHeaderOrig = originROM.GetData(offset, 0x18);
            rawHeader = (byte[])rawHeaderOrig.Clone();
            LoadLayoutHeaderFromRaw();

            if (borderBlocksPointer > 0 && borderBlocksPointer < Program.ROM.Length)
            {
                rawBorderOrig = originROM.GetData(borderBlocksPointer, borderWidth * borderHeight * 4);
                rawBorder = (byte[])rawBorderOrig.Clone();
                LoadBorderFromRaw();
            }

            if (mapDataPointer > 0 && mapDataPointer < Program.ROM.Length)
            {
                rawLayoutOrig = originROM.GetData(mapDataPointer, layoutWidth * layoutHeight * 4);
                rawLayout = (byte[])rawLayoutOrig.Clone();
                LoadLayoutFromRaw();
            }
        }

        public void LoadLayoutFromRaw()
        {
            layout = new short[rawLayout.Length / 2];
            Buffer.BlockCopy(rawLayout, 0, layout, 0, rawLayout.Length);
        }

        public void LoadBorderFromRaw()
        {
            border = new short[rawBorder.Length / 2];
            Buffer.BlockCopy(rawBorder, 0, border, 0, rawBorder.Length);
        }

        public void LoadLayoutHeaderFromRaw()
        {
            layoutWidth = BitConverter.ToInt32(rawHeader, 0);
            layoutHeight = BitConverter.ToInt32(rawHeader, 4);
            borderBlocksPointer = BitConverter.ToInt32(rawHeader, 0x8) - 0x8000000;
            mapDataPointer = BitConverter.ToInt32(rawHeader, 0xC) - 0x8000000;
            globalTilesetPointer = BitConverter.ToInt32(rawHeader, 0x10) - 0x8000000;
            localTilesetPointer = BitConverter.ToInt32(rawHeader, 0x14) - 0x8000000;
            if (Program.currentGame.RomType == "FRLG")
            {
                borderWidth = (byte)((rawHeader[0x18] <= 8) ? rawHeader[0x18] : 8);
                borderHeight = (byte)((rawHeader[0x19] <= 8) ? rawHeader[0x19] : 8);
                buffer1 = rawHeader[0x1A];
                buffer2 = rawHeader[0x1B];
            }
            else
            {
                borderWidth = 2;
                borderHeight = 2;
                buffer1 = 0;
                buffer2 = 0;
            }

            if (globalTilesetPointer != -0x8000000)
            {
                if (!Program.mapTilesets.ContainsKey(globalTilesetPointer))
                    Program.mapTilesets.Add(globalTilesetPointer, new MapTileset(globalTilesetPointer, originROM));
                globalTileset = Program.mapTilesets[globalTilesetPointer];
            }

            if (localTilesetPointer != -0x8000000)
            {
                if (!Program.mapTilesets.ContainsKey(localTilesetPointer))
                    Program.mapTilesets.Add(localTilesetPointer, new MapTileset(localTilesetPointer, originROM));
                localTileset = Program.mapTilesets[localTilesetPointer];
            }
        }

        public class VisualMapTile {
            public FrameBuffer buffer;
            public int xpos = 0;
            public int ypos = 0;
            public int Width = 64;
            public int Height = 64;
            public bool Redraw = true;
        }

        public void Unload()
        {
            if (drawTiles != null)
            {
                foreach (var v in drawTiles)
                    v.buffer.Dispose();
                drawTiles.Clear();
            }
        }

        public List<VisualMapTile> drawTiles;

        // TODO: Refactor to utilize FBO thing
        public void RefreshChunks(Spritesheet[] globalSheets, Spritesheet[] localSheets, int xPos, int yPos, double scale)
        {
            if (drawTiles == null || drawTiles.Count == 0)
            {
                drawTiles = new List<VisualMapTile>();
                int tilesx = Math.Max(layoutWidth / 16, 1);
                int tilesy = Math.Max(layoutHeight / 16, 1);
                tilesx += layoutWidth % tilesx;
                tilesy += layoutWidth % tilesy;

                int tileWidth = layoutWidth / tilesx;
                int tileHeight = layoutHeight / tilesy;
                int xtiles = layoutWidth / tileWidth;
                int ytiles = layoutHeight / tileHeight;
                for (int y = 0; y < ytiles; y++)
                {
                    for (int x = 0; x < xtiles; x++)
                    {
                        var tile = new VisualMapTile();
                        tile.Width = tileWidth;
                        tile.Height = tileHeight;
                        tile.buffer = new FrameBuffer(tileWidth * 16, tileHeight * 16);
                        tile.xpos = x * tileWidth;
                        tile.ypos = y * tileHeight;
                        drawTiles.Add(tile);
                    }
                }

                int xremain = layoutWidth - xtiles * tileWidth;
                int yremain = layoutHeight - ytiles * tileHeight;
                if (xremain > 0)
                    for (int y = 0; y < ytiles; y++)
                    {
                        var tile = new VisualMapTile();
                        tile.Width = xremain;
                        tile.Height = tileHeight;
                        tile.buffer = new FrameBuffer(tile.Width * 16, tile.Height * 16);
                        tile.xpos = xtiles * tileWidth;
                        tile.ypos = y * tileHeight;
                        drawTiles.Add(tile);
                    }

                if (yremain > 0)
                    for (int x = 0; x < xtiles; x++)
                    {
                        var tile = new VisualMapTile();
                        tile.Width = tileWidth;
                        tile.Height = yremain;
                        tile.buffer = new FrameBuffer(tile.Width * 16, tile.Height * 16);
                        tile.xpos = x * tileWidth;
                        tile.ypos = ytiles * tileHeight;
                        drawTiles.Add(tile);
                    }

                if ((xremain > 0) && (yremain > 0))
                {
                    var tile = new VisualMapTile();
                    tile.Width = xremain;
                    tile.Height = yremain;
                    tile.buffer = new FrameBuffer(tile.Width * 16, tile.Height * 16);
                    tile.xpos = xtiles * tileWidth;
                    tile.ypos = ytiles * tileHeight;
                    drawTiles.Add(tile);
                }
            }

            foreach (var v in drawTiles)
            {
                if (v.Redraw)
                {
                    FrameBuffer.Active = v.buffer;
                    GL.PushMatrix();
                    {
                        int xoff = v.xpos * 16;
                        int yoff = v.ypos * 16;
                        for (int i = v.ypos; i < v.ypos + v.Height; i++)
                        {
                            for (int j = v.xpos; j < v.xpos + v.Width; j++)
                            {
                                short block = layout[i * layoutWidth + j];
                                short blockIndex = (short)(block & 0x3FF);
                                byte movementPerm = (byte)((block & 0xFC00) >> 10);

                                if (blockIndex < Program.currentGame.MainTSBlocks)
                                {
                                    if (globalTileset != null)
                                        globalTileset.blockSet.blocks[blockIndex].Draw(globalSheets, localSheets, xPos + j * 16 - xoff, yPos + i * 16 - yoff, scale);
                                    else
                                        Surface.DrawRect(xPos - xoff, yPos - yoff, 16, 16, Color.Black);
                                }
                                else
                                {
                                    if (localTileset != null)
                                        localTileset.blockSet.blocks[blockIndex - Program.currentGame.MainTSBlocks].Draw(globalSheets, localSheets, xPos + j * 16 - xoff, yPos + i * 16 - yoff, scale);
                                    else
                                        Surface.DrawRect(xPos - xoff, yPos - yoff, 16, 16, Color.Black);
                                }
                            }
                        }

                        GL.PopMatrix();
                    }
                    FrameBuffer.Active = null;
                    v.Redraw = false;
                }
            }
        }

        public void Draw(Spritesheet[] globalSheets, Spritesheet[] localSheets, int xPos, int yPos, double scale)
        {
            GL.Disable(EnableCap.Blend);
            foreach (var v in drawTiles)
            {
                Surface.SetColor(Color.White);
                Surface.SetTexture(v.buffer.ColorTexture);
                Surface.DrawRect(v.xpos * 16, v.ypos * 16, v.buffer.Width, v.buffer.Height);
            }

            GL.Enable(EnableCap.Blend);

            Surface.SetTexture(null);
        }
        
        public void DrawBorder(Spritesheet[] globalSheets, Spritesheet[] localSheets, int xPos, int yPos, double scale)
        {
            for (int i = 0; i < borderHeight; i++)
            {
                for (int j = 0; j < borderWidth; j++)
                {
                    short block = border[i * borderWidth + j];
                    short blockIndex = (short)(block & 0x3FF);

                    if (blockIndex < Program.currentGame.MainTSBlocks)
                    {
                        if (globalTileset != null)
                            globalTileset.blockSet.blocks[blockIndex].Draw(globalSheets, localSheets, xPos + j * 16, yPos + i * 16, scale);
                        else
                            Surface.DrawRect(xPos, yPos, 16, 16, Color.Black);
                    }
                    else
                    {
                        if (localTileset != null)
                            localTileset.blockSet.blocks[blockIndex - Program.currentGame.MainTSBlocks].Draw(globalSheets, localSheets, xPos + j * 16, yPos + i * 16, scale);
                        else
                            Surface.DrawRect(xPos, yPos, 16, 16, Color.Black);
                    }
                }
            }

            Surface.SetTexture(null);
        }
    }
}
