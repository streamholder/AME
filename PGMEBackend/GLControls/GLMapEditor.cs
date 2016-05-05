﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace PGMEBackend.GLControls
{
    class GLMapEditor
    {
        Color rectColor;

        public int width = 0;
        public int height = 0;

        public int mouseX = -1;
        public int mouseY = -1;
        public int endMouseX = -1;
        public int endMouseY = -1;

        public Color rectDefaultColor = Color.FromArgb(0, 255, 0);
        public Color rectPaintColor = Color.FromArgb(255, 0, 0);
        public Color rectSelectColor = Color.FromArgb(255, 255, 0);

        public GLMapEditor(int w, int h)
        {
            width = w;
            height = h;
            GL.ClearColor(Color.Transparent);
            SetupViewport();
            rectColor = rectDefaultColor;
        }

        public static implicit operator bool (GLMapEditor b)
        {
            return b != null;
        }

        private void SetupViewport()
        {
            GL.Viewport(0, 0, width, height); // Use all of the glControl painting area
        }

        public void Paint(int w, int h)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            PreRender();

            width = w;
            height = h;
            SetupViewport();

            GL.ClearColor(Color.Transparent);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var proj = OpenTK.Matrix4.CreateOrthographicOffCenter(0, width, height, 0, -1, 1);
            GL.LoadMatrix(ref proj);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            Render();

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }

        private void PreRender()
        {
            var layout = Program.currentLayout;
            if (layout != null)
            {
                layout.RefreshChunks((Program.currentLayout.globalTileset != null) ? Program.currentLayout.globalTileset.tileSheets : null,
                            (Program.currentLayout.localTileset != null) ? Program.currentLayout.localTileset.tileSheets : null, 0, 0, 1);
            }
        }

        private void Render()
        {
            MapLayout layout = Program.currentLayout;
            if (layout != null)
            {
                layout.Draw((layout.globalTileset != null) ? layout.globalTileset.tileSheets : null,
                            (layout.localTileset != null) ? layout.localTileset.tileSheets : null, 0, 0, 1);

                if (mouseX != -1 && mouseY != -1)
                {
                    int x = mouseX * 16;
                    int y = mouseY * 16;
                    int endX = endMouseX * 16;
                    int endY = endMouseY * 16;

                    if (endMouseX >= width / 16)
                        endX = ((width - 1) / 16) * 16;
                    if (endMouseY >= height / 16)
                        endY = ((height - 1) / 16) * 16;

                    int w = x - endX;
                    int h = y - endY;
                    
                    Surface.DrawOutlineRect(endX + (w < 0 ? 16 : 0), endY + (h < 0 ? 16 : 0), w + (w >= 0 ? 16 : -16), h + (h >= 0 ? 16 : -16), rectColor);
                }
            }
        }
        
        public void MouseMove(int x, int y)
        {
            int oldMouseX = mouseX;
            int oldMouseY = mouseY;

            mouseX = x / 16;
            mouseY = y / 16;

            if (mouseX >= width / 16)
                mouseX = (width - 1) / 16;
            if (mouseY >= height / 16)
                mouseY = (height - 1) / 16;

            if (mouseX < 0)
                mouseX = 0;
            if (mouseY < 0)
                mouseY = 0;

            if (mouseX == oldMouseX && mouseY == oldMouseY)
                return;
        }

        public void MouseLeave()
        {
            mouseX = -1;
            mouseY = -1;
            endMouseX = -1;
            endMouseY = -1;
        }

        public void PaintBlocksToMap(short[] blockArray, int x, int y, int w, int h)
        {
            bool found = false;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((x + j < Program.currentLayout.layoutWidth) && (y + i < Program.currentLayout.layoutHeight))
                    {
                        if (Program.currentLayout.layout[(x + (y * Program.currentLayout.layoutWidth)) + (i * Program.currentLayout.layoutWidth) + j] != blockArray[(i * w) + j])
                            found = true;
                    }
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            if (found)
            {
                Console.WriteLine("Painting of " + w * h + " blocks...");
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if ((x + j < Program.currentLayout.layoutWidth) && (y + i < Program.currentLayout.layoutHeight))
                        {
                            Console.WriteLine("Drawing at (" + (x + j) + ", " + (y + i) + "): " + blockArray[i * w + j]);
                            if (blockArray.Length == 1)
                                Program.currentLayout.layout[(x + (y * Program.currentLayout.layoutWidth)) + (i * Program.currentLayout.layoutWidth) + j] =
                                    (short)((blockArray[0] & 0x3FF) + (Program.currentLayout.layout[(x + (y * Program.currentLayout.layoutWidth)) + (i * Program.currentLayout.layoutWidth) + j] & 0xFC00));
                            else
                                Program.currentLayout.layout[(x + (y * Program.currentLayout.layoutWidth)) + (i * Program.currentLayout.layoutWidth) + j] =
                                    blockArray[(i * w) + j];
                        }
                    }
                }

                for (int j = y; j <= y + h; j++)
                {
                    for (int i = x; i <= x + w; i++)
                    {
                        foreach (var v in Program.currentLayout.drawTiles)
                        {
                            if (v.Redraw)
                                continue;
                            if (i < v.xpos + v.Width && i >= v.xpos && j >= v.ypos && j < v.ypos + v.Height)
                            {
                                v.Redraw = true;
                                Console.WriteLine("Redrawing " + v.buffer.FBOHandle);
                                continue;
                            }
                        }
                    }
                }
            }
        }

        public void PaintPermsToMap(byte[] permsArray, int x, int y, int w, int h)
        {
            bool found = false;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((x + j < Program.currentLayout.layoutWidth) && (y + i < Program.currentLayout.layoutHeight))
                    {
                        if (Program.currentLayout.layout[(x + (y * Program.currentLayout.layoutWidth)) + (i * Program.currentLayout.layoutWidth) + j] != permsArray[(i * w) + j])
                            found = true;
                    }
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            if (found)
            {
                Console.WriteLine("Painting of " + w * h + " blocks...");
                for (int i = 0; i < h; i++)
                {
                    for (int j = 0; j < w; j++)
                    {
                        if ((x + j < Program.currentLayout.layoutWidth) && (y + i < Program.currentLayout.layoutHeight))
                        {
                            Console.WriteLine("Drawing at (" + (x + j) + ", " + (y + i) + "): " + permsArray[i * w + j]);
                            Program.currentLayout.layout[(x + (y * Program.currentLayout.layoutWidth)) + (i * Program.currentLayout.layoutWidth) + j] =
                                (short)((Program.currentLayout.layout[(x + (y * Program.currentLayout.layoutWidth)) + (i * Program.currentLayout.layoutWidth) + j] & 0x3FF) + (permsArray[(i * w) + j] << 10));
                        }
                    }
                }

                for (int j = y; j <= y + h; j++)
                {
                    for (int i = x; i <= x + w; i++)
                    {
                        foreach (var v in Program.currentLayout.drawTiles)
                        {
                            if (v.Redraw)
                                continue;
                            if (i < v.xpos + v.Width && i >= v.xpos && j >= v.ypos && j < v.ypos + v.Height)
                            {
                                v.Redraw = true;
                                Console.WriteLine("Redrawing " + v.buffer.FBOHandle);
                                continue;
                            }
                        }
                    }
                }
            }
        }

        public void RedrawAllChunks()
        {
            foreach (var v in Program.currentLayout.drawTiles)
            {
                v.Redraw = true;
                Console.WriteLine("Redrawing " + v.buffer.FBOHandle);
            }
        }

        public void FillBlocks(int x, int y, short originalBlock, short newBlock)
        {
            if (x >= 0 && x < Program.currentLayout.layoutWidth &&
                y >= 0 && y < Program.currentLayout.layoutHeight &&
                (short)(Program.currentLayout.layout[x + (y * Program.currentLayout.layoutWidth)] & 0x3FF) == originalBlock)
            {
                Program.currentLayout.layout[x + (y * Program.currentLayout.layoutWidth)] = (short)(newBlock + (Program.currentLayout.layout[x + (y * Program.currentLayout.layoutWidth)] & 0xFC00));
                FillBlocks(x, y + 1, originalBlock, newBlock);
                FillBlocks(x, y - 1, originalBlock, newBlock);
                FillBlocks(x - 1, y, originalBlock, newBlock);
                FillBlocks(x + 1, y, originalBlock, newBlock);
            }
        }

        public void FillPerms(int x, int y, byte originalPerm, byte newPerm)
        {
            if (x >= 0 && x < Program.currentLayout.layoutWidth &&
                y >= 0 && y < Program.currentLayout.layoutHeight &&
                (byte)((Program.currentLayout.layout[x + (y * Program.currentLayout.layoutWidth)] & 0xFC00) >> 10) == originalPerm)
            {
                Program.currentLayout.layout[x + (y * Program.currentLayout.layoutWidth)] = (short)((Program.currentLayout.layout[x + (y * Program.currentLayout.layoutWidth)] & 0x3FF) + (newPerm << 10));
                FillPerms(x, y + 1, originalPerm, newPerm);
                FillPerms(x, y - 1, originalPerm, newPerm);
                FillPerms(x - 1, y, originalPerm, newPerm);
                FillPerms(x + 1, y, originalPerm, newPerm);
            }
        }
    }
}
