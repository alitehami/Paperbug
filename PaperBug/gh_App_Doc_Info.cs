using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using InDesign;

namespace PaperBug
{
    public class gh_App_Doc_Info : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DocumentPages class.
        /// </summary>
        public gh_App_Doc_Info()
          : base("Document Info", "Doc Info",
              "Gets A Document's Information",
              "PaperBug", "01.InDesign")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Document", "Doc", "Indesign Document Instance", GH_ParamAccess.item);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            /* 0 */ pManager.AddGenericParameter("Name", "Name", "Document's Name", GH_ParamAccess.item);
            /* 1 */ pManager.AddGenericParameter("Path", "Path", "Path to the Document location", GH_ParamAccess.item);
            /* 2 */ pManager.AddGenericParameter("Doc Units", "Units", "The Document's Measurement Units\n(H = Horizontal Measurement Units)\n(V = Vertical Measurement Units)", GH_ParamAccess.item);
            /* 3 */ pManager.AddGenericParameter("Page Count", "Count", "Number of Pages", GH_ParamAccess.item);
            /* 4 */ pManager.AddGenericParameter("Page Orientation", "Orient", "The Document's Page Orientation", GH_ParamAccess.item);
            /* 5 */ pManager.AddGenericParameter("Page Width", "Width", "Page Width", GH_ParamAccess.item);
            /* 6 */ pManager.AddGenericParameter("Page Height", "Height", "Page Width", GH_ParamAccess.item);
            /* 7 */ pManager.AddGenericParameter("Facing Pages", "Facing", "Reports if Facing-Pages mode is Enabled", GH_ParamAccess.item);
            /* 8 */ pManager.AddGenericParameter("Active Layer", "Layer", "Active Layer", GH_ParamAccess.item);
            /* 9 */ pManager.AddGenericParameter("Doc Modified", "Modified", "Reports if Document is Modified from last Save", GH_ParamAccess.item);
            /* 10*/ pManager.AddGenericParameter("Doc Read Only", "ReadOnly", "Reports if Document is in a Read Only State", GH_ParamAccess.item);
            /* 11*/ pManager.AddGenericParameter("Doc Recovered", "Recovered", "Reports if Document is a 'Recovered' Copy", GH_ParamAccess.item);

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

                                 
            //DA[0] Doc Name
            string _DocName = doc.InDesignDoc.Name;

            //DA[1] Doc Path
            string _DocPath;
            bool _DocSaved = doc.InDesignDoc.Saved;
            
            if (_DocSaved)
            {
                _DocPath = doc.InDesignDoc.FilePath;
            }
            else
            {
                _DocPath = "Document is Never Saved Yet!";
            }

            //DA[2] Doc Measurement Units 
            string _DocUnits;
            var w = doc.InDesignDoc.ViewPreferences.HorizontalMeasurementUnits;
            var h = doc.InDesignDoc.ViewPreferences.VerticalMeasurementUnits;

            if (w == h)
                _DocUnits = w.ToString().Remove(0, 2);
            else
                _DocUnits = string.Format("{0}(H) & {1}(V)", w.ToString().Remove(0, 2), h.ToString().Remove(0, 2));

            //DA[3] Doc Pages Count
            int _PageCount = doc.InDesignDoc.Pages.Count;

            //DA[4] Doc Pages Orientation
            string _PageOrientation = doc.InDesignDoc.DocumentPreferences.PageOrientation.ToString().Remove(0, 2);



            //DA[5] Doc Page Width
            double _PageWidth = doc.InDesignDoc.DocumentPreferences.PageWidth;

            //DA[6] Doc Page Width
            double _PageHeight = doc.InDesignDoc.DocumentPreferences.PageHeight;

            //DA[7] is Doc Recovered
            bool _FacingPages = doc.InDesignDoc.DocumentPreferences.FacingPages;

            //DA[8] Active Layer Name
            string _ActiveLayer = ( (InDesign.Layer)doc.InDesignDoc.ActiveLayer ).Name;

            //DA[9] is Doc Modified
            bool _DocModified = doc.InDesignDoc.Modified;

            //DA[10] is Doc ReadOnly
            bool _DocReadOnly = doc.InDesignDoc.ReadOnly;

            //DA[11] is Doc Recovered
            bool _DocRecovered = doc.InDesignDoc.Recovered;

            
            
            DA.SetData(0, _DocName);
            DA.SetData(1, _DocPath);
            DA.SetData(2, _DocUnits);
            DA.SetData(3, _PageCount);
            DA.SetData(4, _PageOrientation);
            DA.SetData(5, _PageWidth);
            DA.SetData(6, _PageHeight);
            DA.SetData(7, _FacingPages);
            DA.SetData(8, _ActiveLayer);
            DA.SetData(9, _DocModified);
            DA.SetData(10, _DocReadOnly);
            DA.SetData(11, _DocRecovered);
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
            get { return new  Guid("8145E284-1BA1-4291-8036-F313D55D6B3E") ; }
            
        }
    }
}


