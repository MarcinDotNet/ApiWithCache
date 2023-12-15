using AspWithCache.Model.Interfaces;

namespace AspWithCache.Tests.Integration.MockImplementations
{
    public class MockLogger : IAspWithCacheLogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug(string providerId, string className, string message)
        {
            Console.WriteLine(providerId, className, "", message);
        }

        public void Debug(string providerId, string className, string functionName, string message)
        {
            Console.WriteLine($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Error(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string providerId, string className, string message)
        {
            this.Error(providerId, className, "", message);
        }

        public void Error(string providerId, string className, string functionName, string message)
        {
            Console.WriteLine($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Error(string providerId, string className, string functionName, string message, Exception ex)
        {
            string innerExceptionMessage = ex.InnerException != null ? ex.InnerException.Message : "";
            Console.WriteLine($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}  || {ex.Message} ||  {ex.InnerException} || {ex.StackTrace} ");
        }

        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Info(string providerId, string className, string message)
        {
            this.Info(providerId, className, "", message);
        }

        public void Info(string providerId, string className, string functionName, string message)
        {
            Console.WriteLine($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void LogJsonObject(string jsonMessage)
        {
            // _nlogLogger.
        }

        public void Trace(string message)
        {
            Console.WriteLine(message);
        }

        public void Trace(string providerId, string className, string message)
        {
            this.Trace(providerId, className, "", message);
        }

        public void Trace(string providerId, string className, string functionName, string message)
        {
            Console.WriteLine($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Warn(string message)
        {
            Console.WriteLine(message);
        }

        public void Warn(string providerId, string className, string message)
        {
            this.Warn(providerId, className, "", message);
        }

        public void Warn(string providerId, string className, string functionName, string message)
        {
            Console.WriteLine($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }
    }
}