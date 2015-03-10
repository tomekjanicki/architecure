using System;
using Architecture.Business.Exception.Base;

namespace Architecture.Business.Exception
{
    [Serializable]
    public class UniqueConstraintViolationException : BaseBusinessLogicException
    {
        public UniqueConstraintViolationException(string key, Type objectType) : base(key, objectType)
        {
        }

        public UniqueConstraintViolationException(string key, Type objectType, System.Exception innerException) : base(key, objectType, innerException)
        {
        }

        protected override string Text
        {
            get { return "Unique Constraint Violation"; }
        }
    }
}