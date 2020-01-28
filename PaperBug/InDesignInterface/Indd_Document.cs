using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InDesign;

namespace PaperBug.InDesignInterface
{
    public class Indd_Document
    {
        public InDesign.Document InDesignDoc;

        public Indd_Document(Document doc)
        {
            InDesignDoc = doc as Document;
        }

        public List<Indd_Page> pages()
        {
            List<Indd_Page> docPages = new List<Indd_Page>();

            foreach (Page p in InDesignDoc.Pages)
            {
                Indd_Page page = new Indd_Page(p as Page);
                docPages.Add(page);
            }

            return docPages;
        }


        public void CloseDocument(Indd_Document doc, bool save)
        {
            idSaveOptions saveOp = save ? idSaveOptions.idYes : idSaveOptions.idNo;

            //System.IO.Directory.CreateDirectory(Environment.SpecialFolder.Desktop.ToString() + "\\PaperBug_Temp\\");
            string desktop = @"C:\Users\dxbaat\Desktop\PaperBug_Temp\" + doc.InDesignDoc.Name + ".indd";

            if (save)
                doc.InDesignDoc.Close(saveOp, desktop);
            else
                doc.InDesignDoc.Close(saveOp);
        }

    }
}
