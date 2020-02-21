using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InDesign;

namespace PaperBug.InDesignInterface
{
    public class Indd_PageItem
    {

        public InDesign.PageItem InDesignItem;

        public Indd_PageItem(PageItem pageItem)
        {
            InDesignItem = pageItem as PageItem;
        }

    }
}
