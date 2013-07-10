using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Pong
{
    public class Form1 : Form
    {
        static readonly Brush bbg, bfg;
        static readonly Pen pbg, pfg;

        static Form1()
        {
            bbg = new SolidBrush(Color.Black);
            bfg = new SolidBrush(Color.White);
            pbg = new Pen(Color.Black);
            pfg = new Pen(Color.White);
        }

        Area ball;
        int bdx, bdy, width, height;
        bool isRunning;
        Graphics front;
        public Form1()
        {
            MinimizeBox = false;
            Visible = true;
            Closing += new System.ComponentModel.CancelEventHandler(Form1_Closing);
            width = ClientSize.Width;
            height = ClientSize.Height;
            ball.X = (width - ball.W) / 2;
            ball.Y = (height - ball.H) / 2;
            BackColor = Color.Black;
            bdx = 2;
            bdy = 1;
            isRunning = true;
            front = CreateGraphics();
        }

        void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isRunning = false;
            e.Cancel = true;
        }
        void Erase()
        {
            if (isRunning)
                front.FillRectangle(bbg, ball.X, ball.Y, ball.W, ball.H);
        }
        void Draw()
        {
            if (isRunning)
                front.FillRectangle(bfg, ball.X, ball.Y, ball.W, ball.H);
        }
        void UpdateIt()
        {
            ball.X += bdx;
            if (ball.R >= width || ball.X <= 0)
                bdx *= -1;
            ball.Y += bdy;
            if (ball.B >= height || ball.Y <= 0)
                bdy *= -1;
        }
        public void Run()
        {
            while (isRunning)
            {
                Erase();
                UpdateIt();
                Draw();
                Application.DoEvents();
            }
            Application.Exit();
        }
    }

    struct Area
    {
        int x, y, w, h, oldx, oldy;
        public Area(int w, int h)
        {
            x = y = oldx = oldy = 0;
            this.w = w;
            this.h = h;
        }
        public int X { get { return x; } set { oldx = x; x = value; } }
        public int Y { get { return y; } set { oldy = y; y = value; } }
        public int H { get { return h; } }
        public int W { get { return w; } }
        public int R { get { return x + w; } }
        public int B { get { return y + h; } }
    }
}