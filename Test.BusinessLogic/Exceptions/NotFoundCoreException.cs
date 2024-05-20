namespace Test.Core.Exceptions
{
    public class NotFoundCoreException : CoreException
    {
        public NotFoundCoreException()
        {
        }

        public NotFoundCoreException(string message)
            : base(message)
        {
        }

        public NotFoundCoreException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
