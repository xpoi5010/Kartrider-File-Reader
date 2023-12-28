using KartLibrary.Tests.Command.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KartLibrary.Tests.Command
{
    public abstract class Commandable
    {
        private Dictionary<string, CommandExecuteInfo> _commandExecInfo = new Dictionary<string, CommandExecuteInfo>();
        private Dictionary<string, CommandAutoCompleteInfo> _commandAutoComplInfo = new Dictionary<string, CommandAutoCompleteInfo>();

        protected IReadOnlyDictionary<string, CommandExecuteInfo> RegistedCommands => _commandExecInfo;

        protected IReadOnlyDictionary<string, CommandAutoCompleteInfo> RegistedCommandAutoCompletes => _commandAutoComplInfo;

        protected Commandable()
        {
            Type derivedType = this.GetType();
            MethodInfo[] methods = derivedType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach(MethodInfo method in methods)
            {
                CommandAttribute? commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                CommandAutoCompleteAttribute? commandAutoComplAttribute = method.GetCustomAttribute<CommandAutoCompleteAttribute>();
                if (commandAttribute is not null)
                {
                    CommandFunction commandFunc = method.CreateDelegate<CommandFunction>(this);
                    CommandExecuteInfo execInfo = new CommandExecuteInfo(commandAttribute.CommandName, commandAttribute.CommandDescription, commandFunc);
                    if (_commandExecInfo.ContainsKey(commandAttribute.CommandName))
                        throw new Exception("");
                    _commandExecInfo.Add(execInfo.CommandName, execInfo);
                }
                else if(commandAutoComplAttribute is not null)
                {
                    CommandAutoCompleteFunc commandAutoCompleteFunc = method.CreateDelegate<CommandAutoCompleteFunc>(this);
                    CommandAutoCompleteInfo commandAutoCompleteInfo = new CommandAutoCompleteInfo(commandAutoComplAttribute.CommandName, commandAutoCompleteFunc);
                    if (_commandAutoComplInfo.ContainsKey(commandAutoComplAttribute.CommandName))
                        throw new Exception("");
                    _commandAutoComplInfo.Add(commandAutoCompleteInfo.CommandName, commandAutoCompleteInfo);
                }
            }
        }

        public void ExecuteCommand(IConsole commandConsole, string command)
        {
            try
            {
                ParsedCommand parsedCommand = CommandParser.Parse(command);
                if(!_commandExecInfo.ContainsKey(parsedCommand.CommandName)) 
                {
                    throw new CommandNotFoundException(parsedCommand.CommandName);
                }
                CommandExecuteInfo execInfo = _commandExecInfo[parsedCommand.CommandName];
                CommandExecuteResult result = execInfo.Function.Invoke(commandConsole, parsedCommand.ArgumentQueue);
                if(result.ResultType == ResultType.Failure) 
                {
                    commandConsole.SetForegroundColor(ConsoleColor.Red);
                    commandConsole.WriteLine($"Fail to execute command {parsedCommand.CommandName}: {result.Message}");
                    commandConsole.SetDefaultColor();
                }
            }
            catch(Exception ex)
            {
                string exceptionMsg = "";
                switch (ex)
                {
                    case CommandArgumentException commandArgumentException:
                        if(commandArgumentException.ArgumentExceptionType == CommandArgumentExceptionType.ArgumentQueueIsEmpty)
                            exceptionMsg = $"Too few of argument to command.";
                        else if(commandArgumentException.ArgumentExceptionType == CommandArgumentExceptionType.RequiredArgumentIsNotOption)
                            exceptionMsg = $"Argument {commandArgumentException.ArgumentNumber} excepts option but string.";
                        break;
                    case CommandNotFoundException commandNotFoundException:
                        exceptionMsg = $"Cannot found \"{commandNotFoundException.CommandName}\" command.";
                        break;
                    default:
                        exceptionMsg = $"Exception: {ex.Message}\r\n{ex.StackTrace}";
                        break;
                }
                commandConsole.SetForegroundColor(ConsoleColor.Red);
                commandConsole.WriteLine(exceptionMsg);
                commandConsole.SetDefaultColor();
            }
        }

        public string[] AutoCompleteCommand(string command, int index)
        {
            string trimCommand = command.Trim();
            if (trimCommand.Length == 0)
                return Array.Empty<string>();
            ParsedCommand parsedCommand = CommandParser.Parse(trimCommand);
            if(parsedCommand.ArgumentQueue.Count > 0 || command.EndsWith(' ') || command.EndsWith('-'))
            {
                if (!_commandAutoComplInfo.ContainsKey(parsedCommand.CommandName) || !_commandExecInfo.ContainsKey(parsedCommand.CommandName))
                    return Array.Empty<string>();
                else
                {
                    CommandAutoCompleteInfo autoCompleteInfo = _commandAutoComplInfo[parsedCommand.CommandName];
                    try
                    {
                        return autoCompleteInfo.CommandAutoCompleteFunction.Invoke(parsedCommand.ArgumentQueue);
                    }
                    catch
                    {
                        return Array.Empty<string>();
                    }
                }
            }
            else
            {
                string searchCommandName = parsedCommand.CommandName;
                List<string> matchCommands = new List<string>();
                foreach(string commandName in RegistedCommands.Keys)
                    if(commandName.StartsWith(searchCommandName))
                        matchCommands.Add(commandName);
                return matchCommands.ToArray();
            }
        }

        [Command("help", "List all commands name and its usage.")]
        public virtual CommandExecuteResult PrintHelp(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            int maxCommandNameLen = _commandExecInfo.Keys.Select(x => x.Length).Max();
            commandConsole.WriteLine("Available commands: ");
            foreach (CommandExecuteInfo commandExecInfo in _commandExecInfo.Values)
                commandConsole.WriteLine($"{commandExecInfo.CommandName.PadRight(maxCommandNameLen + 5)} {commandExecInfo.CommandDescription}");
            return new CommandExecuteResult(ResultType.Success, "");
        }
    }
}
