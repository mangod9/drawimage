using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DrawImage
{
    class Program
    {
        // image dimensions
        const int X_MAX = 1500;
        const int Y_MAX = 800;
        // array to keep track of "used" pixels
        static bool[,] _arr = new bool[X_MAX, Y_MAX];

        // Blocks the window around x, y
        // 36 pixesl up down and 25 * handle length sideways. 
        static void BlockWindow(int x, int y, int n)
        {
            int windowXlo = (x - n * 25) < 0 ? 0 : (x - n * 25);
            int windowXhi = (x + n * 25) >= X_MAX ? X_MAX : (x + n * 25);
            int windowYlo = (y - 36) < 0 ? 0 : (y - 36);
            int windowYhi = y + 36 >= Y_MAX ? Y_MAX : (y + 36);
            for (int i = windowXlo; i < windowXhi; i++)
            {
                for (int j = windowYlo; j < windowYhi; j++)
                {
                    _arr[i, j] = true;
                }
            }
        }

        static void Main(string[] args)
        {
            Bitmap myBitmap = new(@"C:\temp\blank.bmp");
            Graphics g = Graphics.FromImage(myBitmap);
            var handles = new List<string>();

            // Read the file and create a List of handles.  
            using System.IO.StreamReader file =
                new(@"c:\temp\handles.txt");
            string handle = String.Empty;
            while ((handle = file.ReadLine()) != null)
                handles.Add(handle.ToLowerInvariant());

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Random r = new(3);// (int)DateTime.Now.Ticks);
            foreach (var _ in handles.OrderByDescending(x => x.Length))
            {
                int x = r.Next(0, X_MAX);
                int y = r.Next(0, Y_MAX);
                // if the pixel is used keep looking
                while (_arr[x, y])
                {
                    x = r.Next(X_MAX);
                    y = r.Next(Y_MAX);
                }

                g.DrawString(_,
                             new Font("Segoe UI", 25, FontStyle.Bold),
                             new SolidBrush(Color.FromArgb(0xc9, 0xd1, 0xd9)),
                             new PointF(x, y));
                BlockWindow(x, y, _.Length);
                Console.WriteLine("{0}, {1}: {2}", x, y, _);
            }

            myBitmap.Save(@"C:\temp\handles.bmp");
        }
    }
}
