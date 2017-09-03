using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    interface IDisplayable
    {
        event EventHandler<IndexChangeEvntArg> OnIndexChange;
        void Display(Container container);
        void DisplayItem(int index);
        void DisplayInfo(FileSystemInfo item);
    }
}
