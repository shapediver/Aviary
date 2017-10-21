﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Wind.Containers;
using Grasshopper.Kernel.Parameters;
using Wind.Utilities;
using Parrot.Containers;
using Pollen.Charts;
using Grasshopper.Kernel.Types;
using Pollen.Collections;
using System.Windows.Forms;

namespace Pollen_GH.Charts
{
    public class ChartGauge : GH_Component
    {
        int modeStatus = 0;

        //Stores the instance of each run of the control
        public Dictionary<int, wObject> Elements = new Dictionary<int, wObject>();

        /// <summary>
        /// Initializes a new instance of the RadialChart class.
        /// </summary>
        public ChartGauge()
          : base("Gauge Chart", "Gauge Chart", "---", "Aviary", "Chart")
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "---", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Mode", "M", "---", GH_ParamAccess.item, 0);
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Horizontal", "H", "---", GH_ParamAccess.item, true);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("Gauge Size", "S", "---", GH_ParamAccess.item, 100);
            pManager[3].Optional = true;

            Param_Integer param = (Param_Integer)Params.Input[1];
            param.AddNamedValue("360 Doughnut", 0);
            param.AddNamedValue("180 Doughnut", 1);
            param.AddNamedValue("360 Pie", 2);
            param.AddNamedValue("180 Pie", 3);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Element", "E", "WPF Control Element", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string ID = this.Attributes.InstanceGuid.ToString();
            string name = new GUIDtoAlpha(Convert.ToString(ID + Convert.ToString(this.RunCount)), false).Text;
            int C = this.RunCount;

            wObject WindObject = new wObject();
            pElement Element = new pElement();
            bool Active = Elements.ContainsKey(C);

            var pControl = new pGaugeChartSeries(name);
            if (Elements.ContainsKey(C)) { Active = true; }

            //Check if control already exists
            if (Active)
            {
                WindObject = Elements[C];
                Element = (pElement)WindObject.Element;
                pControl = (pGaugeChartSeries)Element.PollenControl;
            }
            else
            {
                Elements.Add(C, WindObject);
            }

            //Set Unique Control Properties

            IGH_Goo D = null;
            int M = 0;
            bool B = true;
            int S = 100;

            if (!DA.GetData(0, ref D)) return;
            if (!DA.GetData(1, ref M)) return;
            if (!DA.GetData(2, ref B)) return;
            if (!DA.GetData(3, ref S)) return;

            wObject W = new wObject();
            D.CastTo(out W);

            DataSetCollection DC = (DataSetCollection)W.Element;

            pControl.SetProperties(B, 0);

            for (int i = 0; i < DC.Sets.Count; i++)
            {
                for (int j = 0; j < DC.Sets[i].Points.Count; j++)
                {
                    pGaugeChart ChartObj = new pGaugeChart(name + i + j);

                    ChartObj.SetProperties(DC, S);
                    ChartObj.SetGaugeType(M);
                    ChartObj.SetGaugeValues(DC.Sets[i].Points[j], modeStatus, DC.Sets[i].NumericBounds.T0, DC.Sets[i].NumericBounds.T1, DC.Sets[i].NumericSum);

                    pControl.AddChart(ChartObj);
                }
            }


            //Set Parrot Element and Wind Object properties
            if (!Active) { Element = new pElement(pControl.Element, pControl, pControl.Type); }
            WindObject = new wObject(Element, "Parrot", Element.Type);
            WindObject.GUID = this.InstanceGuid;
            WindObject.Instance = C;

            Elements[this.RunCount] = WindObject;

            DA.SetData(0, WindObject);

        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            Menu_AppendSeparator(menu);

            Menu_AppendItem(menu, "Sum Percentage", ModeSumPercent, true, (modeStatus == 0));
            Menu_AppendItem(menu, "Sum Value", ModeSumValue, true, (modeStatus == 1));
            Menu_AppendItem(menu, "Set Bound Value", ModeBndValue, true, (modeStatus == 2));
            Menu_AppendItem(menu, "Set Bound Percentage", ModeBndPercent, true, (modeStatus == 3));

        }


        private void ModeSumPercent(Object sender, EventArgs e)
        {
            modeStatus = 0;
            this.ExpireSolution(true);
        }

        private void ModeSumValue(Object sender, EventArgs e)
        {
            modeStatus = 1;
            this.ExpireSolution(true);
        }

        private void ModeBndValue(Object sender, EventArgs e)
        {
            modeStatus = 2;
            this.ExpireSolution(true);
        }

        private void ModeBndPercent(Object sender, EventArgs e)
        {
            modeStatus = 3;
            this.ExpireSolution(true);
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Pollen_Guage1;
            }
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{48cae1c1-6d39-4240-a86e-48d763183786}"); }
        }
    }
}