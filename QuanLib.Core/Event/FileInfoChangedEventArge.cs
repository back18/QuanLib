using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core.Event
{
    public class FileInfoChangedEventArge : EventArgs
    {
        public FileInfoChangedEventArge(FileInfo oldFileInfo, FileInfo newFileInfo)
        {
            OldFileInfo = oldFileInfo;
            NewFileInfo = newFileInfo;
        }

        public FileInfo OldFileInfo { get; }

        public FileInfo NewFileInfo { get; }
    }
}
