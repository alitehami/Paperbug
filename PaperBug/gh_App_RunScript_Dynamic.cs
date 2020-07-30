using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
            Params.ParameterChanged += Event_Params_ParameterChanged;
            //Params.ParameterNickNameChanged += Event_Params_ParameterChanged
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>

        int coreParamsCount = 4;
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("InDesign App", "app", "Indesign Application Instance", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[0].MutableNickName = false;
            pManager.AddTextParameter("Script", "src", "The Script as a Text input, or provide a Path to the Script File.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "run", "Set True to Run the Script", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("_Generate", "_gen_", "Automatically Generates input parameters by parsing the input script for \nany variables named with the $varName$ syntax.\n\nOnce Set to True then it will Generate all missing dynamic inputs from the script text.", GH_ParamAccess.item, false);

            for (int i = 0; i < coreParamsCount; i++)
            {
                pManager[i].MutableNickName = false;
            }

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Dynamic Script", "ds", "The Dynamic Script", GH_ParamAccess.item);
            pManager.AddTextParameter("Dynamic Variables", "vars", "The Dynamically detected variables", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Result", "r", "The Result of Script Run.\n This can be used to chain Scripts Runs in order. Will turn to True on script run completion.", GH_ParamAccess.item);

        }

        private DynamicScript ds = new DynamicScript();
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            InDesignInterface.Indd_Application app = null;
            string script = null;
            bool run = false;
            bool generate = false;

            //Set the Script Run Result to False by default.
            DA.SetData(2, false);

            if (!DA.GetData(1, ref script))
            { return; }

            DA.GetData(3, ref generate);

            if (generate)
            {
                generateInputParams();
                VariableParameterMaintenance();
            }

            ds.Script = script;
            string dynamicScript = ds.dynamicScript(Params, coreParamsCount, DA);
            DA.SetData(0, dynamicScript);
            DA.SetDataList(1, ds.parameters(true));

            DA.GetData(2, ref run);
            if (run)
            {
                if (!DA.GetData(0, ref app))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "An Active Indd App Input is Required (app)");
                    return;
                }
                else
                {
                    app.RunScript(dynamicScript, app);
                    //Set the Script Run Result to True after completion.
                    DA.SetData(2, true);
                }
            }

        }

        //private int i = 1;
        private void Event_Params_ParameterChanged(object sender, GH_ParamServerEventArgs e)
        {
            //MessageBox.Show(string.Format("param changed firrrred!! \n{0} time  ----> BEFORE Expire()",i));
            VariableParameterMaintenance();
            //MessageBox.Show(string.Format("param changed firrrred!! \n{0} time  ----> AFTER Expire()", i));
            //i++;
        }


        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if ((side == GH_ParameterSide.Output))
                return false;
            else if (index < coreParamsCount)
                return false;
            else return true;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if ((side == GH_ParameterSide.Output))
                return false;
            else if ((Params.Input.Count <= coreParamsCount))
                return false;
            else if (index < coreParamsCount)
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
            //Params.UnregisterParameter(Params.Input[index],true);
            return true;
        }

        public void VariableParameterMaintenance()
        {
            //GH_ComponentParamServer.IGH_SyncObject ighsync = Params.EmitSyncObject();
            Params.OnParametersChanged();
            //Params.Sync(ighsync);
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
            const string message = "This is Providing the same functionality as the '_gen_' component boolean input. It will Automatically Generate dynamic input parameters by parsing the input script for any variables named with the $varName$ syntax. Only variables that are not available yet will be created and added. \n\nDo you want to do this?";
            const string caption = "Generating Dynamic Inputs..";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Warning);

            if (result == DialogResult.No)
            {
                return;
            }

            if (result == DialogResult.Yes)
            {
                ExpireSolution(false);
                generateInputParams();
                VariableParameterMaintenance();
            }
        }

        private void generateInputParams()
        {

            List<string> curInputs = new List<string>();

            for (int i = coreParamsCount; i < Params.Input.Count; i++)
            {
                curInputs.Add(Params.Input[i].NickName);
            }

            foreach (string varInput in ds.parameters(true))
            {
                if (!curInputs.Contains(varInput))
                {
                    IGH_Param newParam = CreateParameter(GH_ParameterSide.Input, Params.Input.Count);
                    createDefaultParamAttributes(newParam, varInput);

                }
            }

        }


        private static readonly Regex regex_pNames = new Regex(@"^var[A-Z]+$");

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


        class DynamicScript
        {
            private static readonly Regex regex_IsNumber = new Regex(@"^[-]?\d+(\.\d+)?$");
            private static readonly Regex regex_DynamicParameters = new Regex(@"\$([A-Za-z_]+\w+)\$");
            public string Script { get; set; } = string.Empty;

            public DynamicScript()
            {
            }

            public List<string> parameters(bool cleaned = false)
            {
                List<string> collect = new List<string>();

                var matches = regex_DynamicParameters.Matches(Script);
                foreach (Match m in matches)
                {
                    if (cleaned)
                    {
                        collect.Add(m.Value.Replace("$", ""));
                    }
                    else
                    {
                        collect.Add(m.Value);
                    }
                }

                return collect.Distinct().ToList();
            }

            public string dynamicScript(GH_ComponentParamServer _Params, int coreParamsCount, IGH_DataAccess _DA)
            {


                string ds = Script;
                Dictionary<string, int> varParamsIndicies = new Dictionary<string, int>();
                for (int i = coreParamsCount; i < _Params.Input.Count; i++)
                {
                    varParamsIndicies.Add(_Params.Input[i].NickName, i);
                }
                foreach (string p in parameters())
                {
                    string cp = p.Replace("$", "");
                    if (varParamsIndicies.ContainsKey(cp))
                    {
                        var data = new List<Object>();
                        int index = varParamsIndicies[cp];
                        if (_DA.GetDataList(index, data))
                        {
                            //MessageBox.Show(stringify(data).ToString());
                            ds = ds.Replace(p, stringify(data));
                        }
                    }

                }
                return ds;
            }

            /// <summary>
            /// Generates a JavaScript Item or List based on individual vs many items in the input List data. The method preserves all standalone numbered values as numbers (int or double) in the output JS string.
            /// </summary>
            /// <param name="list">List of Strings or Numbers; use a list of single item for an individual variable value output.</param>
            /// <returns>A formatted string for use as a JavaScript variable assignment, either a list of values or individual value.</returns>
            public static string stringify(List<System.Object> list)
            {
                string val = string.Empty;
                if (list.Count == 1)
                {
                    bool isboolValue;
                    if (bool.TryParse(list[0].ToString(),out isboolValue))
                    {
                        if (isboolValue)
                        {
                            val = "true";
                        }
                        else
                        {
                            val = "false";
                        }
                    }
                    //if (IsBool(list[0], out val)) { }
                    else
                    {
                        val = (!IsNumber(list[0].ToString())) ? string.Format("\"{0}\"", list[0].ToString()) : string.Format("{0}", list[0]);
                    }
                }
                else
                {
                    val += "[";

                    for (int i = 0; i < list.Count; i++)
                    {
                        string item = string.Empty;


                        bool isboolValue;
                        if (bool.TryParse(list[i].ToString(), out isboolValue))
                        {
                            if (isboolValue)
                            {
                                item = "true";
                            }
                            else
                            {
                                item = "false";
                            }
                        }
                        //if (IsBool(list[i], out item)) { }
                        else
                        {
                            item = (!IsNumber(list[i].ToString())) ? string.Format("\"{0}\"", list[i].ToString()) : list[i].ToString();
                        }


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

            public static bool IsNumber(string test)
            {
                if (regex_IsNumber.IsMatch(test.ToString().Trim()))
                {
                    return true;
                }
                else return false;
            }

            public static bool IsBool(Object test, out string converted)
            {
                if (test is bool)
                {
                    if ((bool)test)
                    {
                        converted = "true";
                    }
                    else
                    {
                        converted = "false";
                    }
                    return true;
                }
                else
                {
                    converted = string.Empty;
                    return false;
                }
            }

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
            get { return new Guid("6B170F4B-A51D-4D70-8987-DC095A00374E"); }

        }

    }
}