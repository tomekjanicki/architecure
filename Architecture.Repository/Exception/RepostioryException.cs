using Architecture.Util.Exception;

namespace Architecture.Repository.Exception
{
    public class RepostioryException : BaseException
    {

        public RepostioryException(System.Exception innerException) : base("Error during handling repository operation. See inner exception for details", innerException)
        {
        }
    }
}
