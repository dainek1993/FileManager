using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller c = new Controller();
            Manager m = new Manager(c);
            m.BeginManage();


        }
    }
}
