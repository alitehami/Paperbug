using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InDesign;



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


        public Indd_Document OpenDocumentFromPath(string path, out string name)
        {
            Document doc = InDesignApp.Open(path) as Document;
            name = doc.Name;
            return new Indd_Document(doc);
        }

        public List<Indd_Document> OpenedDocuments(out List<string> docsNames)
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

        public void RunScript(string path, Indd_Application app) 
        {
            if (app != null)
            {
                InDesignApp.Activate();
                InDesignApp.DoScript(path,idScriptLanguage.idJavascript);
            }
        }


        public Indd_Document CreateNewDocument()
        {
            InDesignApp.ViewPreferences.HorizontalMeasurementUnits = idMeasurementUnits.idMillimeters;
            InDesignApp.ViewPreferences.VerticalMeasurementUnits = idMeasurementUnits.idMillimeters;

            Document doc = InDesignApp.Documents.Add(true, InDesignApp.DocumentPresets.FirstItem()) as Document;
            doc.DocumentPreferences.FacingPages = false;
            doc.DocumentPreferences.PagesPerDocument = 10;
            doc.DocumentPreferences.PageWidth = 500;
            doc.DocumentPreferences.PageHeight= 500;

            return new Indd_Document(doc);
        }






    }
}
