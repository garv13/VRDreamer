using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace VRDreamer
{
    class MainPage_Listing
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public string Id { get; set; }

        public Colors color { get; set; }
        public string Type { get; set; }
        public BitmapImage Image { get; set; }
    }
}
