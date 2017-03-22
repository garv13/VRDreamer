using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace VRDreamer
{
    class PointerView
    {

        public string Serial { get; set; }
        public string Title { get; set; }
        public StorageFile File { get; set; }
        public string Desc { get; set; }
    }
}
