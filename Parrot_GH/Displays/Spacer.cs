﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Wind.Containers;
using Wind.Utilities;
using Wind.Types;

using Parrot.Containers;
using Parrot.Displays;
using System.Windows.Forms;
using GH_IO.Serialization;

namespace Parrot_GH.Displays
{
    public class Spacer : GH_Component
    {
        //Stores the instance of each run of the control
        public Dictionary<int, wObject> Elements = new Dictionary<int, wObject>();

        public bool IsHorizontal = false;
        public bool IsFill = false;

        /// <summary>
        /// Initializes a new instance of the Spacer class.
        /// </summary>
        public Spacer()
          : base("Spacer", "Spacer", "---", "Aviary", "Dashboard Control")
        {
            this.UpdateMessage();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Color", "C", "Color", GH_ParamAccess.item, System.Drawing.Color.Transparent);
            pManager[0].Optional = true;
            pManager.AddNumberParameter("Width", "W", "---", GH_ParamAccess.item, 10);
            pManager[1].Optional = true;
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

            var pCtrl = new pSpacer(name);
            if (Elements.ContainsKey(C)) { Active = true; }

            //Check if control already exists
            if (Active)
            {
                if (Elements[C] != null)
                {
                    WindObject = Elements[C];
                    Element = (pElement)WindObject.Element;
                    pCtrl = (pSpacer)Element.ParrotControl;
                }
            }
            else
            {
                Elements.Add(C, WindObject);
            }

            //Set Unique Control Properties
            System.Drawing.Color color = System.Drawing.Color.Transparent;
            double Width = 0;

            if (!DA.GetData(0, ref color)) return;
            if (!DA.GetData(1, ref Width)) return;

            pCtrl.SetProperties(new wColor(color).ToMediaColor(),Width, IsHorizontal,IsFill);


            //Set Parrot Element and Wind Object properties
            if (!Active) { Element = new pElement(pCtrl.Element, pCtrl, pCtrl.Type); }
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

            Menu_AppendItem(menu, "Horizontal", ModeDirection, true, IsHorizontal);

            Menu_AppendSeparator(menu);

            Menu_AppendItem(menu, "Fill", ModeFill, true, IsFill);

        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("Horizontal", IsHorizontal);
            writer.SetBoolean("Extents", IsFill);

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            IsHorizontal = reader.GetBoolean("Horizontal");
            IsFill = reader.GetBoolean("Extents");

            this.UpdateMessage();
            this.ExpireSolution(true);
            return base.Read(reader);
        }

        private void ModeDirection(Object sender, EventArgs e)
        {
            IsHorizontal = !IsHorizontal;

            this.UpdateMessage();
            this.ExpireSolution(true);
        }

        private void ModeFill(Object sender, EventArgs e)
        {
            IsFill = !IsFill;

            this.ExpireSolution(true);
        }

        private void UpdateMessage()
        {
            if (IsHorizontal) { Message = "Horizontal"; } else { Message = "Vertical"; }
        }

        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quinary; }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Parrot_Margins;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{97bdbc35-269d-4122-8074-1e9e37d8475b}"); }
        }
    }
}