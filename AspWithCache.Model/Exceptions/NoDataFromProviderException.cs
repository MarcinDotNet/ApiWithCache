namespace AspWithCache.Model.Exceptions
{
    public class NoDataFromProviderException : Exception
    {
        public NoDataFromProviderException()
        { }

        public NoDataFromProviderException(string message) : base(message)
        {
        }

        public NoDataFromProviderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}