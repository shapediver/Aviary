﻿using System.Collections.Generic;
using System.Drawing;

namespace Macaw.Utilities.Channels
{
    public class mGetBrightness : mGetChannel
    {
        public mGetBrightness(Bitmap BaseBitmap)
        {
            Bitmap bmp = new Bitmap(BaseBitmap);

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Values.Add(bmp.GetPixel(i, j).GetBrightness());
                }
            }
        }
        
        public mGetBrightness(Bitmap BaseBitmap, List<double> X, List<double> Y)
        {

            Bitmap bmp = new Bitmap(BaseBitmap);
            for (int i = 0; i < X.Count; i++)
            {
                Values.Add(bmp.GetPixel((int)((bmp.Width - 1) * X[i]), (int)((bmp.Height - 1) * Y[i])).GetBrightness());
            }
        }


    }
}