﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;

using Wind.Containers;

using Pollen.Collections;
using System.Windows.Forms;
using Wind.Geometry.Vectors;
using GH_IO.Serialization;
using Wind.Types;
using Wind.Presets;

namespace Pollen_GH.Data
{
    public class SetDataPoint : GH_Component
    {
        int modeStatus = 0;
        int LabelStatus = 2;

        /// <summary>
        /// Initializes a new instance of the DataPoint class.
        /// </summary>
        public SetDataPoint()
          : base("DataPoint", "DataPt","---", "Aviary", "Charting & Data")
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Value", "V", "---", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Data Type", "D", "---", GH_ParamAccess.item, 1);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Format", "F", "---", GH_ParamAccess.item, "G");
            pManager[2].Optional = true;
            pManager.AddTextParameter("Tag", "T", "---", GH_ParamAccess.item,"");
            pManager[3].Optional = true;

            Param_Integer param = (Param_Integer)Params.Input[1];
            param.AddNamedValue("String", 0);
            param.AddNamedValue("Number", 1);
            param.AddNamedValue("Integer", 2);
            param.AddNamedValue("Domain", 3);
            param.AddNamedValue("Point", 4);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("DataObject", "D", "---", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo X = null;
            int D = 0;
            string F = "G";
            string T = "";

            if (!DA.GetData(0, ref X)) return;
            if (!DA.GetData(1, ref D)) return;
            if (!DA.GetData(2, ref F)) return;
            if (!DA.GetData(3, ref T)) return;

            DataPt DataObj = new DataPt();

            object obj = new object();
            X.CastTo(out obj);
            DataObj.Value = obj;

            DataObj.Type = D;
            DataObj.Tag = T;

            DataObj.Label.Format = F;
            DataObj.ToolTip.Format = F;

            DataObj.Graphics.Width = 1;

            string L = "";

            switch (D)
            {
                case 1:
                    double num = new double();
                    X.CastTo(out num);

                    L = string.Format("{0:" + F + "}",num);

                    DataObj.Integer = (int)num;
                    DataObj.Number = num;
                    DataObj.Text = num.ToString();
                    DataObj.Domain = new Tuple<double, double>(0, num);

                    break;
                case 2:
                    int intg = new int();
                    X.CastTo(out intg);

                    L = string.Format("{0:" + F + "}",intg);

                    DataObj.Text = intg.ToString();
                    DataObj.Integer = intg;
                    DataObj.Number = (double)intg;
                    DataObj.Domain = new Tuple<double, double>(0, intg);
                    break;
                case 3:
                    Interval domain = new Interval();
                    X.CastTo(out domain);

                    L = string.Format("{0:" + F + "}" +" to " + "{1:" + F + "}", domain.T0, domain.T1);

                    DataObj.Text = domain.ToString();
                    DataObj.Domain = new Tuple<double, double>(domain.T0, domain.T1);
                    DataObj.Point = new wPoint(domain.T0, domain.T1, 0);
                    DataObj.Graphics.Width = 10;
                    break;
                case 4:
                    Point3d point = new Point3d();
                    X.CastTo(out point);

                    L = string.Format("{0:" + F + "}", point.Z);

                    DataObj.Text = point.ToString();
                    DataObj.Point = new wPoint(point.X,point.Y,point.Z);

                    DataObj.Marker.Mode = wMarker.MarkerType.Circle;
                    break;
                default:
                    string text = "";
                    X.CastTo(out text);
                    L = text;
                    DataObj.Text = text;
                    break;
            }

            switch (LabelStatus)
            {
                default:
                    DataObj.Label.Content = "";
                    DataObj.ToolTip.Content = "";
                    break;
                case 1:
                    DataObj.Label.Enabled = true;
                    DataObj.Label.Content = T;
                    DataObj.ToolTip.Enabled = true;
                    DataObj.ToolTip.Content = T;
                    break;
                case 2:
                    DataObj.Label.Enabled = true;
                    DataObj.Label.Content = L;
                    DataObj.ToolTip.Enabled = true;
                    DataObj.ToolTip.Content = L;
                    break;
            }

            DataObj.Graphics.FontObject.FontColor = wColors.Gray;
            DataObj.Graphics.Background = wColors.VeryLightGray;
            DataObj.Graphics.StrokeColor = wColors.OffWhite;
            DataObj.Graphics.SetUniformStrokeWeight(1);
            DataObj.Graphics.SetUniformPadding(2);

            DataObj.ToolTip.Graphics.Background = new wColor(150, 250, 250, 250);
            DataObj.ToolTip.Graphics.FontObject.Size = 10;
            DataObj.ToolTip.Graphics.FontObject.FontColor = wColors.Gray;

            wObject WindObject = new wObject(DataObj, "Pollen", "DataPoint");
            WindObject.Graphics = DataObj.Graphics;

            DA.SetData(0, WindObject);
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendSeparator(menu);

            Menu_AppendItem(menu, "None", ModeLabelNone, true, (LabelStatus == 0));
            Menu_AppendItem(menu, "Tag", ModeLabelTag, true, (LabelStatus == 1));
            Menu_AppendItem(menu, "Value", ModeLabelValue, true, (LabelStatus == 2));

            Menu_AppendSeparator(menu);

            Menu_AppendItem(menu, "None", ModeNone, true, (modeStatus == 0));
            Menu_AppendItem(menu, "Chart", ModeChart, true, (modeStatus == 1));
            Menu_AppendItem(menu, "Data Grid", ModeGrid, true, (modeStatus == 2));
            Menu_AppendItem(menu, "Excel", ModeExcel, true, (modeStatus == 3));

        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("Label", LabelStatus);
            writer.SetInt32("Mode", modeStatus);

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            LabelStatus = reader.GetInt32("Label");
            modeStatus = reader.GetInt32("Mode");

            return base.Read(reader);
        }

        private void ModeLabelNone(Object sender, EventArgs e)
        {
            LabelStatus = 0;
            this.ExpireSolution(true);
        }

        private void ModeLabelTag(Object sender, EventArgs e)
        {
            LabelStatus = 1;
            this.ExpireSolution(true);
        }

        private void ModeLabelValue(Object sender, EventArgs e)
        {
            LabelStatus = 2;
            this.ExpireSolution(true);
        }

        private void ModeNone(Object sender, EventArgs e)
        {
            modeStatus = 0;
            this.ExpireSolution(true);
        }

        private void ModeChart(Object sender, EventArgs e)
        {
            modeStatus = 1;
            this.ExpireSolution(true);
        }

        private void ModeGrid(Object sender, EventArgs e)
        {
            modeStatus = 2;
            this.ExpireSolution(true);
        }

        private void ModeExcel(Object sender, EventArgs e)
        {
            modeStatus = 3;
            this.ExpireSolution(true);
        }

        /// <summary>
        /// The Exposure property control
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.primary; }
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                return Properties.Resources.Wind_DataPoint;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{80c52f7c-bd0b-410f-986f-3ea4f6f99663}"); }
        }
    }
}