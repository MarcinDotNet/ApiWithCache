using AspWithCache.Model.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWithCache.Services
{
    /// <summary>
    /// Implementation of IAspWithCacheLogger with Nlog component
    /// </summary>
    public class NlogLogger : IAspWithCacheLogger
    {
        private readonly Logger _nlogLogger;
        public NlogLogger()
        {
            _nlogLogger = LogManager.GetCurrentClassLogger();
        }

        public void Debug(string message)
        {
            _nlogLogger.Debug(message);
        }

        public void Debug(string providerId, string className, string message)
        {
            this.Debug(providerId, className, "", message);
        }

        public void Debug(string providerId, string className, string functionName, string message)
        {
            _nlogLogger.Debug($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }
        public void Error(string message)
        {
            _nlogLogger.Error(message);
        }
        public void Error(string providerId, string className, string message)
        {
            this.Error(providerId, className, "", message);
        }

        public void Error(string providerId, string className, string functionName, string message)
        {
            _nlogLogger.Error($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Error(string providerId, string className, string functionName, string message, Exception ex)
        {
            
            _nlogLogger.Error($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}  || {ex.Message} ||  {ex.InnerException} || {ex.StackTrace} ");
        }

        public void Error(string className, string message)
        {
            _nlogLogger.Error($"{className} ||  {message}");
        }

        public void Info(string message)
        {
            _nlogLogger.Info(message);
        }

        public void Info(string providerId, string className, string message)
        {
            this.Info(providerId, className, "", message);
        }

        public void Info(string providerId, string className, string functionName, string message)
        {
            _nlogLogger.Info($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Info(string className, string message)
        {
            _nlogLogger.Info($"{className} ||  {message}");
        }

        public void LogJsonObject(string jsonMessage)
        {
            _nlogLogger.Error(jsonMessage);
        }

        public void Trace(string message)
        {
            _nlogLogger.Trace(message);
        }
        public void Trace(string providerId, string className, string message)
        {
            this.Trace(providerId, className, "", message);
        }

        public void Trace(string providerId, string className, string functionName, string message)
        {
            _nlogLogger.Trace($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Trace(string className, string message)
        {
            throw new NotImplementedException();
        }

        public void Warn(string message)
        {
            _nlogLogger.Warn(message);
        }
        public void Warn(string providerId, string className, string message)
        {
            this.Warn(providerId, className, "", message);
        }

        public void Warn(string providerId, string className, string functionName, string message)
        {
            _nlogLogger.Warn($"PROVIDER ID {providerId} || {className}  ||  {functionName}  ||  {message}");
        }

        public void Warn(string className, string message)
        {
            _nlogLogger.Warn($"{className} ||  {message}");
        }
    }
}
