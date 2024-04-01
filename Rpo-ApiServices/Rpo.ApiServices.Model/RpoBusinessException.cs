using System;

namespace Rpo.ApiServices.Model
{
    [Serializable]
    public class RpoBusinessException : Exception
    {
        public RpoBusinessException(string message)
            : base(message)
        {

        }
        public RpoBusinessException(string message, Exception innerException)
            : base (message, innerException)
        {

        }

    }

    [Serializable]
    public class RpoUnAuthorizedException : Exception
    {
        public RpoUnAuthorizedException(string message)
            : base(message)
        {

        }
        public RpoUnAuthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

    }
}
