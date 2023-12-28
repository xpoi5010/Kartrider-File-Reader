using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public class ParsedCommand
    {
        public string CommandName { get; set; } = "";

        public CommandArgumentQueue ArgumentQueue { get; } = new CommandArgumentQueue();

    }
}
