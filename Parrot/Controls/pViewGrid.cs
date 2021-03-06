﻿using System;

using System.Windows;
using System.Windows.Media;
using System.Data;
using System.Collections.Generic;

using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;

using Wind.Containers;
using Wind.Collections;

namespace Parrot.Controls
{
    public class pViewGrid : pControl
    {
        public DataGrid Element;

        DataTable Table = new DataTable();
        DataSet DS = new DataSet();

        List<string> GridTitles = new List<string>();

        public pViewGrid(string InstanceName)
        {
            //Set Element info setup
            Element = new DataGrid();
            Element.Name = InstanceName;
            Type = "GridView";

            //Set "Clear" appearance to all elements
        }
        

        public void SetProperties(int GridType, bool ResizeRows, bool ResizeCols, bool Sortable, bool Alternate, bool AddRows)
        {
            Table = new DataTable();
            DS = new DataSet();

            DS.Tables.Add(Table);
            
            Element.CanUserResizeColumns = ResizeCols;
            Element.CanUserResizeRows = ResizeRows;

            Element.CanUserSortColumns = Sortable;
            Element.VerticalGridLinesBrush = Element.HorizontalGridLinesBrush;
            Element.CanUserAddRows = AddRows;

            switch (GridType)
            {
                case (1):
                    Element.GridLinesVisibility = DataGridGridLinesVisibility.Vertical;
                    break;
                case (2):
                    Element.GridLinesVisibility = DataGridGridLinesVisibility.Horizontal;
                    break;
                case (3):
                    Element.GridLinesVisibility = DataGridGridLinesVisibility.All;
                    break;
                default:
                    Element.GridLinesVisibility = DataGridGridLinesVisibility.None;
                    break;
            }

            if (Alternate) {
                Element.AlternationCount = 2;
                Element.AlternatingRowBackground = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                Element.AlternationCount = 0;
                Element.AlternatingRowBackground = Element.Background;
            }
            
            Element.AutoGenerateColumns = true;
        }

        public void SetTitles(List<string> Titles)
        {
            GridTitles = Titles;
            for (int i = 0; i < Titles.Count; i++)
            {
                DataColumn col = new DataColumn(Titles[i], typeof(string));
                Table.Columns.Add(col);
            }

        }

        public void SetRows(int TotalRows)
        {
            for (int i = 0; i < TotalRows; i++)
            {
                System.Data.DataRow row = Table.NewRow();
                Table.Rows.Add(row);
            }
        }


        public void AddRow(string Title,List<string> DataColumnValues)
        {
            
                for (int i = 0; i < DataColumnValues.Count; i++)
                {
                    Table.Rows[i][Title] = DataColumnValues[i];
                }
        }

        public void BuildTable()
        {
            Element.ItemsSource = Table.DefaultView;
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
