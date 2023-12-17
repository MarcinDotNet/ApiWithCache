namespace AspWithCache.Model.Exceptions
{
    public class UnableToLoadConfigurationException : Exception
    {
        public UnableToLoadConfigurationException()
        { }

        public UnableToLoadConfigurationException(string message) : base(message)
        {
        }

        public UnableToLoadConfigurationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}