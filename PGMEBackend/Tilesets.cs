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
using Nintenlord.ROMHacking.GBA.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGMEBackend
{
    public class MapTileset
    {
        public int offset;

        public int isCompressed;
        public int isSecondary;
        public int buffer1;
        public int buffer2;

        public int imagePointer;
        public int imagePalsPointer;
        public int blocksPointer;

        public Bitmap[] image;
        public Spritesheet[] tileSheets;
        
        public Blockset blockSet;

        public GBAROM originROM;

        public MapTileset(int Offset, GBAROM ROM)
        {
            offset = Offset;
            originROM = ROM;
            byte[] temp = originROM.GetData(offset, 0x4);
            isCompressed = temp[0];
            isSecondary = temp[1];
            buffer1 = temp[2];
            buffer2 = temp[3];
            imagePointer = originROM.ReadPointer(offset + 0x4);
            imagePalsPointer = originROM.ReadPointer(offset + 0x8);
            blocksPointer = originROM.ReadPointer(offset + 0xC);

            blockSet = new Blockset(blocksPointer, (isSecondary & 1) == 1, originROM);
        }

        public void Initialize(MapLayout mapLayout)
        {
            image = new Bitmap[16];

            int width = 128;
            int height = Program.currentGame.MainTSSize * 8;
            int palSpace = 6;
            if (Program.currentGame.RomType == "FRLG")
                palSpace = 7;
            if ((isSecondary & 1) == 0)
                height = Program.currentGame.LocalTSSize * 8;

            if (mapLayout.globalTileset != null)
                for (int i = 0; i < palSpace; i++)
                    image[i] = Graphics.ImageManipulator.imageLoader(Program.ROM, imagePointer, mapLayout.globalTileset.imagePalsPointer + i * 32, width, height, (isCompressed & 1) == 1, false, true);

            if (mapLayout.localTileset != null)
                for (int i = palSpace; i < 13; i++)
                    image[i] = Graphics.ImageManipulator.imageLoader(Program.ROM, imagePointer, mapLayout.localTileset.imagePalsPointer + i * 32, width, height, (isCompressed & 1) == 1, false, true);

            for (int i = 13; i < 16; i++)
                image[i] = Graphics.ImageManipulator.imageLoader(Program.ROM, imagePointer, 0, width, height, (isCompressed & 1) == 1, false, true);

            tileSheets = new Spritesheet[16];
            for (int i = 0; i < 16; i++)
                if(image[i] != null)
                    tileSheets[i] = Spritesheet.Load(image[i], 8, 8);
        }
    }

    public class Blockset
    {
        public Block[] blocks;
        public GBAROM originROM;

        public Blockset(int blocksPointer,  bool isSecondary, GBAROM ROM)
        {
            originROM = ROM;

            if(!isSecondary)
                blocks = new Block[Program.currentGame.MainTSBlocks];
            else
                blocks = new Block[0x400 - Program.currentGame.MainTSBlocks];  //this needs fixing

            for (int i = 0; i < blocks.Length; i++)
            {
                int isTriple = 0;

                byte[] rawTileData = originROM.GetData(blocksPointer + (i * 16) + ((isTriple == 2) ? 8 : 0), ((isTriple == 0) ? 16 : 24));

                short[] tileData = new short[(int)Math.Ceiling((double)(rawTileData.Length / 2))];
                Buffer.BlockCopy(rawTileData, 0, tileData, 0, rawTileData.Length);

                Tile[] tiles = new Tile[(isTriple == 0) ? 8 : 12];
                for (int j = 0; j < tiles.Length; j++)
                    tiles[j] = new Tile(tileData[j] & 0x3FF, (tileData[j] & 0xF000) >> 12, (tileData[j] & 0x400) == 0x400, (tileData[j] & 0x800) == 0x800);
                
                blocks[i] = new Block(tiles);
            }
        }

        public void Draw(Spritesheet[] globalSheets, Spritesheet[] localSheets, int xPos, int yPos, double scale)
        {
            for (int i = 0; i < blocks.Length / 8; i++)
                for (int j = 0; j < 8; j++)
                    blocks[j + (i * 8)].Draw(globalSheets, localSheets, xPos + j * 16, yPos + i * 16, scale);
        }
    }

    public class Block
    {
        public Tile[] tileArray;

        public Block(Tile[] tiles)
        {
            tileArray = tiles;
        }

        public void Draw(Spritesheet[] globalSheets, Spritesheet[] localSheets, int xPos, int yPos, double scale)
        {
            GL.Disable(EnableCap.Blend);
            for (int i = 0; i < 4; i++)
            {
                tileArray[i].Draw(globalSheets, localSheets, xPos + ((i % 2) * 8), yPos + (((i % 4) / 2) * 8), scale);
            }
            GL.Enable(EnableCap.Blend);
            for (int i = 4; i < tileArray.Length; i++)
            {
                tileArray[i].Draw(globalSheets, localSheets, xPos + ((i % 2) * 8), yPos + (((i % 4) / 2) * 8), scale);
            }
        }
    }

    public class Tile
    {
        public int index;
        public int palette;
        public bool xFlip;
        public bool yFlip;

        public Tile(int ind, int pal, bool x, bool y)
        {
            index = ind;
            palette = pal;
            xFlip = x;
            yFlip = y;
        }

        public void Draw(Spritesheet[] globalSheets, Spritesheet[] localSheets, int xPos, int yPos, double scale)
        {
            if (index < Program.currentGame.MainTSSize)
            {
                if (globalSheets != null && globalSheets[palette] != null)
                    globalSheets[palette].Draw(index, xPos, yPos, xFlip, yFlip, scale);
                else
                    Surface.DrawRect(xPos, yPos, 8, 8, Color.Black);
            }
            else
            {
                if (localSheets != null && localSheets[palette] != null)
                    localSheets[palette].Draw(index - Program.currentGame.MainTSSize, xPos, yPos, xFlip, yFlip, scale);
                else
                    Surface.DrawRect(xPos, yPos, 8, 8, Color.Black);
            }
        }
    }
}
