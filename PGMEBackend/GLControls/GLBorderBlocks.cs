using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PGMEBackend.GLControls
{
    class GLBorderBlocks
    {
        Color rectColor;

        int width = 0;
        int height = 0;

        public int mouseX = -1;
        public int mouseY = -1;
        public int endMouseX = -1;
        public int endMouseY = -1;

        public Color rectDefaultColor = Color.FromArgb(0, 255, 0);
        public Color rectPaintColor = Color.FromArgb(255, 0, 0);
        public Color rectSelectColor = Color.FromArgb(255, 255, 0);

        public GLBorderBlocks(int w, int h)
        {
            width = w;
            height = h;
            GL.ClearColor(Color.Transparent);
            SetupViewport();
            rectColor = rectDefaultColor;
        }

        public static implicit operator bool (GLBorderBlocks b)
        {
            return b != null;
        }
        
        private void SetupViewport()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, width, height, 0, -1, 1); // Top left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, width, height); // Use all of the glControl painting area
        }

        public void Paint(int w, int h)
        {
            width = w;
            height = h;
            SetupViewport();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            Render();

            GL.Disable(EnableCap.Texture2D);
            GL.Disable(EnableCap.Blend);
        }
        
        private void Render()
        {
            MapLayout layout = Program.currentLayout;
            if (layout != null)
            {
                width = layout.borderWidth * 16;
                height = layout.borderHeight * 16;
                
                layout.DrawBorder((Program.currentLayout.globalTileset != null) ? Program.currentLayout.globalTileset.tileSheets : null,
                            (Program.currentLayout.localTileset != null) ? Program.currentLayout.localTileset.tileSheets : null, 0, 0, 1);
                /*
                if(Config.settings.ShowGrid)
                {
                    Surface.SetColor(Color.Black);
                    for(int i = 0; i < layout.layoutWidth; i++)
                        Surface.DrawLine(new double[] { i * 16, 0, i * 16, height });
                    for (int i = 0; i < layout.layoutHeight; i++)
                        Surface.DrawLine(new double[] { 0, i * 16, width, i * 16 });
                }*/

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

            Program.glBlockChooser.editorSelectWidth = Math.Abs(Program.glBlockChooser.editorSelectWidth);
            Program.glBlockChooser.editorSelectHeight = Math.Abs(Program.glBlockChooser.editorSelectHeight);
            endMouseX = mouseX + Program.glBlockChooser.editorSelectWidth - 1;
            endMouseY = mouseY + Program.glBlockChooser.editorSelectHeight - 1;
        }

        public void MouseLeave()
        {
            mouseX = -1;
            mouseY = -1;
            endMouseX = -1;
            endMouseY = -1;
        }

        [Obsolete("Now handled by undo code")]
        void Paint()
        {
            // insert painting code
            for (int j = mouseY; j <= mouseY + Program.glBlockChooser.editorSelectHeight; j++)
            {
                for (int i = mouseX; i <= mouseX + Program.glBlockChooser.editorSelectWidth; i++)
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

        short[] oldBorder;

        public void PaintBlocksToBorder(short[] blockArray, int x, int y, int w, int h)
        {
            bool found = false;
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if ((x + j < Program.currentLayout.borderWidth) && (y + i < Program.currentLayout.borderHeight))
                    {
                        if (Program.currentLayout.border[(x + (y * Program.currentLayout.borderWidth)) + (i * Program.currentLayout.borderWidth) + j] != blockArray[(i * w) + j])
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
                        if ((x + j < Program.currentLayout.borderWidth) && (y + i < Program.currentLayout.borderHeight))
                        {
                            Console.WriteLine("Drawing at (" + (x + j) + ", " + (y + i) + "): " + blockArray[i * w + j]);
                            Program.currentLayout.border[(x + (y * Program.currentLayout.borderWidth)) + (i * Program.currentLayout.borderWidth) + j] = blockArray[(i * w) + j];
                        }
                    }
                }
            }
        }

        public void FillBlocks(int x, int y, short originalBlock, short newBlock)
        {
            if (x >= 0 && x < Program.currentLayout.borderWidth &&
                y >= 0 && y < Program.currentLayout.borderHeight &&
                Program.currentLayout.border[x + (y * Program.currentLayout.borderWidth)] == originalBlock)
            {
                Program.currentLayout.border[x + (y * Program.currentLayout.borderWidth)] = newBlock;
                FillBlocks(x, y + 1, originalBlock, newBlock);
                FillBlocks(x, y - 1, originalBlock, newBlock);
                FillBlocks(x - 1, y, originalBlock, newBlock);
                FillBlocks(x + 1, y, originalBlock, newBlock);
            }
        }
    }
}
