using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InDesign;

namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}

namespace PaperBug.InDesignInterface
{
    
    public class Indd_Application
    {
        public InDesign.Application InDesignApp;

        public Indd_Application(Application app) 
        { 
            InDesignApp = app; 
        }

        public Indd_Application() 
        {
            // Create application instance
            Type type = Type.GetTypeFromProgID("InDesign.Application");
            Application application = (Application)Activator.CreateInstance(type);
            application.Activate();

            InDesignApp = application as InDesign.Application; 
        }


        public List<Indd_Document> OpenDocsNames(out List<string> docsNames)
        {
            List< Indd_Document> docs = new List<Indd_Document>();
            Documents curr_docs = InDesignApp.Documents;
            docsNames = new List<string>();
            foreach (Document d in curr_docs)
            {
                Document doc = d as Document;
                docs.Add(new Indd_Document(doc));
                docsNames.Add(doc.Name);
            }

            return docs;
        }



    }
}
