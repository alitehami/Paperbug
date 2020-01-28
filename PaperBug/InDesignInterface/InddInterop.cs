using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InDesign;

namespace PaperBug.InDesignInterface
{
    class InddInterop
    {
        public static Application Run()
        {

            // Create application instance
            Type type = Type.GetTypeFromProgID("InDesign.Application");
            Application application = (Application)Activator.CreateInstance(type);
            //Application appInst = new InDesign.Application();
            application.Activate();

            return application;

            //return appInst;
        }

        public static List<string> OpenDocsNames() 
        {
            //Application app = null;
            //Type oType = Type.GetTypeFromProgID("InDesign.Application");
            //if (oType != null)
            //{
            //    app = System.Runtime.InteropServices.Marshal.GetActiveObject("InDesign.Application") as InDesign.Application;
            //}
            //else return null;

            Type type = Type.GetTypeFromProgID("InDesign.Application");
            Application app = (Application)Activator.CreateInstance(type);

            List<string> docsNames = new List<string>();  
            Documents docs = app.Documents;

            foreach (Document d in docs)
            {
                Document doc = d as Document;
                docsNames.Add(doc.Name);
            }

            return docsNames;
        }
    }
}
