using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileBackupTool
{
    internal class BackupUpdater
    {
        IProgress<DetectedFile> uiProgressCallback;

        public BackupUpdater(IProgress<DetectedFile> progress)
        {
            uiProgressCallback = progress;
        }

        public async Task run(List<DetectedFile> filesToUpdate)
        {
            await Task.Run(async () =>
            {
                foreach (var file in filesToUpdate)
                {
                    processFile(file);
                    uiProgressCallback.Report(file);
                    await Task.Delay(250);
                }
            });
        }

        private void processFile(DetectedFile file)
        {
            switch (file.status)
            {
                case FileStatus.Created:
                    ensureTargetDirectoryRecursive(Path.GetDirectoryName(file.targetPath));
                    File.Copy(file.sourcePath, file.targetPath);
                    break;
                case FileStatus.Moved:
                    ensureTargetDirectoryRecursive(Path.GetDirectoryName(file.targetPath));
                    File.Move(file.targetPathMovedFrom, file.targetPath);
                    removeTargetDirectoryRecursiveIfEmpty(Path.GetDirectoryName(file.targetPathMovedFrom));
                    break;
                case FileStatus.Deleted:
                    {
                        File.Move(file.targetPath, getArchiveFilePath(file));
                    }
                    break;
                case FileStatus.Modified:
                    {
                        File.Move(file.targetPath, getArchiveFilePath(file));
                        File.Copy(file.sourcePath, file.targetPath, true);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ensureTargetDirectoryRecursive(string path)
        {
            if (!Directory.Exists(path))
            {
                var parent = Path.GetDirectoryName(path);
                if (!Directory.Exists(parent))
                {
                    ensureTargetDirectoryRecursive(parent);
                }
                Directory.CreateDirectory(path);
            }
        }

        private void removeTargetDirectoryRecursiveIfEmpty(string path)
        {
            if (isDirectoryEmpty(path))
            {
                Directory.Delete(path);
                removeTargetDirectoryRecursiveIfEmpty(Path.GetDirectoryName(path));
            }
        }

        private bool isDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private string getArchiveFilePath(DetectedFile file)
        {
            var archiveDir = ensureArchiveDirectoryAndReturnPath(file);
            var filename = Path.GetFileNameWithoutExtension(file.targetPath) + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            return Path.Combine(archiveDir, filename + Path.GetExtension(file.targetPath));
        }

        private string ensureArchiveDirectoryAndReturnPath(DetectedFile file)
        {
            var archiveDir = Path.Combine(Path.GetDirectoryName(file.targetPath), ".archive");
            if (!Directory.Exists(archiveDir))
            {
                Directory.CreateDirectory(archiveDir);
            }
            return archiveDir;
        }
    }
}
