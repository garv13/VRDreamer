using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace VRDreamer
{
    class Monument_Detail_View
    {
        public string Title { get; set; }

        public string Desc { get; set; }

        public double MyLat { get; set; }

        public double MyLon { get; set; }
        public BitmapImage Image { get; set; }
    }
}
