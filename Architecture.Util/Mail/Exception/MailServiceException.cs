﻿using System;
using Architecture.Util.Exception;

namespace Architecture.Util.Mail.Exception
{
    [Serializable]
    public class MailServiceException : BaseException
    {
        public MailServiceException(System.Exception innerException)
            : base("Error during sending mail. See inner exception for details", innerException)
        {
        }
    }
}
