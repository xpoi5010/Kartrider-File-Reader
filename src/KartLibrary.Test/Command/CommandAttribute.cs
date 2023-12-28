using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute: Attribute
    {
        public string CommandName { get; set; }

        public string CommandDescription { get; set; }

        public CommandAttribute(string commandName, string desc = "") 
        { 
            CommandName = commandName;
            CommandDescription = desc;
        }
    }
}
