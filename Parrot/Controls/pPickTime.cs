﻿using System;
using System.Windows;
using System.Windows.Media;

using Wind.Containers;
//using Xceed.Wpf.Toolkit;

using MaterialDesignThemes.Wpf;
//using MahApps.Metro.Controls;

namespace Parrot.Controls
{
    public class pPickTime : pControl
    {
        public TimePicker Element;

        public pPickTime(string InstanceName)
        {
            //Set Element info setup
            Element = new TimePicker();
            Element.Name = InstanceName;
            Type = "TimePicker";
        }

        public void SetProperties(DateTime date, int mode, string format)
        {

        }

        private string DateStructures(int type)
        {
            switch (type)
            {
                case 1:
                    return "hh:mm tt";
                case 2:
                    return "hh: mm: s tt";
                case 3:
                    return "HH:mm:ss";
                default:
                    return "HH:mm:ss";
            }
        }

        public override void SetFill()
        {
            Element.Background = Graphics.WpfFill;
        }

        public override void SetStroke()
        {
            Element.BorderThickness = new Thickness(Graphics.StrokeWeight[0], Graphics.StrokeWeight[1], Graphics.StrokeWeight[2], Graphics.StrokeWeight[3]);
            Element.BorderBrush = new SolidColorBrush(Graphics.StrokeColor.ToMediaColor());
        }

        public override void SetSize()
        {
            if (Graphics.Width < 1) { Element.Width = double.NaN; } else { Element.Width = Graphics.Width; }
            if (Graphics.Height < 1) { Element.Height = double.NaN; } else { Element.Height = Graphics.Height; }
        }

        public override void SetMargin()
        {
            Element.Margin = new Thickness(Graphics.Margin[0], Graphics.Margin[1], Graphics.Margin[2], Graphics.Margin[3]);
        }

        public override void SetPadding()
        {
            Element.Padding = new Thickness(Graphics.Padding[0], Graphics.Padding[1], Graphics.Padding[2], Graphics.Padding[3]);
        }

        public override void SetFont()
        {
            Element.Foreground = new SolidColorBrush(Graphics.FontObject.FontColor.ToMediaColor());
            Element.FontFamily = Graphics.FontObject.ToMediaFont().Family;
            Element.FontSize = Graphics.FontObject.Size;
            Element.FontStyle = Graphics.FontObject.ToMediaFont().Italic;
            Element.FontWeight = Graphics.FontObject.ToMediaFont().Bold;
        }

    }
}
