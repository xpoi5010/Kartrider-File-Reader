using KartLibrary.Consts;
using KartLibrary.File;
using KartLibrary.Tests.Command;
using KartLibrary.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Testing
{
    public abstract class TestIRhoArchiveBase<TFolder, TFile>: TestStage where TFile: IRhoFile where TFolder : IRhoFolder<TFolder, TFile>
    {
        protected abstract IRhoArchive<TFolder, TFile>? BaseArchive { get; }

        protected TFolder? CurrentFolder;

        private Dictionary<string, Process> _openedTmpFiles = new Dictionary<string, Process>();

        protected override void OnExit()
        {
            if (_openedTmpFiles.Count > 0)
            {
                foreach (KeyValuePair<string, Process> tmpFile in _openedTmpFiles)
                {
                    if (!tmpFile.Value.HasExited)
                    {
                        tmpFile.Value.Kill();
                        tmpFile.Value.WaitForExitAsync().Wait(300);
                    }
                    if (tmpFile.Value.HasExited)
                    {
                        System.IO.File.Delete(tmpFile.Key);
                    }
                }
            }
            CurrentFolder = default;
            base.OnExit();
        }

         [Command("ls", "List all contents of current folder or specific folder.")]
        protected CommandExecuteResult commandListFolder(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (BaseArchive is null)
                return new CommandExecuteResult(ResultType.Failure, "It isn't initialized.");
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            TFolder listFolder = CurrentFolder;
            if (argumentQueue.Count > 0)
            {
                string folderName = argumentQueue.PopArgumentString();
                TFolder? findFolder = CurrentFolder.GetFolder(folderName);
                if (findFolder is null)
                    if (CurrentFolder.ContainsFile(folderName))
                        return new CommandExecuteResult(ResultType.Failure, $"\"{folderName}\" is a file, not a folder.");
                    else
                        return new CommandExecuteResult(ResultType.Failure, $"Folder: \"{folderName}\" is not exist in current folder.");
                listFolder = findFolder;
            }
            commandConsole.WriteLine($"\n\tFolder: {listFolder.FullName}\n");
            commandConsole.WriteLine($"  Type      |  Size        |  Name");
            commandConsole.WriteLine($"------------+--------------+------------------");
            foreach (TFolder folder in listFolder.Folders)
                commandConsole.WriteLine($" [ Folder ]                  {folder.Name}");
            foreach (TFile file in listFolder.Files)
                commandConsole.WriteLine($" [  File  ]   {UnitUtility.FormatDataSize(file.Size)}   {file.Name}");
            commandConsole.WriteLine("");
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("cd", "Change current work folder.")]
        protected CommandExecuteResult commandChangeFolder(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (argumentQueue.Count == 0)
                return new CommandExecuteResult(ResultType.Success, "");
            else
            {
                string folderName = argumentQueue.PopArgumentString();
                TFolder? changeFolder = CurrentFolder.GetFolder(folderName);
                if (changeFolder is null)
                    return new CommandExecuteResult(ResultType.Failure, $"Cannot found folder: \"{folderName}\"");
                CurrentFolder = changeFolder;
                return new CommandExecuteResult(ResultType.Success, "");
            }
        }

        [CommandAutoComplete("cd")]
        protected string[] commandAutoComplChangeFolder(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (BaseArchive is null)
                return Array.Empty<string>();
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            if (argumentQueue.Count > 0)
            {
                string findFolderName = argumentQueue.PopArgumentString();
                foreach (TFolder folder in CurrentFolder.Folders)
                    if (folder.Name.StartsWith(findFolderName))
                        suggestions.Add(folder.Name);
            }
            else
            {
                foreach (TFolder folder in CurrentFolder.Folders)
                    suggestions.Add(folder.Name);
            }
            return suggestions.ToArray();
        }

        [Command("pwd", "Print work folder.")]
        protected CommandExecuteResult commandPrintWorkFolder(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (BaseArchive is null)
                return new CommandExecuteResult(ResultType.Failure, "It isn't initialized.");
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            commandConsole.WriteLine($"\nWork folder: {CurrentFolder.FullName}\n");
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("open", "Open specific file")]
        protected CommandExecuteResult commandOpen(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (BaseArchive is null)
                return new CommandExecuteResult(ResultType.Failure, "It isn't initialized.");
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            string fileName = argumentQueue.PopArgumentString();
            TFile? file = CurrentFolder.GetFile(fileName);
            if (file is null)
                return new CommandExecuteResult(ResultType.Failure, $"Cannot found file: {fileName}.");
            if (file.DataSource is null)
                return new CommandExecuteResult(ResultType.Failure, $"file: {fileName} has no any data source.");
            string tmpFileName = $"{Path.GetTempFileName()}_{file.Name}";
            using (FileStream outFileStream = new FileStream(tmpFileName, FileMode.Create))
            {
                file?.DataSource?.WriteTo(outFileStream);
            }
            if (OperatingSystem.IsWindows())
            {
                Process openFileProc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "explorer",
                        Arguments = $"\"{tmpFileName}\""
                    }
                };
                openFileProc.Start();
                _openedTmpFiles.Add(tmpFileName, openFileProc);
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
            {
                Process openFileProc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = "open",
                        Arguments = $"\"{tmpFileName}\"",
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                    }
                };
                openFileProc.Start();
                _openedTmpFiles.Add(tmpFileName, openFileProc);
            }
            else
            {
                return new CommandExecuteResult(ResultType.Failure, $"This command is not available on {Environment.OSVersion.VersionString}");
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [CommandAutoComplete("open")]
        protected string[] commandAutoComplOpen(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (BaseArchive is null)
                return Array.Empty<string>();
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            if (argumentQueue.Count > 0)
            {
                string findFileName = argumentQueue.PopArgumentString();
                foreach (TFile file in CurrentFolder.Files)
                    if (file.Name.StartsWith(findFileName))
                        suggestions.Add(file.Name);
            }
            else
            {
                foreach (TFile file in CurrentFolder.Files)
                    suggestions.Add(file.Name);
            }
            return suggestions.ToArray();
        }

        [Command("extract", "Extract specific file")]
        protected CommandExecuteResult commandExtract(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (BaseArchive is null)
                return new CommandExecuteResult(ResultType.Failure, "It isn't initialized.");
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            string fileName = argumentQueue.PopArgumentString();
            string toPath = argumentQueue.PopArgumentString();
            TFile? file = CurrentFolder.GetFile(fileName);
            if (file is null)
                return new CommandExecuteResult(ResultType.Failure, $"Cannot found file: {fileName}.");
            if (file.DataSource is null)
                return new CommandExecuteResult(ResultType.Failure, $"file: {fileName} has no any data source.");
            if (!Directory.Exists(toPath))
                return new CommandExecuteResult(ResultType.Failure, $"out path is not exist.");
            string outFileName = Path.Combine(toPath, fileName);
            using (FileStream outFileStream = new FileStream(outFileName, FileMode.Create))
            {
                file?.DataSource?.WriteTo(outFileStream);
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [CommandAutoComplete("extract")]
        protected string[] commandAutoComplExtract(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (BaseArchive is null)
                return Array.Empty<string>();
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            if (argumentQueue.Count > 0)
            {
                string findFileName = argumentQueue.PopArgumentString();
                foreach (TFile file in CurrentFolder.Files)
                    if (file.Name.StartsWith(findFileName))
                        suggestions.Add(file.Name);
            }
            else
            {
                foreach (TFile file in CurrentFolder.Files)
                    suggestions.Add(file.Name);
            }
            return suggestions.ToArray();
        }

    }
}
