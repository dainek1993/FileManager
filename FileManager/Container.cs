using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManager
{
    class Container
    {
        private List<FileSystemInfo> fileSystemInfoItems = new List<FileSystemInfo>();
        public string Path { get; private set; }

        public int Count => fileSystemInfoItems.Count;

        public Container()
        {
            Path = null;
            LoadDriveInfo();
        }

        public Container(string path)
        {
            Path = path;
            LoadFSItems(path);
        }

        private void LoadFSItems(string path)
        {
            if (fileSystemInfoItems.Count != 0)
                fileSystemInfoItems.Clear();

            DirectoryInfo parentDirect = Directory.GetParent(path);
            fileSystemInfoItems.Add(parentDirect);

            string[] directoryInfos = Directory.GetDirectories(path);
            foreach (var di in directoryInfos)
                fileSystemInfoItems.Add(new DirectoryInfo(di));

            string[] fileInfos = Directory.GetFiles(path);
            foreach (var fi in fileInfos)
                fileSystemInfoItems.Add(new FileInfo(fi));
        }

        private void LoadDriveInfo()
        {
            if (fileSystemInfoItems.Count != 0)
                fileSystemInfoItems.Clear();

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var di in drives)
            {
                if(di.IsReady)
                    fileSystemInfoItems.Add(di.RootDirectory);
            }
        }

        //Indexer
        public FileSystemInfo this[int index]
        {
            get { return fileSystemInfoItems[index]; }
        }

        private void Clear()
        {
            fileSystemInfoItems.Clear();
        }

        public void Update()
        {
            Clear();
            LoadFSItems(Path);
        }
    }
}
