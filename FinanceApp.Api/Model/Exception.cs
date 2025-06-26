namespace FinanceApp.Api.Model
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Resource not found.") { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }
    public class ConflictException : Exception
    {
        public ConflictException() : base("Conflict occurred.") { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException) { }
    }
    public class ValidationException : Exception
    {
        public ValidationException() : base("Validation failed.") { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized access.") { }

        public UnauthorizedException(string message) : base(message) { }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
