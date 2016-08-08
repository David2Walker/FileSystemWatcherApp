using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace walkerFileSystemWatcher
{
    class watchedObject
    {
        public string watchingPath { get; set; }
        public string userName { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public string fileEvent { get; set; }
        public string date { get; set; }
        public string time { get; set; }

        public watchedObject(string watching, string user,string name, string path, string fileevent, string date, string time)
        {
            this.watchingPath = watching;
            this.userName = user;
            this.fileName = name;
            this.filePath = path;
            this.fileEvent = fileevent;
            this.date = date;
            this.time = time;
        }

        public override string ToString()
        {
            return "Watching: " + watchingPath + 
                   " User: " + userName +
                   " Filename: " + fileName + 
                   " Path: " + filePath + 
                   " Event: " + fileEvent + 
                   " Date: " + date + 
                   " Time: " + time;
        }



    }
}
