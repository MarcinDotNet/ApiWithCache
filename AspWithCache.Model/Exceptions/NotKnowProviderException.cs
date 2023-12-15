namespace AspWithCache.Model.Exceptions
{
    public class NotKnowProviderException : Exception
    {
        public NotKnowProviderException()
        { }

        public NotKnowProviderException(string message) : base(message)
        {
        }

        public NotKnowProviderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}