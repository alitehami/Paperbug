using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PaperBug
{
    public class gh_Doc_Pages : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DocumentPages class.
        /// </summary>
        public gh_Doc_Pages()
          : base("Document Pages", "InDesign.Doc.Pages",
              "Get the Pages in The Document",
              "PaperBug", "01.Setup")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Document", "Doc", "Indesign Document", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Pages", "Pages", "Pages of the Document", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Document doc = null;

            if (!DA.GetData(0, ref doc))
            { return; }

            List<InDesignInterface.Indd_Page> pages = doc.pages();
            DA.SetDataList(0, pages);

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
            get { return new Guid("c2cb3da5-2786-4414-86cb-8ea8559be131"); }
        }
    }
}