using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public class CommandExecuteInfo
    {
        public string CommandName { get; set; }

        public string CommandDescription { get; set; }

        public CommandFunction Function { get; set; }

        public CommandExecuteInfo(string commandName, string commandDesc, CommandFunction func) 
        {
            CommandName = commandName;
            CommandDescription = commandDesc;
            Function = func;
        }
    }

    public delegate CommandExecuteResult CommandFunction(IConsole commandConsole, CommandArgumentQueue argumentQueue);
}
