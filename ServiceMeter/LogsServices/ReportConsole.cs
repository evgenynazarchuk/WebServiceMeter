using System;
using System.Threading.Tasks;

namespace ServiceMeter.LogsServices;

public class ReportConsole : Report
{
    public ReportConsole(string logName)
        : base(logName)
    {
    }

    public override Task StartReportProcessAsync()
    {
        var processWrite = Task.Run(() =>
        {
            while (true)
            {
                if (this.Token.IsCancellationRequested && this.LogsQueue.IsEmpty)
                {
                    break;
                }
                
                if (this.LogsQueue.TryDequeue(out var logMessage))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(this.LogName + ":");
                    Console.ResetColor();
                    Console.WriteLine(logMessage);
                }
            }
        });

        return processWrite;
    }
}