﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

using System.Windows.Controls;

using Wind.Containers;


namespace Parrot.Controls
{
    public class pDropdownList : pControl
    {
        public ComboBox Element;

        public List<string> values = new List<string>();

        public pDropdownList(string InstanceName)
        {
            //Set Element info setup
            Element = new ComboBox();
            Element.Name = InstanceName;
            Type = "ComboBox";

            Element.VerticalContentAlignment = VerticalAlignment.Center;;

            //Set "Clear" appearance to all elements
        }
        
        public void SetProperties(List<string> Values, int index)
        {
            //Element.Items.Clear();
            Element.ItemsSource = Values;
            Element.SelectedIndex = index;
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
