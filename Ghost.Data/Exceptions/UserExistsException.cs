namespace Ghost.Exceptions
{
    public class UserExistsException : Exception
    {
        public UserExistsException() : base("User already exsits") { }
    }
}