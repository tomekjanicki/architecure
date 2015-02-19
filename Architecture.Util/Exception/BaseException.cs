namespace Architecture.Util.Exception
{
    public abstract class BaseException : System.Exception
    {
        protected BaseException(string message) : base(message)
        {
            
        }

        protected BaseException(string messsage, System.Exception innerException) : base(messsage, innerException)
        {
            
        }
    }
}
