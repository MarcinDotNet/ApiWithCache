// Ignore Spelling: json

namespace AspWithCache.Model.Interfaces
{
    /// <summary>
    /// Logger data
    /// </summary>
    public interface IAspWithCacheLogger
    {
        public void Error(string message);

        public void Error(string providerId, string className, string message);

        public void Error(string providerId, string className, string functionName, string message);

        public void Error(string providerId, string className, string functionName, string message, Exception ex);

        public void Warn(string message);

        public void Warn(string providerId, string className, string message);

        public void Warn(string providerId, string className, string functionName, string message);

        public void Trace(string message);

        public void Trace(string providerId, string className, string message);

        public void Trace(string providerId, string className, string functionName, string message);

        public void Info(string message);

        public void Info(string providerId, string className, string message);

        public void Info(string providerId, string className, string functionName, string message);

        public void Debug(string message);

        public void Debug(string providerId, string className, string message);

        public void Debug(string providerId, string className, string functionName, string message);

        public void LogJsonObject(string jsonMessage);
    }
}