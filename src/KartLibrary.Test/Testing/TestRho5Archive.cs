using KartLibrary.Consts;
using KartLibrary.File;
using KartLibrary.Tests.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Testing
{
    public class TestRho5Archive : TestIRhoArchiveBase<Rho5Folder, Rho5File>
    {
        private Rho5Archive? _rho5Archive;
        private string _rho5DataPackName;
        private CountryCode _clientRegion;
        
        protected override IRhoArchive<Rho5Folder, Rho5File>? BaseArchive => _rho5Archive;

        public TestRho5Archive()
        {
            addStep("Regenerate Rho5Archive.", () =>
            {
                if(_rho5Archive is null)
                {
                    throw new Exception("It is not initialized.");
                }
                else
                {
                    string storePath = Path.Combine(Environment.CurrentDirectory, "rho5_gen");
                    if(!Directory.Exists(storePath))
                        Directory.CreateDirectory(storePath);
                    _rho5Archive.Save(storePath, _rho5DataPackName, _clientRegion, SavePattern.AlwaysRegeneration);
                }
            });
        }

        protected override void OnExit()
        {
            _rho5Archive?.Dispose();
            _rho5Archive = null;
            base.OnExit();
        }

        [Command("init", "init Rho5Archive.")]
        private CommandExecuteResult commandInit(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_rho5Archive is not null)
            {
                return new CommandExecuteResult(ResultType.Failure, "Please close current Rho5Archive before initializing new Rho5Archive instance.");
            }
            else
            {
                string dataPackPath = argumentQueue.PopArgumentString();
                string dataPackName = argumentQueue.PopArgumentString();
                string clientRegion = argumentQueue.PopArgumentString();
                if(!Path.Exists(dataPackPath))
                {
                    return new CommandExecuteResult(ResultType.Failure, $"Cannot found path: {dataPackPath}.");
                }
                CountryCode clientRegionCC;
                if(!Enum.TryParse<CountryCode>(clientRegion.ToUpper(), out clientRegionCC))
                {
                    return new CommandExecuteResult(ResultType.Failure, $"Cannot found country code: {clientRegion.ToUpper()}");
                }
                _rho5Archive = new Rho5Archive();
                _rho5Archive.Open(dataPackPath, dataPackName, clientRegionCC);
                _rho5DataPackName = dataPackName;
                _clientRegion = clientRegionCC;
                return new CommandExecuteResult(ResultType.Success, "");
            }
        }

        [Command("close", "")]
        private CommandExecuteResult commandClose(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (_rho5Archive is not null)
            {
                _rho5Archive.Dispose();
                _rho5Archive = null;
                CurrentFolder = null;
            }
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("modify", "")]
        private CommandExecuteResult commandModify(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            if (BaseArchive is null)
                return new CommandExecuteResult(ResultType.Failure, "It isn't initialized.");
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            string modifiyTarget = argumentQueue.PopArgumentString();
            string modifiySource = argumentQueue.PopArgumentString();
            Rho5File? file = CurrentFolder.GetFile(modifiyTarget);
            if(file is null)
                return new CommandExecuteResult(ResultType.Failure, $"Current folder is not exist file: {modifiyTarget}.");
            if(!System.IO.File.Exists(modifiySource))
                return new CommandExecuteResult(ResultType.Failure, $"Source file is not exist.");
            file.DataSource = new FileDataSource(modifiySource);
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [CommandAutoComplete("modify")]
        protected string[] commandAutoComplModify(CommandArgumentQueue argumentQueue)
        {
            List<string> suggestions = new List<string>();
            if (BaseArchive is null)
                return Array.Empty<string>();
            if (CurrentFolder is null)
                CurrentFolder = BaseArchive.RootFolder;
            if (argumentQueue.Count > 0)
            {
                string findFileName = argumentQueue.PopArgumentString();
                foreach (Rho5File file in CurrentFolder.Files)
                    if (file.Name.StartsWith(findFileName))
                        suggestions.Add(file.Name);
            }
            else
            {
                foreach (Rho5File file in CurrentFolder.Files)
                    suggestions.Add(file.Name);
            }
            return suggestions.ToArray();
        }
    }
}
