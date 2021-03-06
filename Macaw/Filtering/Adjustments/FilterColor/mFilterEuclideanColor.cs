﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Accord;
using Accord.Imaging.Filters;

using Wind.Types;
using Macaw.Filtering;

namespace Macaw.Filtering.Adjustments.FilterColor
{
    public class mFilterEuclideanColor : mFilter
    {
        EuclideanColorFiltering Effect = new EuclideanColorFiltering();

        wColor Source = new wColor(255,0,0);
        wColor Target = new wColor(0, 0, 255);
        short Radius = 100;

        public mFilterEuclideanColor(wColor SourceColor, wColor TargetColor,  short RadiusValue)
        {
            Source = SourceColor;
            Target = TargetColor;
            Radius = RadiusValue;

            BitmapType = mFilter.BitmapTypes.None;

            Effect = new EuclideanColorFiltering(new Accord.Imaging.RGB((byte)Source.R, (byte)Source.G, (byte)Source.B),Radius);

            Effect.FillColor = new Accord.Imaging.RGB((byte)Target.R, (byte)Target.G, (byte)Target.B);

            filter = Effect;
        }

    }
}