namespace BackEndTest.Infra
{
    public class UserFriendlyException : System.Exception
    {
        public UserFriendlyException(string message) : base(message)
        {
        }

        public UserFriendlyException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
