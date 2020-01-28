using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PaperBug
{
    public class gh_App_Doc_Close : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DocumentPages class.
        /// </summary>
        public gh_App_Doc_Close()
          : base("Close Document", "InDesign.App.Close Document",
              "Closes A Document",
              "PaperBug", "01.InDesign")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Document", "Doc", "Indesign Document Instance", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "Path", "Path to save the new File", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Save", "Save", "Set True to save before Closing the Document", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "Run", "Set True to Close the Document", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Document doc = null;
            string path = null;
            bool save = false;
            bool run = false;

            if (!DA.GetData(0, ref doc))
            { return; }
            if (!DA.GetData(2, ref save))
            { return; }
            if (!DA.GetData(3, ref run))
            { return; }

            if (run)
            {
                doc.CloseDocument(doc,save);
            }




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
            get { return new  Guid("E008B1B5-A271-4762-858F-9199BE8F495F") ; }
            
        }
    }
}