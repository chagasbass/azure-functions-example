﻿namespace Serverless.Extensions.Shared.Logs.Services;

public interface ILogServices
{
    public LogData LogData { get; set; }
    void WriteLog();
    void WriteErrorLog();
    void CreateStructuredLog(LogData logData);
    void WriteLogWhenRaiseExceptions();
    void WriteMessage(string message);
}
