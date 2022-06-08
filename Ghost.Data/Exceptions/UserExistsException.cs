namespace Ghost.Exceptions
{
  public class UserExisistException : Exception
  {
    public UserExisistException() : base("User already exsits") { }
  }
}