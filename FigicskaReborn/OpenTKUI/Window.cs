using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;

namespace FigicskaReborn
{
    public class AppWindow : GameWindow
    {
        int texture;
        static int blockSize = 25;
        float x, y;

        Dictionary<ConsoleKey, PlayerControlEnumeration> oControls = new Dictionary<ConsoleKey, PlayerControlEnumeration>();
        Dictionary<ConsoleKey, PlayerControlEnumeration> xControls = new Dictionary<ConsoleKey, PlayerControlEnumeration>();

        Dictionary<Key, Tuple<Player, PlayerControlEnumeration>> Controls = new Dictionary<Key, Tuple<Player, PlayerControlEnumeration>>();

        private TwoPlayerGame thisGame;

        public AppWindow()
        {

            // Set the borders of the field
            Field.LeftBorder = 0;
            Field.RightBorder = 79;
            Field.TopBorder = 2;
            Field.BottomBorder = 24;

            // Graphic magic
            Width = Field.Width * blockSize;
            Height = Field.Height * blockSize;
            this.Title = "Figicska Graphics!";
            this.WindowBorder = WindowBorder.Fixed;
            this.ClientSize = new Size(Width, Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color.CornflowerBlue);
            GL.Ortho(0, Width, Height, 0, -1, 1);
            GL.Viewport(0, 0, Width, Height);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out this.texture);
            GL.BindTexture(TextureTarget.Texture2D, this.texture);

            Bitmap bitmap = new Bitmap("x.png");
            bitmap.MakeTransparent(Color.Magenta);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            bitmap.Dispose();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            this.x = 40.0f;
            this.y = 40.0f;

        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            float dx = 0.0f;
            float dy = 0.0f;

            Direction moveDirection = Direction.Nowhere;

            if (this.Keyboard[Key.Left])
                dx = -1.0f;
            else if (this.Keyboard[Key.Right])
                dx = 1.0f;
            if (this.Keyboard[Key.Up])
                dy = -1.0f;
            else if (this.Keyboard[Key.Down])
                dy = 1.0f;

            this.x += 100.0f * dx * (float)e.Time;
			this.y += 100.0f * dy * (float)e.Time; 
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(this.x - blockSize / 2.0, this.y - blockSize / 2.0);
            GL.TexCoord2(1, 0);
            GL.Vertex2(this.x + blockSize / 2.0, this.y - blockSize / 2.0);
            GL.TexCoord2(1, 1);
            GL.Vertex2(this.x + blockSize / 2.0, this.y + blockSize / 2.0);
            GL.TexCoord2(0, 1);
            GL.Vertex2(this.x - blockSize / 2.0, this.y + blockSize / 2.0);
            GL.End();

            GL.Flush();
            this.SwapBuffers();
        }

        private void defineControls()
        {
            if (thisGame.o is HumanPlayer)
            {
                Controls.Add(Key.A, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepLeft));
                Controls.Add(Key.S, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepDown));
                Controls.Add(Key.D, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepRight));
                Controls.Add(Key.W, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.StepUp));
                Controls.Add(Key.Q, new Tuple<Player, PlayerControlEnumeration>(thisGame.x, PlayerControlEnumeration.DeployTrap));
            }

            if (thisGame.x is HumanPlayer)
            {
                Controls.Add(Key.Left, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepLeft));
                Controls.Add(Key.Down, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepDown));
                Controls.Add(Key.Right, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepRight));
                Controls.Add(Key.Up, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.StepUp));
                Controls.Add(Key.P, new Tuple<Player, PlayerControlEnumeration>(thisGame.o, PlayerControlEnumeration.DeployTrap));
            }
        }
    }
}