using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command.Exceptions
{
    public class CommandArgumentException: Exception
    {
        public int ArgumentNumber { get; init; }

        public CommandArgumentExceptionType ArgumentExceptionType { get; init; }

        public CommandArgumentException(int argumentNum, CommandArgumentExceptionType exceptionType, string message) : base(message) 
        { 
            ArgumentNumber = argumentNum;
            ArgumentExceptionType = exceptionType;
        }
    }

    public enum CommandArgumentExceptionType
    {
        ArgumentQueueIsEmpty,
        RequiredArgumentIsNotOption,
    }
}
