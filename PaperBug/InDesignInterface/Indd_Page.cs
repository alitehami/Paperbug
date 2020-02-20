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

        public List<PageItem> _AllPageItemsOfType(Page p, out List<string> names, string t = "test")
        {
            names = new List<string>();
            List<PageItem> pi = new List<PageItem>();
            foreach (PageItem i in p.AllPageItems)
            {
                PageItem pItem = i as PageItem;
                string pType = pItem.Name;
                names.Add(pType);
                pi.Add(i as PageItem);
            }
           
            return  pi;
        }



    }
}
