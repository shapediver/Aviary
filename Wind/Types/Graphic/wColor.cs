﻿

using System;

namespace Wind.Types
{
    public class wColor
    {
        public int A = 255;
        public int R = 0;
        public int G = 0;
        public int B = 0;

        public wColor()
        {
        }

        public wColor(wColor WindColor)
        {
            A = WindColor.A;
            R = WindColor.R;
            G = WindColor.G;
            B = WindColor.B;
        }

        public wColor(int Red, int Green, int Blue)
        {
            R = Red;
            G = Green;
            B = Blue;
        }

        public wColor(int Alpha, int Red, int Green, int Blue)
        {
            A = Alpha;
            R = Red;
            G = Green;
            B = Blue;
        }

        public void Flatten()
        {
            A = 255;
        }

        public wColor(System.Windows.Media.Color MediaColor)
        {
            A = MediaColor.A;
            R = MediaColor.R;
            G = MediaColor.G;
            B = MediaColor.B;
        }

        public wColor(wColor StartColor, int Alpha)
        {
            A = Alpha;
            R = StartColor.R;
            G = StartColor.G;
            B = StartColor.B;
        }

        public void Lighten( double T)
        {
            R = (int)Math.Floor(R + (255 - R) * T);
            G = (int)Math.Floor(G + (255 - G) * T);
            B = (int)Math.Floor(B + (255 - B) * T);
        }

        public System.Windows.Media.Color? ToNullableMediaColor()
        {
            return System.Windows.Media.Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
        }

        public System.Windows.Media.Color ToMediaColor()
        {
            return System.Windows.Media.Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B);
        }
        
        public wColor(System.Drawing.Color DrawingColor)
        {
            A = DrawingColor.A;
            R = DrawingColor.R;
            G = DrawingColor.G;
            B = DrawingColor.B;
        }
        
        public System.Drawing.Color ToDrawingColor()
        {
            return System.Drawing.Color.FromArgb(A, R, G, B);
        }
        

    }
}
