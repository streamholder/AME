using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PGMEBackend.GLControls
{
    class GLBlockChooser
    {
        Color rectColor;

        public int width = 0;
        public int height = 0;

        public int mouseX = -1;
        public int mouseY = -1;
        public int endMouseX = -1;
        public int endMouseY = -1;
        public int selectX = 0;
        public int selectY = 0;
        public int selectWidth = 1;
        public int selectHeight = 1;
        public int editorSelectWidth = 1;
        public int editorSelectHeight = 1;
        public short[] selectArray = { 0 };

        public Color rectDefaultColor = Color.FromArgb(0, 255, 0);
        public Color rectPaintColor = Color.FromArgb(255, 0, 0);
        public Color rectSelectColor = Color.FromArgb(255, 255, 0);
        
        public GLBlockChooser(int w, int h)
        {
            width = w;
            height = h;
            GL.ClearColor(Color.Transparent);
            SetupViewport();
            rectColor = rectPaintColor;
        }

        public static implicit operator bool (GLBlockChooser b)
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
                MapTileset globalTileset = Program.currentLayout.globalTileset;
                MapTileset localTileset = Program.currentLayout.localTileset;
                if (globalTileset != null && globalTileset.blockSet != null)
                    globalTileset.blockSet.Draw((globalTileset != null) ? globalTileset.tileSheets : null, (localTileset != null) ? localTileset.tileSheets : null, 0, 0, 1);

                if (localTileset != null && localTileset.blockSet != null)
                    localTileset.blockSet.Draw((globalTileset != null) ? globalTileset.tileSheets : null, (localTileset != null) ? localTileset.tileSheets : null, 0, height, 1);
                    
                int x = selectX * 16;
                int y = selectY * 16;
                int w = selectWidth * 16;
                int h = selectHeight * 16;

                if (selectX + selectWidth > width / 16)
                    w = ((width - 1) / 16 - selectX) * 16;
                if (selectY + selectHeight > height / 16)
                    h = ((height - 1) / 16 - selectY) * 16;

                Surface.DrawOutlineRect(x + (w < 0 ? 16 : 0), y + (h < 0 ? 16 : 0), w + (w >= 0 ? 0 : -16), h + (h >= 0 ? 0 : -16), rectColor);

                Surface.DrawOutlineRect(mouseX * 16, mouseY * 16, 16, 16, rectDefaultColor);
            }
        }
    }
}
