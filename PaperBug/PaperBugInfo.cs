using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace PaperBug
{
    public class PaperBugInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "PaperBug";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Properties.Resources.paperbug_icon_96;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Providing Interoperability with Adobe InDesign";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("d8f51be0-36f1-4b07-b370-44f5f7cc5d3a");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Ali Tehami, Woods Bagot";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "ali.tehami@yahoo.com";
            }
        }
    }
}
