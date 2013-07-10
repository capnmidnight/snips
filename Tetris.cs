using System;
using System.Collections.Generic;
using System.Text;

namespace Tetris
{
    class Program
    {
        static bool[,] l1, l2, i, b, tp, z1, z2;
        static Piece[] pieces;
        static Piece curPiece, nextPiece;
        static Board board;
        static Random rand;
        static int score;
        static void Main(string[] args)
        {
            bool t = true;
            bool f = false;
            rand = new Random();
            l1 = new bool[,]{{t, f},
                             {t, f},
                             {t, t}};
            l2 = new bool[,]{{f, t},
                             {f, t},
                             {t, t}};
            i  = new bool[,]{{t, t, t, t}};
            b  = new bool[,]{{t, t},
                             {t, t}};
            tp = new bool[,]{{t, f},
                             {t, t},
                             {t, f}};
            z1 = new bool[,]{{t, t, f},
                             {f, t, t}};
            z2 = new bool[,]{{f, t, t},
                             {t, t, f}};
            pieces = new Piece[]{new Piece(l1, ConsoleColor.Blue, ConsoleColor.DarkBlue), 
                                 new Piece(l2, ConsoleColor.Green, ConsoleColor.DarkGreen),
                                 new Piece(i, ConsoleColor.Gray, ConsoleColor.DarkGray),
                                 new Piece(b, ConsoleColor.Cyan, ConsoleColor.DarkCyan),
                                 new Piece(tp, ConsoleColor.Magenta, ConsoleColor.DarkMagenta),
                                 new Piece(z2, ConsoleColor.Yellow, ConsoleColor.DarkYellow),
                                 new Piece(z1, ConsoleColor.Red, ConsoleColor.DarkRed)};
            board = new Board(40, 10);
            Console.CursorVisible = false;
            bool done = false;
            int time = Now();
            int nextIndex = rand.Next(pieces.Length);
            int dt = 500;
            score = 0;
            ShowScore();
            nextPiece = new Piece(pieces[nextIndex]);
            while (!done)
            {
                if (Now() - time > dt)
                {
                    time = Now();
                    if (curPiece == null)
                    {
                        nextPiece.Clean();
                        curPiece = pieces[nextIndex];
                        curPiece.MoveTo(36, 4);
                        done = !curPiece.Fits(board, 0, 0);
                        nextIndex = rand.Next(pieces.Length);
                        nextPiece.Set(pieces[nextIndex]);
                        nextPiece.MoveTo(45, 4);
                    }
                    MoveLeft();
                }
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        done = true;
                    }
                    if (curPiece != null)
                    {
                        if (key.Key == ConsoleKey.RightArrow)
                        {
                            curPiece.Clean();
                            curPiece.Rotate();
                            while (!curPiece.Fits(board, 0, 0)) curPiece.MoveBy(0, -1);
                        }
                        else if (key.Key == ConsoleKey.UpArrow && curPiece.Fits(board, 0, -1))
                        {
                            curPiece.Clean();
                            curPiece.MoveBy(0, -1);
                        }
                        else if (key.Key == ConsoleKey.DownArrow && curPiece.Fits(board, 0, 1))
                        {
                            curPiece.Clean();
                            curPiece.MoveBy(0, 1);
                        }
                        if (key.Key == ConsoleKey.LeftArrow)
                        {
                            MoveLeft();
                        }
                    }
                }
                int l = board.ClearLines();
                if(l > 0)
                {
                    score += l * l * 10;
                    if (dt > 50) dt -= l * 10;
                    if (dt < 50) dt = 50;
                    if (l <= 3)
                    {
                        for (int j = 0; j < l; ++j)
                        {
                            Console.Beep(3500 + j * 250, 100);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 10; ++j)
                        {
                            Console.Beep(3750 + (int)(250 * Math.Cos(j * 18 / Math.PI)), 50);
                            if (j % 2 == 0)
                            {
                                Console.BackgroundColor = ConsoleColor.White;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            Console.Clear();
                            board.Redraw();
                        }
                    }
                    ShowScore();
                }
                board.Draw();
                if (curPiece != null) curPiece.Draw();
                nextPiece.Draw();
            }
            Console.CursorTop = 13;
            Console.CursorLeft = 15;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Game Over!");
            Console.ReadKey(true);
            Console.ResetColor();
        }
        private static void MoveLeft()
        {
            if (curPiece.Fits(board, -1, 0))
            {
                curPiece.Clean();
                curPiece.MoveBy(-1, 0);
            }
            else
            {
                for (int i = 0; i < 10; ++i)
                {
                    Console.Beep(300-i*10, 10);
                }
                curPiece.Place(board);
                curPiece = null;
            }
        }
        private static int Now()
        {
            DateTime n = DateTime.Now;
            return n.Millisecond + 1000 * (n.Second * +60 * (n.Minute + 60 * (n.Hour + 24 * n.Day)));
        }
        private static void ShowScore()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.CursorLeft = 1;
            Console.CursorTop = 13;
            Console.Write("Score: {0}", score);
        }
    }
    class Piece
    {
        private bool[,] pattern;
        private int x, y, w, h, rc;
        private ConsoleColor f, b;
        private bool dirty;
        public Piece(Piece copy)
            : this(copy.pattern, copy.f, copy.b)
        {
        }
        public Piece(bool[,] pattern, ConsoleColor f, ConsoleColor b)
        {
            this.f = f;
            this.b = b;
            this.w = pattern.GetLength(0);
            this.h = pattern.GetLength(1);
            this.pattern = new bool[w, h];
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    this.pattern[x, y] = pattern[x, y];
                }
            }
            this.x = 0;
            this.y = 0;
            this.rc = 0;
            this.dirty = true;
        }
        public void Set(Piece copy)
        {
            this.f = copy.f;
            this.b = copy.b;
            this.w = copy.pattern.GetLength(0);
            this.h = copy.pattern.GetLength(1);
            this.pattern = new bool[w, h];
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    this.pattern[x, y] = copy.pattern[x, y];
                }
            }
            this.x = 0;
            this.y = 0;
            this.rc = 0;
            this.dirty = true;
        }
        public void Rotate()
        {
            dirty = true;
            rc = (rc + 1) % 4;
            bool[,] newPat = new bool[h, w];
            for (int dy = 0; dy < h; ++dy)
            {
                for (int dx = 0; dx < w; ++dx)
                {
                    newPat[dy, w-dx-1] = pattern[dx, dy];
                }
            }
            pattern = newPat;
            this.w = this.pattern.GetLength(0);
            this.h = this.pattern.GetLength(1);
        }
        public void Clean()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int dy = 0; dy < h; ++dy)
            {
                for (int dx = 0; dx < w; ++dx)
                {
                    if (pattern[dx, dy])
                    {
                        Console.CursorLeft = x + dx+1;
                        Console.CursorTop = y + dy+1;
                        Console.Write("#");
                    }
                }
            }
        }
        public void Draw()
        {
            if (dirty)
            {
                dirty = false;
                Console.BackgroundColor = b;
                Console.ForegroundColor = f;
                for (int dy = 0; dy < h; ++dy)
                {
                    for (int dx = 0; dx < w; ++dx)
                    {
                        if (pattern[dx, dy])
                        {
                            Console.CursorLeft = dx + x+1;
                            Console.CursorTop = dy + y+1;
                            Console.Write("#");
                        }
                    }
                }
            }
        }
        public void MoveBy(int dx, int dy)
        {
            dirty = true;
            x += dx;
            y += dy;
        }
        public void MoveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void Reset()
        {
            while (rc != 0)
            {
                this.Rotate();
            }
        }
        public void Place(Board b)
        {
            for (int dy = 0; dy < h; ++dy)
            {
                for (int dx = 0; dx < w; ++dx)
                {
                    if (pattern[dx, dy])
                    {
                        b.Set(this.x+dx, this.y+dy, this.f, this.b);
                    }
                }
            }
        }
        public bool Fits(Board b, int mx, int my)
        {
            for (int dy = 0; dy < h; ++dy)
            {
                for (int dx = 0; dx < w; ++dx)
                {
                    if (pattern[dx, dy] && !b.IsClear(x+dx+mx, y+dy+my))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
    class Board
    {
        private ConsoleColor[,] bc, fc;
        private bool[,] dirty;
        private int width, height;
        public Board(int width, int height)
        {
            bc = new ConsoleColor[width, height];
            fc = new ConsoleColor[width, height];
            dirty = new bool[width, height];
            this.width = width;
            this.height = height;
        }
        public void Redraw()
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    dirty[x, y] = true;
                }
            }
        }
        public void Draw()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int x = 0; x < width+2; ++x)
            {
                Console.Write("=");
            }
            for (int y = 0; y < height; ++y)
            {
                Console.CursorTop = y+1;
                Console.CursorLeft = 0;
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("=");
                for (int x = 0; x < width; ++x)
                {
                    if (dirty[x, y])
                    {
                        dirty[x, y] = false;
                        Console.BackgroundColor = bc[x, y];
                        Console.ForegroundColor = fc[x, y];
                        Console.CursorLeft = x+1;
                        Console.Write("#");
                    }
                }
                Console.CursorLeft = width + 1;
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("=");
            }
            Console.CursorTop = height + 1;
            Console.CursorLeft = 0;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int x = 0; x < width+2; ++x)
            {
                Console.Write("=");
            }
        }
        public void Set(int x, int y, ConsoleColor f, ConsoleColor b)
        {
            if (0 <= x && x < width && 0 <= y && y < height)
            {
                dirty[x, y] = true;
                fc[x, y] = f;
                bc[x, y] = b;
            }
        }
        public void Clear(int x, int y)
        {
            if (0 <= x && x < width && 0 <= y && y < height)
            {
                dirty[x, y] = true;
                fc[x, y] = ConsoleColor.Black;
                bc[x, y] = ConsoleColor.Black;
            }
        }
        public void Clear()
        {
            Console.Clear();
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    dirty[x, y] = false;
                    fc[x, y] = ConsoleColor.Black;
                    bc[x, y] = ConsoleColor.Black;
                }
            }
        }
        public bool IsClear(int x, int y)
        {
            return 0 <= x && x < width && 0 <= y && y < height && fc[x, y] == ConsoleColor.Black && bc[x, y] == ConsoleColor.Black;
        }
        public int ClearLines()
        {
            int total = 0;
            int x = 0;
            while(x < width)
            {
                int count = 0;
                for (int y = 0; y < height; ++y)
                {
                    if (!IsClear(x, y))
                    {
                        count++;
                    }
                }
                if (count != this.height)
                {
                    x++;
                }
                else
                {
                    total++;
                    for (int dx = x; dx < width; ++dx)
                    {
                        for (int y = 0; y < height; ++y)
                        {
                            dirty[dx, y] = true;
                            if (dx < width - 1)
                            {
                                fc[dx, y] = fc[dx + 1, y];
                                bc[dx, y] = bc[dx + 1, y];
                            }
                            else
                            {
                                fc[dx, y] = ConsoleColor.Black;
                                bc[dx, y] = ConsoleColor.Black;
                            }
                        }
                    }
                }
            }
            return total;
        }
    }
}