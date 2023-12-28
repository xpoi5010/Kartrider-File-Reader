using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command.Exceptions
{
    public class CommandNotFoundException: Exception
    {
        public string CommandName { get; init; }

        public CommandNotFoundException(string commandName)
        {
            CommandName = commandName;
        }

        public CommandNotFoundException(string commandName, string message): base(message)
        {
            CommandName = commandName;
        }
    }
}
