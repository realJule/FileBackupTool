using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileBackupTool
{
    internal class BackupChecker
    {
        List<DetectedFile> files;
        IProgress<DetectedFile> uiProgressCallback;
        bool flagCalculateChecksum;

        public List<DetectedFile> GetDetectedFiles() { return files; }

        public BackupChecker(bool shouldCalculateChecksum, IProgress<DetectedFile> progress)
        {
            files = new List<DetectedFile>();
            uiProgressCallback = progress;
            flagCalculateChecksum = shouldCalculateChecksum;
        }

        public async Task check(string source, string target)
        {
            files = new List<DetectedFile>();
            await checkDirectoryRecursively(source, target);
        }

        private async Task checkDirectoryRecursively(string source, string target)
        {
            await checkForNewOrModifiedFiles(source, target);
            await checkForDeletedOrMovedFiles(source, target);
        }

        private async Task checkForNewOrModifiedFiles(string source, string target)
        {
            foreach (var filepath in getDirectoryFiles(source))
            {
                try
                {
                    DetectedFile file = getCheckedFile(filepath, target);
                    uiProgressCallback.Report(file);
                    if (file.status != FileStatus.OK)
                    {
                        files.Add(file);
                        await Task.Delay(250);
                    }
                }
                catch (IOException e)
                {
                    MessageBox.Show($"Failed to process file {filepath}:\n {e.Message}", "Skipping file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            foreach (var dir in getChildDirectories(source))
            {
                string dirname = Path.GetFileName(dir);
                if (!isDirectoryOnIgnoreList(dirname))
                {
                    await checkForNewOrModifiedFiles(dir, Path.Combine(target, Path.GetFileName(dir)));
                }
            }
        }

        private string[] getDirectoryFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show($"Can not browse directory {path}:\n {e.Message}", "Skipping directory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new string[] { };
            }
        }

        private string[] getChildDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch (UnauthorizedAccessException e)
            {
                MessageBox.Show($"Can not browse directory {path}:\n {e.Message}", "Skipping directory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return new string[] { };
            }
        }

        private bool isDirectoryOnIgnoreList(string dirName)
        {
            List<string> ignoreList = new List<string> { ".git" }; // TODO: Remove/Replace this hard coded ignore list by something more user friendly...
            return ignoreList.IndexOf(dirName) >= 0;
        }

        private DetectedFile getCheckedFile(string filepath, string targetDir)
        {
            var obj = new DetectedFile();
            obj.sourcePath = filepath;
            obj.targetPath = Path.Combine(targetDir, Path.GetFileName(filepath));
            var fileInfo = new FileInfo(filepath);
            obj.bytes = fileInfo.Length;
            obj.modifiedDate = fileInfo.LastWriteTime;
            obj.checksum = getChecksum(filepath);
            obj.status = getStatus(obj);
            return obj;
        }

        string getChecksum(string filename)
        {
            return flagCalculateChecksum ? getMD5Hash(filename) : "-";
        }

        static string getMD5Hash(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        FileStatus getStatus(DetectedFile file)
        {
            if (doesFileExistInBackup(file))
            {
                if (isFileModified(file))
                {
                    return FileStatus.Modified;
                }
                else
                {
                    return FileStatus.OK;
                }
            }
            else
            {
                return FileStatus.Created;
            }
        }

        private bool doesFileExistInBackup(DetectedFile file)
        {
            return File.Exists(file.targetPath);
        }

        private bool isFileModified(DetectedFile file)
        {
            if (flagCalculateChecksum)
            {
                if (file.checksum != getChecksum(file.targetPath))
                {
                    return true;
                }
            }
            else
            {
                var backupFileInfo = new FileInfo(file.targetPath);
                if (file.bytes != backupFileInfo.Length)
                {
                    return true;
                }
                else
                {
                    if (!areModifiedDatesEqual(file.modifiedDate, backupFileInfo.LastWriteTime))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool areModifiedDatesEqual(DateTime first, DateTime other)
        {
            first = first.ToUniversalTime();
            other = other.ToUniversalTime();

            if (first == other)
            {
                return true;
            }

            // Modified date of a copied file may differ (by up to 2 seconds) from it's source file
            // depending on the file system due to rounding differences.
            return Math.Abs((first - other).TotalSeconds) <= 2;
        }

        private async Task checkForDeletedOrMovedFiles(string source, string target)
        {
            foreach (var filepath in Directory.GetFiles(target))
            {
                DetectedFile file = getCheckedBackupFile(filepath, source);
                if (file.status != FileStatus.OK) {
                    file = addDeletedFileOrMarkAsMoved(file);
                    uiProgressCallback.Report(file);
                    await Task.Delay(250);
                }
            }

            foreach (var dir in Directory.GetDirectories(target))
            {
                if (Path.GetFileName(dir) == ".archive") { 
                    continue;
                }
                await checkForDeletedOrMovedFiles(Path.Combine(source, Path.GetFileName(dir)), dir);
            }
        }

        private DetectedFile getCheckedBackupFile(string filepath, string sourceDir)
        {
            var obj = new DetectedFile();
            obj.sourcePath = Path.Combine(sourceDir, Path.GetFileName(filepath));
            obj.targetPath = filepath;
            var fileInfo = new FileInfo(filepath);
            obj.bytes = fileInfo.Length;
            obj.modifiedDate = fileInfo.LastWriteTime;
            obj.checksum = getChecksum(filepath);
            obj.status = doesFileExistInSource(obj) ? FileStatus.OK : FileStatus.Deleted;
            return obj;
        }

        private bool doesFileExistInSource(DetectedFile file)
        {
            return File.Exists(file.sourcePath);
        }

        private DetectedFile addDeletedFileOrMarkAsMoved(DetectedFile file)
        {
            DetectedFile fileDouble = tryFindMovedFile(file);
            if (fileDouble != null)
            {
                fileDouble.status = FileStatus.Moved;
                fileDouble.targetPathMovedFrom = file.targetPath;
                return fileDouble;
            }
            else
            {
                file.status = FileStatus.Deleted;
                files.Add(file);
                return file;
            }
        }

        private DetectedFile? tryFindMovedFile(DetectedFile file)
        {
            foreach (var other_file in files) // TODO: Consider using a hash map for "files" to make this faster (?)
            {
                if (other_file.status == FileStatus.Created)
                {
                    if (flagCalculateChecksum)
                    {
                        if (file.checksum == other_file.checksum)
                        {
                            return other_file;
                        }
                    }
                    else
                    {
                        if ((file.getName() == other_file.getName()) &&
                            (areModifiedDatesEqual(file.modifiedDate, other_file.modifiedDate)) &&
                            (file.bytes == other_file.bytes))
                        {
                            return other_file;
                        }
                    }
                }
            }

            return null;
        }
    }
}
