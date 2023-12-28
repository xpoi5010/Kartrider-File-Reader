using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public class CommandAutoCompleteInfo
    {
        public string CommandName { get; init; }

        public CommandAutoCompleteFunc CommandAutoCompleteFunction { get; init; }

        public CommandAutoCompleteInfo(string commandName, CommandAutoCompleteFunc commandAutoCompleteFunction) 
        {
            CommandName = commandName;
            CommandAutoCompleteFunction = commandAutoCompleteFunction;
        }
    }

    public delegate string[] CommandAutoCompleteFunc(CommandArgumentQueue argumentQueue);
}
