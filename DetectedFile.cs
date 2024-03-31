using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileBackupTool
{
    internal enum FileStatus
    {
        OK,       // File is in-sync with backup.
        Created,  // File is not present in backup.
        Modified, // File is present in backup, but content is different.
        Deleted,  // File is present in backup, but not in source.
        Moved,    // File would be 'Created' but has the same checksum of another 'Deleted' file.
        Ignored,  // File is present in source but shall not be processed for backup.
    }

    internal class DetectedFile
    {
        public string sourcePath;
        public string targetPath;
        public string targetPathMovedFrom;
        public FileStatus status;
        public string checksum;
        public long bytes;
        public DateTime modifiedDate;

        public string getName()
        {
            return Path.GetFileName(sourcePath);
        }

        public string getUiKey()
        {
            return sourcePath + checksum;
        }
    }
}
