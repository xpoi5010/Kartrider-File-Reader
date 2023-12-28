using KartLibrary.Tests.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace KartLibrary.Tests
{
    public abstract class TestStage: Commandable
    {
        private List<TestStepInfo> _steps = new List<TestStepInfo>();

        public event Action Exited;

        public virtual string StageName => this.GetType().Name;

        public void RunSteps(IConsole commandConsole)
        {
            foreach (TestStepInfo testStep in _steps)
                runStep(commandConsole, testStep);
        }

        protected void addStep(string stepName, Action action)
        {
            _steps.Add(new TestStepInfo(stepName, action));
        }

        protected void addDelay(string stepName, int delay)
        {
            _steps.Add(new TestStepInfo(stepName, () => Task.Delay(delay).Wait()));
        }

        [Command("runstep", "Runs all test step.")]
        protected virtual CommandExecuteResult commmandRunSteps(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            RunSteps(commandConsole);
            return new CommandExecuteResult(ResultType.Success, "");
        }

        [Command("exit", "Exit current test stage.")]
        protected virtual CommandExecuteResult commandExit(IConsole commandConsole, CommandArgumentQueue argumentQueue)
        {
            OnExit();
            Exited?.Invoke();
            return new CommandExecuteResult(ResultType.Success, "");
        }

        protected virtual void OnExit()
        {

        }

        private void runStep(IConsole commandConsole, TestStepInfo testStep)
        {
            string[] aniStrs = new[] { "  ---  ", "   |   " }; 
            string stepMsg = $"[  ---  ] [   Testing   ] {testStep.StepName}";
            int aniState = 0;

            DateTime taskBeginTime = DateTime.Now;
            Task<double> measureTask = measureExecuteTime(testStep.StepAction);

            if (commandConsole.CursorX != 0)
                commandConsole.WriteLine("");
            int originalCursorY = commandConsole.CursorY;
            commandConsole.Write(stepMsg);
            int originalCursorX = commandConsole.CursorX;

            long prevTimeStamp = Environment.TickCount64;
            while (!measureTask.IsCompleted && !measureTask.IsFaulted)
            {
                long duration = Environment.TickCount64 - prevTimeStamp;
                if(duration >= 391)
                {
                    lock (commandConsole)
                    {
                        aniState ^= 1;
                        commandConsole.MoveCursorTo(1, commandConsole.CursorY);
                        commandConsole.Write(aniStrs[aniState]);
                        commandConsole.MoveCursorTo(originalCursorX, originalCursorY);
                        prevTimeStamp = Environment.TickCount64;
                    }
                }
            }
            TimeSpan taskDurationWhenError = DateTime.Now - taskBeginTime;
            if (measureTask.IsCompletedSuccessfully)
            {
                commandConsole.MoveCursorTo(2, commandConsole.CursorY);
                commandConsole.SetForegroundColor(ConsoleColor.Green);
                commandConsole.Write("PASS!");
                commandConsole.SetDefaultColor();
                commandConsole.MoveCursorTo(originalCursorX, originalCursorY);
            }
            else
            {
                commandConsole.MoveCursorTo(2, originalCursorY);
                commandConsole.SetForegroundColor(ConsoleColor.Red);
                commandConsole.Write("FAIL!");
                commandConsole.SetDefaultColor();
                commandConsole.MoveCursorTo(originalCursorX, originalCursorY);
            }
            commandConsole.MoveCursorTo(12, commandConsole.CursorY);
            commandConsole.Write(formatTime(measureTask.IsFaulted ? taskDurationWhenError.TotalMilliseconds : measureTask.Result));
            commandConsole.MoveCursorTo(originalCursorX, originalCursorY);
            commandConsole.WriteLine("");
            
        }

        private string formatTime(double ms)
        {
            if(ms < 10000)
            {
                return $"{ms,8:0.000} ms";
            }
            else if(ms < 100000000)
            {
                return $"{ms,8:0.000} s ";
            }
            else
            {
                int totalSec = (int)Math.Floor(ms / 1000);
                int sec = totalSec % 60;
                int min = (totalSec / 60) % 60;
                int hour = (totalSec / 3600);
                return $"{hour,2}h {min,2}m {sec,2}s ";
            }
        }

        private async Task<double> measureExecuteTime(Action action)
        {
            return await Task.Run(() =>
            {
                DateTime beginTime = DateTime.Now;
                action.Invoke();
                DateTime endTime = DateTime.Now;
                TimeSpan duration = endTime - beginTime;
                return duration.TotalMilliseconds;
            });
        }

    }

    public class TestStepInfo
    {
        public string StepName { get; init; }

        public Action StepAction { get; init; }

        public TestStepInfo(string stepName, Action stepAction)
        {
            StepName = stepName;
            StepAction = stepAction;
        }
    }
}
