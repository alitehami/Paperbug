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
    }
}
