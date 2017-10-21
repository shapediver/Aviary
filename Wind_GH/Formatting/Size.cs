﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

using Wind.Containers;
using Wind.Types;

using Parrot.Containers;
using Pollen.Collections;
using Parrot.Controls;

namespace Wind_GH.Formatting
{
    public class Size : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Size class.
        /// </summary>
        public Size()
          : base("Size", "Size", "---", "Aviary", "Format")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "O", "Updated Wind Object", GH_ParamAccess.item);
            pManager.AddNumberParameter("Width", "W", "---", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddNumberParameter("Height", "H", "---", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "O", "Updated Wind Object", GH_ParamAccess.item);
            pManager.AddGenericParameter("Graphics", "G", "Graphics Object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            IGH_Goo Element = null;
            double width = 0;
            double height = 0;

            if (!DA.GetData(0, ref Element)) return;
            if (!DA.GetData(1, ref width)) return;
            if (!DA.GetData(2, ref height)) return;

            wObject W = new wObject();
            if (Element != null) { Element.CastTo(out W); }
            wGraphic G = W.Graphics;

            G.Width = width;
            G.Height = height;
            
            W.Graphics = G;

            switch (W.Type)
            {
                case "Parrot":
                    pElement E = (pElement)W.Element;
                    pControl C = (pControl)E.ParrotControl;

                    C.Graphics = G;
                    C.SetSize();
                    break;
                case "Pollen":
                    switch (W.SubType)
                    {
                        case "DataPoint":
                            DataPt tDataPt = (DataPt)W.Element;
                            tDataPt.Graphics = G;
                            W.Element = tDataPt;
                            break;
                        case "DataSet":
                            DataSetCollection tDataSet = (DataSetCollection)W.Element;
                            tDataSet.Graphics = G;
                            W.Element = tDataSet;
                            break;
                    }
                    break;
            }

            DA.SetData(0, W);
        }


        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Wind_Sizing;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{81c07c87-da27-4a88-a70d-4a8c4185a214}"); }
        }
    }
}