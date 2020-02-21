using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PaperBug
{
    public class gh_Page_Items : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DocumentPages class.
        /// </summary>
        public gh_Page_Items()
          : base("Page Items", "InDesign.Doc.Page.Items",
              "Get the Items in a Page",
              "PaperBug", "01.InDesign")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Page", "Page", "Indesign Document's Page", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Items", "Items", "Items in the Page", GH_ParamAccess.list);
            pManager.AddGenericParameter("Names", "Names", "Items Names", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Page page = null;
            

            if (!DA.GetData(0, ref page))
            { return; }
            List<string> names;
            List<InDesignInterface.Indd_PageItem> items = page._AllPageItemsOfType(out names);
            DA.SetDataList(0, items);
            DA.SetDataList(0, names);

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
            get { return new Guid("E15206BE-F711-4BEA-85B3-FCFA67704AD7"); }
        }
    }


}