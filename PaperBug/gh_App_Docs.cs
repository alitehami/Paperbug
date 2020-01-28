using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PaperBug
{
    public class gh_App_Docs : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OpenDocuments class.
        /// </summary>
        public gh_App_Docs()
          : base("Open Documents", "Open Docs",
              "Gets A List of All Open Documents in the Input InDesign Application",
              "PaperBug","01.Setup")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Indesign Application Instance", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Documents", "Docs", "Current Open Documents Objects", GH_ParamAccess.list);
            pManager.AddGenericParameter("Names","Names","Current Open Documents Names",GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Application app = null;

            if (!DA.GetData(0, ref app))
            { return; }

            List<string> docsNames = new List<string>();

            List<InDesignInterface.Indd_Document> docs = (app as InDesignInterface.Indd_Application).OpenDocsNames(out docsNames);
            DA.SetDataList(0, docs);
            DA.SetDataList(1, docsNames);
           
        }


        

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.paperbug_icon_24;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7c35df1a-36aa-497f-9801-e01d8e1020f1"); }
        }
    }
}