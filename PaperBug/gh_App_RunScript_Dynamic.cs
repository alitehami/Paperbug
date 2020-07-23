using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace PaperBug
{
    public class gh_App_RunScript_Dynamic : GH_Component, IGH_VariableParameterComponent
    {
        /// <summary>
        /// Initializes a new instance of the DocumentPages class.
        /// </summary>
        public gh_App_RunScript_Dynamic()
          : base("Dynamic Script", "InDesign.App.Dynamic Run Script",
              "Runs a Dynamically Generated Inputs Script from path or a string",
              "PaperBug", "01.InDesign")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Application", "App", "Indesign Application Instance", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "Path", "Path to the Script File", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "Run", "Set True to Run the Script", GH_ParamAccess.item);
            pManager.AddBooleanParameter("-----", "-----", "-----", GH_ParamAccess.item);

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
            InDesignInterface.Indd_Application app = null;
            string path = null;
            bool run = false;

            if (!DA.GetData(0, ref app))
            { return; }

            if (!DA.GetData(1, ref path))
            { return; }

            if (!DA.GetData(2, ref run))
            { return; }

            if (run)
            {
                app.RunScript(path, app);
            }


        }

        //////
        //////
        //////
        //////
        //////
        

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            throw new NotImplementedException();
        }

        public void VariableParameterMaintenance()
        {
            throw new NotImplementedException();
        }


        //////
        //////
        //////
        //////


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
            get { return new Guid("10AF40B0-9FC4-4236-A312-3DBE1681DC5E"); }
            
        }
    }
}