namespace Ghost.Exceptions
{
    public class IncorrectJobTypeException : Exception
    {
        public IncorrectJobTypeException() : base("Incorrect job type was given") { }
    }
}