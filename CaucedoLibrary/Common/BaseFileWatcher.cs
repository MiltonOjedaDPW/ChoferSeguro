using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caucedo.Base.FileWatcher
{
    public class FileWatcherParams
    {
        public string Filter { get; set; }
        public string SourceFolder { get; set; }
        public string WorkFolder { get; set; }
        public string DestFolder { get; set; }
        public string User { get; set; }
        public bool ProcessExistingFiles { get; set; } = false;
        public Action<string, string> onNewFile { get; set; }
        public Action<object, string> LogEvents { get; set; }
    }

    public class BaseFileWatcher
    {
        FileWatcherParams Parameters { get; set; }
        FileSystemWatcher fsw;
        public BaseFileWatcher(FileWatcherParams parameters)
        {
            Parameters = parameters;
        }
        public void StartWatching()
        {
            var sourceFolder = Parameters.SourceFolder;
            fsw = new FileSystemWatcher(sourceFolder);
            fsw.Created += fileCreated;
            fsw.Changed += fileCreated;
            fsw.Error += onFileWatcherError;             
            fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            fsw.Filter = Parameters.Filter;
            fsw.EnableRaisingEvents = true;
            if (Parameters.ProcessExistingFiles)
            {
                var files = Directory.GetFiles(Parameters.SourceFolder, Parameters.Filter);
                if (files.Length > 0)
                {
                    Parameters.LogEvents($"Found {files.Length} existing files on {sourceFolder}/{Parameters.Filter}", "INFO");
                    foreach (var file in files)
                    {
                        processFile(file);
                    }
                }
            }
            Parameters.LogEvents($"Started watching {sourceFolder}/{Parameters.Filter}", "INFO");
            
        }
        public void StopWatching()
        {
            fsw.EnableRaisingEvents = false;
            Parameters.LogEvents("STOPPED watching", "INFO");
            fsw = null;
        }
        public static bool IsFileReady(string sFilename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void onFileWatcherError(object sender, ErrorEventArgs e)
        {
            Parameters.LogEvents(e.GetException(), "ERROR");
        }
        private void fileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                var file = e.FullPath;
                Console.WriteLine("Processing ::" + file);
                processFile(file);
            }
            catch (Exception ex)
            {
                Parameters.LogEvents(ex, "ERROR");
            }
        }

        private void processFile(string file)
        {
            var fileName = Path.GetFileName(file);
            var workFile = Path.Combine(Parameters.WorkFolder, fileName);
            var destFile = Path.Combine(Parameters.DestFolder, fileName);
            Parameters.LogEvents(file, "INFO");
            while (!IsFileReady(file)) { }
            if (File.Exists(workFile))
            {
                Parameters.LogEvents($"delete {workFile}", "INFO");
                File.Delete(workFile);
            }
            File.Move(file, workFile);
            Parameters.LogEvents($"Moved {file} to {workFile}", "INFO");
            handleFileCreation(workFile);
            if (File.Exists(destFile))
            {
                Parameters.LogEvents($"delete {destFile}", "INFO");
                File.Delete(destFile);
            }
            File.Move(workFile, destFile);
            if (File.Exists(destFile))
            {
                Parameters.LogEvents($"delete {destFile}", "INFO");
                File.Delete(destFile);
            }
            Parameters.LogEvents($"Done {file}", "INFO");
        }

        protected void handleFileCreation(string workFile)
        {
            Parameters.onNewFile(workFile, Parameters.User);
        }
    }
}
