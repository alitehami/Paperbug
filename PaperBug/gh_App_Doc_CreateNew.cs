using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PaperBug
{
    public class gh_App_Doc_CreateNew : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DocumentPages class.
        /// </summary>
        public gh_App_Doc_CreateNew()
          : base("Create Document", "InDesign.App.Create Document",
              "Creates A New Document",
              "PaperBug", "01.InDesign")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Indesign Application Instance", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "Path", "Path to save the new File", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager.AddBooleanParameter("Run", "Run", "Set True to Create the new Document", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Document", "Doc", "Currently Created Document", GH_ParamAccess.item);
            pManager.AddGenericParameter("Name", "Name", "Document's Name", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Application app = null;
            string path = null;
            bool run = false;

            if (!DA.GetData(0, ref app))
            { return; }
            if (!DA.GetData(2, ref run))
            { return; }

            if (run)
            {
                InDesignInterface.Indd_Document doc = app.CreateNewDocument();
                DA.SetData(0, doc);
                DA.SetData(1, doc.InDesignDoc.Name);
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
            get { return new Guid("65196B0A-20A7-4CCE-86E7-D1A7E190DB21"); }
            
        }
    }
}