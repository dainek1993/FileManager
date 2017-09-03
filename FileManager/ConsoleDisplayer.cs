using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class ConsoleDisplayer : IDisplayable
    {
        public event EventHandler<IndexChangeEvntArg> OnIndexChange;

        private int xCoord;
        private const int visibleItemsCount = 20;
        private int firstVisibleItemIndex = 0;
        private Controller controller;
        private int activeItemIndex = 0;
        private Container dirContainer;

        public ConsoleDisplayer(int xCoord, Controller controller, Container dirContainer)
        {
            this.xCoord = xCoord;
            Console.CursorVisible = false;
            this.controller = controller;
            this.dirContainer = dirContainer;
        }

        private void Controller_KeyKlick(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    ScrollUp();
                    OnIndexChange(this, new IndexChangeEvntArg(activeItemIndex, dirContainer));
                    Display(dirContainer);
                    break;
                case ConsoleKey.DownArrow:
                    ScrollDown();
                    OnIndexChange(this, new IndexChangeEvntArg(activeItemIndex, dirContainer));
                    Display(dirContainer);
                    break;
            }
        }

        private void ScrollDown()
        {
            if (activeItemIndex >= firstVisibleItemIndex + visibleItemsCount - 1)
            {
                firstVisibleItemIndex += 1;
                if (firstVisibleItemIndex + visibleItemsCount >= dirContainer.Count)
                {
                    firstVisibleItemIndex = dirContainer.Count - visibleItemsCount;
                }
                activeItemIndex = firstVisibleItemIndex + visibleItemsCount - 1;
            }
            else
            {
                if (activeItemIndex >= dirContainer.Count - 1)
                {
                    return;
                }
                activeItemIndex += 1;
            }
        }

        private void ScrollUp()
        {
            if (activeItemIndex <= firstVisibleItemIndex)
            {
                firstVisibleItemIndex -= 1;
                if (firstVisibleItemIndex < 0)
                    firstVisibleItemIndex = 0;
                activeItemIndex = firstVisibleItemIndex;
            }
            else
            {
                activeItemIndex -= 1;
            }
        }


        public void SetKeyProcessing()
        {
            controller.KeyKlick += Controller_KeyKlick;
        }

        public void StopKeyProcessing()
        {
            controller.KeyKlick -= Controller_KeyKlick;
        }

        public void Display(Container container)
        {
            int visibleCount = container.Count < visibleItemsCount ? container.Count : visibleItemsCount + firstVisibleItemIndex;
            dirContainer = container;
            ConsoleClear();
            for (int i = firstVisibleItemIndex; i < visibleCount; i++)
            {
                if (i == activeItemIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                if (container[i] != null)
                {
                    DisplayItem(i);
                }
                Console.ResetColor();
            }
        }

        public void Display(Container container, int activeIndex)
        {
            this.activeItemIndex = activeIndex;
            Display(container);
        }

        public void ConsoleClear()
        {
            ConsoleHelper.ClearRect(xCoord, 0, 20, 58);
        }

        public void Update(Container container)
        {
            ConsoleClear();
            string path = container.Path;
            if (path != null)
                container = new Container(path);
            else
                container = new Container();
            Display(container);
        }

        public void DisplayItem(int index)
        {
            int onDisplayIndex = index - firstVisibleItemIndex;
            if (index < 0 || index > dirContainer.Count)
                throw new Exception("Out of Range");
            Console.SetCursorPosition(xCoord, onDisplayIndex);
            if (dirContainer.Path != null && index == 0)
                Console.Write("..");
            else
            {
                string name = dirContainer[index].Name.Length > 30 ? new String(dirContainer[index].Name.Take(30).ToArray()) : dirContainer[index].Name.Split('.').First();
                Console.Write(name);
                Console.SetCursorPosition(xCoord + 50, onDisplayIndex);
                if (dirContainer[index] is DirectoryInfo)
                    Console.Write("<dir>");
                else
                    Console.Write("<" + ((FileInfo)dirContainer[index]).Extension + ">");
            }

        }

        public void DisplayInfo(FileSystemInfo fsi)
        {
            ConsoleClear();
            List<string> info = new List<string>();
             if (fsi is FileInfo)
             {
                info.Add("Name: " + fsi.Name);
                info.Add("Extention " + fsi.Extension);
                info.Add("Size " + ((FileInfo)fsi).Length);
             }
            if (fsi is DirectoryInfo)
            {
                info.Add("Name: " + fsi.Name);
                info.Add("Size: " + DirSize((DirectoryInfo)fsi) + "b");                
            }

            for (int i = 0; i < info.Count; i++)
            {
                Console.SetCursorPosition(xCoord, i + 1);
                Console.Write(info[i]);
            }
            Console.ReadKey();
        }

        private long DirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis = d.GetFiles();
            try
            {
                foreach (FileInfo fi in fis)
                {
                    size += fi.Length;
                }
                DirectoryInfo[] dis = d.GetDirectories();
                foreach (DirectoryInfo di in dis)
                {
                    size += DirSize(di);
                }
            }
            catch (Exception e)
            {
                ConsoleHelper.ShowMessage(0, 38, e.Message);
            }

            return (size);
        }
    }
}
