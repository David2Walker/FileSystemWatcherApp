using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace walkerFileSystemWatcher
{
    class watchedObject
    {
        private string watchingPath { get; set; }
        private string fileName { get; set; }
        private string filePath { get; set; }
        private string fileEvent { get; set; }
        private DateTime date { get; set; }
        private DateTime time { get; set; }
    }
}
