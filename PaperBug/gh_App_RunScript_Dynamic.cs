using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
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
            pManager.AddGenericParameter("InDesign App", "app", "Indesign Application Instance", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddTextParameter("Script", "src", "The Script as a Text input, or provide a Path to the Script File.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "run", "Set True to Run the Script", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("_Generate", "_gen_", "Automatically Generates input parameters by parsing the input script for \nany variables named with the #varName# syntax.\n\nIt is set to False by default to allow manual variables creation. \n \nIf Set to True then it will RECREATE all the dynamic inputs from the script text.", GH_ParamAccess.item, false);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Dynamic Script", "DS", "The Dynamic Script", GH_ParamAccess.item);
            pManager.AddTextParameter("Dynamic Variables", "Vars", "The Dynamically detected variables", GH_ParamAccess.list);

        }




        DynamicScript ds = new DynamicScript();

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Application app = null;
            string script = null;
            bool run = false;

            if (!DA.GetData(1, ref script))
            { return; }

            DA.GetData(2, ref run);
            if (run)
            {

                if (!DA.GetData(0, ref app))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "An Active Indd App Input is Required (app)");
                    return;
                }
                app.RunScript(script, app);

            }

            ds.Script = script;
            DA.SetData(0, ds.Script);
            DA.SetDataList(1, ds.parameters());

        }

        //////
        //////
        //////
        //////
        //////
    

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if ((side == GH_ParameterSide.Output))
                return false;
            else if ((index == 0) || (index == 1) || (index == 2) || (index == 3))
                return false;
            else return true;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if ((side == GH_ParameterSide.Output))
                return false;
            else if ((Params.Input.Count <= 4))
                return false;
            else if ((index == 0) || (index == 1) || (index == 2) || (index == 3))
                return false;
            else return true;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            Param_GenericObject input = new Param_GenericObject();
            Params.RegisterInputParam(input, index);
            createDefaultParamAttributes(input);
            return input;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }
        public void VariableParameterMaintenance()
        {
            ExpireSolution(true);
        }

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Generate Inputs", Menu_DoClick_Generate);
        }

        /// <summary>
        /// The Auto Generate Input Parameters method trigger.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_DoClick_Generate(object sender, EventArgs e)
        {
            const string message = "Warning: This will Automatically Generate dynamic input parameters by parsing the input script for any variables named with the #varName# syntax.\n\nThis will remove all manually created variables!\n\nAre you sure you want to do this?";
            const string caption = "Generating Dynamic Inputs..";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                MessageBox.Show("NO PRESSED");
            }

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("YES PRESSED");
            }
        }

        private static readonly Regex regex_pNames = new Regex(@"^var[A-Z]+$");
        private static readonly Regex regex_IsNumber = new Regex(@"^[-]?\d+(\.\d+)?$");
        private static readonly Regex regex_DynamicParameters = new Regex(@"\#([A-Za-z_]+\w+)\#");

        /// <summary>
        /// Setting the Default attributes of the newly Created Parameter along with a unique NickName string
        /// </summary>
        /// <param name="param">Parameter Object to Set Default values to.</param>
        private void createDefaultParamAttributes(IGH_Param param)
        {
            if (Params.Input.Count > 4)
            {
                List<string> pNames = new List<string>();
                foreach (var p in Params.Input)
                {
                    string nc = p.NickName;
                    if (regex_pNames.IsMatch(nc))
                    {
                        pNames.Add(nc.Substring(3));
                    }
                    else
                    {
                        pNames.Add(nc);
                    }
                }

                param.NickName = "var" + GH_ComponentParamServer.InventUniqueNickname("ABCDEFGHIJKLMNOPQRSTUVWXYZ", pNames);
                param.Name = string.Format("Values of {0}", param.NickName);
                param.Description = "Optional list of text or numbers";
                param.Access = GH_ParamAccess.list;
                param.Optional = true;
            }
        }

        /// <summary>
        /// Setting the Default attributes of the newly Created Parameter with a specified NickName. if the input NickName is taken already, then a unique NickName string will be provided instead.
        /// </summary>
        /// <param name="param">Parameter Object to Set Default values to.</param>
        /// <param name="nickName"></param>
        private void createDefaultParamAttributes(IGH_Param param, string nickName)
        {
            if (Params.Input.Count > 4)
            {
                bool isTaken = Params.Input.Select(i => i.NickName).ToList().Contains(nickName);
                if (!isTaken)
                {
                    List<string> pNames = new List<string>();
                    foreach (var p in Params.Input)
                    {
                        string nc = p.NickName;
                        pNames.Add(nc);
                    }

                    param.NickName = "var" + GH_ComponentParamServer.InventUniqueNickname("ABCDEFGHIJKLMNOPQRSTUVWXYZ", pNames);
                    param.Name = string.Format("Values of {0}", param.NickName);
                }
                param.NickName = nickName;
                param.Name = string.Format("Values of {0}", nickName);
                param.Description = "Optional list of text or numbers";
                param.Access = GH_ParamAccess.list;
                param.Optional = true;
            }
        }


        /// <summary>
        /// Generates a JavaScript Item or List based on individual vs many items in the input List data. The method preserves all standalone numbered values as numbers (int or double) in the output JS string.
        /// </summary>
        /// <param name="list">List of Strings or Numbers; use a list of single item for an individual variable value output.</param>
        /// <returns>A formatted string for use as a JavaScript variable assignment, either a list of values or individual value.</returns>
        public string stringify(List<System.Object> list)
        {
            string val = string.Empty;
            if (list.Count == 1)
            {
                val = (!IsNumber(list[0].ToString())) ? string.Format("\"{0}\"", list[0].ToString()) : string.Format("{0}", list[0]);
            }
            else
            {
                val += "[";

                for (int i = 0; i < list.Count; i++)
                {
                    var item = (!IsNumber(list[i].ToString())) ? string.Format("\"{0}\"", list[i].ToString()) : list[i];
                    if (i == list.Count - 1)
                    {
                        val += string.Format("{0}]", item);
                    }
                    else
                    {
                        val += string.Format("{0},", item);
                    }
                }
            }
            return val;
        }

        public bool IsNumber(string test)
        {
            if (regex_IsNumber.IsMatch(test.ToString().Trim()))
            {
                return true;
            }
            else return false;
        }


        class DynamicScript
        {

            public string Script { get; set; } = string.Empty;

            public DynamicScript()
            {
            }

            public List<string> parameters()
            {
                List<string> collect = new List<string>();

                var matches = regex_DynamicParameters.Matches(Script);
                foreach (Match m in matches)
                {
                    collect.Add(m.Value);
                }
                return collect.Distinct().ToList();
            }


            

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
            get { return new Guid("6B170F4B-A51D-4D70-8987-DC095A00374E"); }

        }
    }
}