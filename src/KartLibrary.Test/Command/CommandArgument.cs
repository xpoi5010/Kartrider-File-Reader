using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public record struct CommandArgument(CommandArgumentType ArgumentType, string Value);
}
