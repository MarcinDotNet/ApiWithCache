// Ignore Spelling: Json

using AspWithCache.Model.Interfaces;

namespace AspWithCache.Tests.Integration.MockImplementations
{
    public class MockLogger : IAspWithCacheLogger
    {
        public void Debug(string message)
        {
            WriteMessageWithTimestamp(message);
        }

        public void Debug(string providerId, string className, string message)
        {
            Console.WriteLine(providerId, className, "", message);
        }

        public void Debug(string providerId, string className, string functionName, string message)
        {
            WriteMessageWithTimestamp($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Error(string message)
        {
            WriteMessageWithTimestamp(message);
        }

        public void Error(string providerId, string className, string message)
        {
            this.Error(providerId, className, "", message);
        }

        public void Error(string providerId, string className, string functionName, string message)
        {
            WriteMessageWithTimestamp($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Error(string providerId, string className, string functionName, string message, Exception ex)
        {
            WriteMessageWithTimestamp($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}  || {ex.Message} ||  {ex.InnerException} || {ex.StackTrace} ");
        }

        public void Error(string className, string message)
        {
            WriteMessageWithTimestamp($" {className}  ||   {message}  ");
        }

        public void Info(string message)
        {
            WriteMessageWithTimestamp(message);
        }

        public void Info(string providerId, string className, string message)
        {
            this.Info(providerId, className, "", message);
        }

        public void Info(string providerId, string className, string functionName, string message)
        {
            WriteMessageWithTimestamp($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Info(string className, string message)
        {
            WriteMessageWithTimestamp($" {className}  ||   {message}  ");
        }

        public void LogJsonObject(string jsonMessage)
        {
            // _nlogLogger.
        }

        public void Trace(string message)
        {
            WriteMessageWithTimestamp(message);
        }

        public void Trace(string providerId, string className, string message)
        {
            this.Trace(providerId, className, "", message);
        }

        public void Trace(string providerId, string className, string functionName, string message)
        {
            WriteMessageWithTimestamp($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Trace(string className, string message)
        {
            WriteMessageWithTimestamp($" {className}  ||   {message}  ");
        }

        public void Warn(string message)
        {
            WriteMessageWithTimestamp(message);
        }

        public void Warn(string providerId, string className, string message)
        {
            this.Warn(providerId, className, "", message);
        }

        public void Warn(string providerId, string className, string functionName, string message)
        {
            WriteMessageWithTimestamp($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Warn(string className, string message)
        {
            WriteMessageWithTimestamp($" {className}  ||   {message}  ");
        }

        private void WriteMessageWithTimestamp(string message)
        {
            Console.WriteLine(DateTime.Now.ToString() + " || " + message);
        }
    }
}