namespace ServiceMeter.Interfaces;

public interface IWatcher
{
    void StartAllReportsProcesses();
    
    void StopAllReportsProcesses();

    void SendMessage(string logMessage);
}