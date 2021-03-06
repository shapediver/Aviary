﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using Wind.Geometry.Meshes;
using Flock.TJS.Geometry.Meshes;
using Flock.TJS.Assemblies;
using Flock.TJS.Build;

namespace Flock_GH.Build
{
    public class BuildThreeJs : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BuildThreeJs class.
        /// </summary>
        public BuildThreeJs()
          : base("Build THREE.js", "3JS", "---", "Aviary", "3D View")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Mesh", "M", "---", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            //pManager.AddGenericParameter("Element", "E", "WPF Control Element", GH_ParamAccess.item);
            pManager.AddGenericParameter("ThreeJS Object", "3JS", "Three.js", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<IGH_Goo> X = new List<IGH_Goo>();
            
            if (!DA.GetDataList(0, X)) return;

            ThreeJS tJS = new ThreeJS();
            

            tJS.SetHeader(new Header());
            tJS.SetFooter(new Footer());

            int i = 0;
            List<wMesh> Meshes = new List<wMesh>();
            foreach (IGH_Goo Obj in X)
            {
                wMesh M = new wMesh();
                Obj.CastTo(out M);

                fMesh tMesh = new fMesh(M);
                tMesh.BuildThreeGeometry(i);
                tJS.AddGeometry(tMesh);
                i += 1;
            }
            tJS.Assemble();
            DA.SetData(0, tJS);
        }


        /// <summary>
        /// Set Exposure level for the component.
        /// </summary>
        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.Flock_ThreeJs;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8890daf4-d3eb-439a-be3b-08a1f67ff51c"); }
        }
    }
}