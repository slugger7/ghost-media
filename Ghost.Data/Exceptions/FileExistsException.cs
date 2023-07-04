namespace Ghost.Exceptions
{
    public class FileExistsException : Exception
    {
        public FileExistsException() : base("File exists already") { }
    }
}