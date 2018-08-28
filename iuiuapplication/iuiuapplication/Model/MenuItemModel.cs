using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class MenuItemModel
    {
        public string menu_image { get; set; }
        public string menu_label { get; set; }
        public MenuItemModel(string img,string label)
        {
            menu_image = img;
            menu_label = label;
        }
    }
}
