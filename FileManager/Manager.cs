using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace FileManager
{
    
    class Manager
    {
        private List<Container> containers = new List<Container>();
        private List<IDisplayable> displaers = new List<IDisplayable>();
        private Container leftContainer;
        private Container rightContainer;
        private Controller ctr;
        private IDisplayable leftDisplayer;
        private IDisplayable rightDisplayer;
        private int activeContainerIndex = 0;
        private int activeItemIndex = 0;
        private FileSystemInfo buffer;

        static Manager()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);
            ConsoleHelper.PrintCenterLine(120, 40);
            ConsoleHelper.PrintHorizontalLine(120, 40);
            ConsoleHelper.PrintKeyInfo(0, 35);
        }

        public Manager(Controller control)
        {
            ctr = control;

            control.KeyKlick += Control_KeyKlick;
            leftContainer = new Container();
            rightContainer = new Container();
            containers.Add(leftContainer);
            containers.Add(rightContainer);

            leftDisplayer = new ConsoleDisplayer(2, ctr, leftContainer);
            rightDisplayer = new ConsoleDisplayer(62, ctr, rightContainer);
            displaers.Add(leftDisplayer);
            displaers.Add(rightDisplayer);

            displaers[activeContainerIndex].OnIndexChange += Displayer_OnIndexChange;
            (displaers[activeContainerIndex] as ConsoleDisplayer).SetKeyProcessing();
        }


        private void Displayer_OnIndexChange(object sender, IndexChangeEvntArg e)
        {
            activeItemIndex = e.Index;
        }

        private void Control_KeyKlick(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.Enter:
                    ActionOfFileSystemInfo();
                    break;
                case ConsoleKey.Tab:
                    ChangeActiveContainer();
                    break;
                case ConsoleKey.Escape:
                    break;
                case ConsoleKey.F1:
                    Copyring();
                    break;
                case ConsoleKey.F2:
                    Paste();
                    break;
                case ConsoleKey.F3:
                    Rename();
                    break;
                case ConsoleKey.F4:
                    PrintInfo();
                    break;
                case ConsoleKey.F5:
                    DisplayDisks();
                    break;
                case ConsoleKey.F6:
                    CreateFolder();
                    break;
                case ConsoleKey.Delete:
                    //Delete();
                    break;
            }
        }

        public void BeginManage()
        {
            leftDisplayer.Display(leftContainer);
            rightDisplayer.Display(rightContainer);
            while (true)
            {
                ctr.Check();
            }
        }

        public void ActionOfFileSystemInfo()
        {
            FileSystemInfo activeItem = containers[activeContainerIndex][activeItemIndex];
            if (activeItem != null)
            {
                if (activeItem is DirectoryInfo)
                {
                   containers[activeContainerIndex] = new Container(activeItem.FullName);
                   ((ConsoleDisplayer)displaers[activeContainerIndex]).Display(containers[activeContainerIndex], 1);
                }
                else
                {
                    Process.Start(((FileInfo)activeItem).FullName);
                }
            }
        }

        private void ChangeActiveContainer()
        {
            displaers[activeContainerIndex].OnIndexChange -= Displayer_OnIndexChange;
            (displaers[activeContainerIndex] as ConsoleDisplayer).StopKeyProcessing();
            activeContainerIndex = (activeContainerIndex + 1) >= containers.Count ? 0 : activeContainerIndex + 1;
            displaers[activeContainerIndex].OnIndexChange += Displayer_OnIndexChange;
            (displaers[activeContainerIndex] as ConsoleDisplayer).SetKeyProcessing();

        }


        private void CopyDirectory(string sourceDirName, string destDirName)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }
        }

        public void Copyring()
        {
            if (containers[activeContainerIndex].Path == null)
                return;
            buffer = containers[activeContainerIndex][activeItemIndex];
        }

        private void Paste()
        {
            if (buffer == null)
                return;
            string destPath = containers[activeContainerIndex].Path;
            DirectoryInfo di = new DirectoryInfo(destPath);
            if (buffer is FileInfo)
            {
                string fullName = di.GetFiles().Select(f => f.Name).Contains(buffer.Name) ? Path.Combine(destPath, buffer.Name.Split('.').First() + "1" + buffer.Extension) : Path.Combine(destPath, buffer.Name);
                File.Copy(buffer.FullName, fullName);
            }
            else
            {
                string fullName = di.GetDirectories().Select(d => d.Name).Contains(buffer.Name) ? Path.Combine(destPath, buffer.Name + "_1") : Path.Combine(destPath, buffer.Name);
                CopyDirectory(buffer.FullName, fullName);
            }
            containers[activeContainerIndex].Update();
            UpdateActiveContainer();
        }

        //private void Delete()
        //{
        //    if (containers[activeContainerIndex].Path == null)
        //        return;
        //    FileSystemInfo fsi = containers[activeContainerIndex][activeItemIndex];
        //    fsi.Delete();
        //    UpdateActiveContainer();
        //}

        private void CreateFolder()
        {
            if (containers[activeContainerIndex].Path == null)
                return;

            string destPath = containers[activeContainerIndex].Path;
            string dirName = GetName("new folder");

            try
            {
                string dirFullName = Path.Combine(destPath, dirName);
                DirectoryInfo dir = new DirectoryInfo(dirFullName);
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch (Exception e)
            {
                ConsoleHelper.ShowMessage(1, 38, e.Message);
            }

            UpdateActiveContainer();
        }

        private string GetName(string deffaultName)
        {
            string name = ConsoleHelper.GetString(1, 38);
            return name.Length == 0 ? deffaultName : name;

        }
    

        private void Rename()
        {
            if (containers[activeContainerIndex].Path == null)
                return;

            FileSystemInfo fsi = containers[activeContainerIndex][activeItemIndex];
            string newName = GetName("new name");
            string newFullName = Path.Combine(containers[activeContainerIndex].Path, newName);

            if (fsi is FileInfo)
                ((FileInfo)fsi).MoveTo(newFullName);
            else
                ((DirectoryInfo)fsi).MoveTo(newFullName);

            UpdateActiveContainer();
        }

        private void PrintInfo()
        {
            displaers[activeContainerIndex].DisplayInfo(containers[activeContainerIndex][activeItemIndex]);
            displaers[activeContainerIndex].Display(containers[activeContainerIndex]);
        }

        private void DisplayDisks()
        {
            containers[activeContainerIndex] = new Container();
            ((ConsoleDisplayer)displaers[activeContainerIndex]).Display(containers[activeContainerIndex], 0);
        }

        private void UpdateActiveContainer()
        {
            displaers[activeContainerIndex].Display(new Container(containers[activeContainerIndex].Path));
        }
    }

}
