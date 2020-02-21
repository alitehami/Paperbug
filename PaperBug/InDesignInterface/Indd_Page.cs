using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InDesign;

namespace PaperBug.InDesignInterface
{
    public class Indd_Page
    {

        public InDesign.Page InDesignPage;

        public Indd_Page(Page page)
        {
            InDesignPage = page as Page;
        }

        public void PlaceImage(string path) 
        {
            InDesignPage.Place(path);
        }


        public List<Indd_PageItem> _AllPageItemsOfType(out List<string> names, string t = "test")
        {
            names = new List<string>();
            List<Indd_PageItem> pi = new List<Indd_PageItem>();
            foreach (PageItem i in InDesignPage.AllPageItems)
            {
                PageItem pItem = i as PageItem;
                string pType = pItem.Name;
                names.Add(pType);
                pi.Add(new Indd_PageItem(i));
            }

            return pi;
        }

    }
}
