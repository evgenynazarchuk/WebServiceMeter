using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebServiceMeter.Interfaces;

namespace WebServiceMeter
{
    public class Watcher
    {
        protected readonly List<IReport> reports;

        public Watcher()
        {
            this.reports = new List<IReport>();
        }

        public void AddReport(IReport report)
        {
            this.reports.Add(report);
        }

        public void ClearReports()
        {
            this.reports.Clear();
        }

        public void SendMessage(string logName, string logMessage, Type logMessageType)
        {
            foreach (var logger in this.reports)
            {
                logger.SendLogMessage(logName, logMessage, logMessageType);
            }
        }

        public List<Task> StartAsync()
        {
            var tasks = new List<Task>();

            foreach (var report in this.reports)
            {
                tasks.Add(report.StartAsync());
            }

            return tasks;
        }

        public void Stop()
        {
            foreach (var report in this.reports)
            {
                report.Stop();
            }
        }
    }
}
