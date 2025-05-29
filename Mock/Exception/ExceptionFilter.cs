namespace Mock.Exception
{
    public class ExceptionFilter
    {
        public class BadRequestException : System.Exception
        {
            public BadRequestException(string message) : base(message)
            {
            }
        }
        [Serializable]
        internal class NotFoundException : System.Exception
        {
            public NotFoundException()
            {
            }

            public NotFoundException(string? message) : base(message)
            {
            }

            public NotFoundException(string? message, System.Exception? innerException) : base(message, innerException)
            {
            }
        }
    }
}
