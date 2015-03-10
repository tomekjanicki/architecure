using System;
using Architecture.Business.Exception.Base;

namespace Architecture.Business.Exception
{
    [Serializable]
    public class OptimisticConcurrencyException : BaseBusinessLogicException
    {
        public OptimisticConcurrencyException(string key, Type objectType) : base(key, objectType)
        {
        }

        public OptimisticConcurrencyException(string key, Type objectType, System.Exception innerException) : base(key, objectType, innerException)
        {
        }

        protected override string Text
        {
            get { return "Optimistic Concurrency"; }
        }
    }
}