using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace distort
{
    public partial class Form1 : Form
    {
        int Ox, Oy;
        double R, F, W, P;
        byte[] grid;
        int width, height;
        Thread t;
        Bitmap b;
        Graphics front, back;
        double pi2 = 2 * Math.PI;
        bool done, finished;
        byte mapPixel(int x, int y, int rgb)
        {
            int dx = Ox - x;
            int dy = Oy - y;
            double D = Math.Sqrt(dx * dx + dy * dy);
            double theta = (D - R);
            if (theta > 0 && theta < W)
            {
                theta *= pi2 * F;
                double A = Math.Atan2(dy, dx);
                double t = P * (1 - Math.Cos(theta));
                int px = (int)(x + t * Math.Cos(A));
                if (px >= 0 && px < width)
                {
                    int py = (int)(y + t * Math.Sin(A));
                    if (py >= 0 && py < height)
                    {
                        return get(px, py, rgb);
                    }
                }
            }
            return get(x, y, rgb);
        }
        byte get(int x, int y, int rgb)
        {
            int l = (y * width + x) * 3 + rgb;
            //if (0 <= l && l < grid.Length)
            return grid[l];
            //return 0;
        }
        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                t = new Thread(new ThreadStart(this.Run));
                Bitmap image = new Bitmap(openFileDialog1.FileName);
                width = image.Width;
                height = image.Height;
                this.ClientSize = new Size(width, height);
                front = this.CreateGraphics();
                BitmapData d = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, image.PixelFormat);
                grid = new byte[d.Stride * height];
                Marshal.Copy(d.Scan0, grid, 0, grid.Length);
                image.UnlockBits(d);
                b = new Bitmap(width, height);
                back = Graphics.FromImage(b);
                W = 25;
                F = 1.0 / W;
                P = 50;
                this.Focus();
                done = false;
                t.Start();
            }
            else
            {
                Application.Exit();
            }

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            done = true;
            while (!finished) Application.DoEvents();
            base.OnClosing(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            Ox = e.X;
            Oy = e.Y;
            R = 0;
        }
        void Run()
        {
            finished = false;
            while (!done)
            {
                using (Bitmap o = new Bitmap(width, height))
                {
                    byte[] og = new byte[grid.Length];
                    for (int y = 0; y < height; ++y)
                        for (int x = 0; x < width; ++x)
                            for (int rgb = 0; rgb < 3; ++rgb)
                                og[y * width + x] = mapPixel(x, y, rgb);

                    BitmapData bd = o.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, o.PixelFormat);
                    Marshal.Copy(og, 0, bd.Scan0, og.Length);
                    o.UnlockBits(bd);
                    back.DrawImageUnscaled(o, 0, 0);
                }
                back.Flush();
                front.DrawImageUnscaled(b, this.ClientRectangle.X, this.ClientRectangle.Y);
                R += 1.0;
                Application.DoEvents();
            }
            finished = true;
        }
    }
}
