using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public record struct CommandExecuteResult(ResultType ResultType, string Message);

    public enum ResultType
    {
        Success,
        Warning,
        Failure,
    }
}
