﻿using System;

using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;
using Wind.Containers;
using Wind.Types;

namespace Parrot.Controls
{
    public class pViewList : pControl
    {
        public ListView Element;
        public Point startPoint;

        public ObservableCollection<Label> ItemsList = new ObservableCollection<Label>();

        public pViewList(string InstanceName)
        {
            //Set Element info setup
            Element = new ListView();
            Element.Name = InstanceName;
            Type = "ListView";

            Style itemContainerStyle = new Style(typeof(ListViewItem));
            itemContainerStyle.Setters.Add(new Setter(ListViewItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(ListViewItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            itemContainerStyle.Setters.Add(new EventSetter(ListViewItem.DropEvent, new DragEventHandler(listbox1_Drop)));
            Element.ItemContainerStyle = itemContainerStyle;

            Element.HorizontalContentAlignment = HorizontalAlignment.Stretch;

            //Set "Clear" appearance to all elements
        }

        public void SetProperties(List<string> D, List<wColor> C)
        {
            ItemsList.Clear();
            for (int i = 0; i < D.Count; i++)
            {
                Label TempText = new Label();
                TempText.Content = D[i];
                TempText.Background = new SolidColorBrush(C[i].ToMediaColor());
                TempText.HorizontalAlignment = HorizontalAlignment.Stretch;
                TempText.ToolTip = i;
                
                ItemsList.Add(TempText);
            }

            Element.ItemsSource = ItemsList;
            
        }
        
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem)
            {
                ListViewItem draggedItem = sender as ListViewItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        public void listbox1_Drop(object sender, DragEventArgs e)
        {
            Label droppedData = e.Data.GetData(typeof(Label)) as Label;
            Label target = ((ListViewItem)(sender)).DataContext as Label;
            
            int removedIdx = Element.Items.IndexOf(droppedData);
            int targetIdx = Element.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                ItemsList.Insert(targetIdx + 1, droppedData);
                ItemsList.RemoveAt(removedIdx);

                Element.SelectedIndex = targetIdx;
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (ItemsList.Count + 1 > remIdx)
                {
                    ItemsList.Insert(targetIdx, droppedData);
                    ItemsList.RemoveAt(remIdx);
                }

                Element.SelectedIndex = targetIdx;
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
