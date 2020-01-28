using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PaperBug
{
    public class gh_App_Doc_OpenFromPath : GH_Component
    {

        public gh_App_Doc_OpenFromPath()
          : base("Open Document From Path", "Open Doc Path",
              "Opens a Docuemnt from provided Path",
              "PaperBug", "01.InDesign")
        {
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Indesign Application Instance", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "Path", "Path to the InDesign Docuemnt to be opened", GH_ParamAccess.item);
        }


        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Document", "Doc", "Currently Opened Document", GH_ParamAccess.item);
            pManager.AddGenericParameter("Name", "Name", "Document's Name", GH_ParamAccess.item);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Application app = null;
            string path = null;

            if (!DA.GetData(0, ref app))
            { return; }
            if (!DA.GetData(1, ref path))
            { return; }

            string docName = null;

            InDesignInterface.Indd_Document doc = (app as InDesignInterface.Indd_Application).OpenDocumentFromPath(path, out docName);

            DA.SetData(0, doc);
            DA.SetData(1, docName);

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Properties.Resources.paperbug_icon_24;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("e7f09cab-d46b-4475-aa33-9013767dc705"); }


        }
    }
}