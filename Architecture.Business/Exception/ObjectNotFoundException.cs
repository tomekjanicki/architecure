using System;
using Architecture.Business.Exception.Base;

namespace Architecture.Business.Exception
{
    public class ObjectNotFoundException : BaseBusinessLogicException
    {
        public ObjectNotFoundException(string key, Type objectType) : base(key, objectType)
        {
        }

        public ObjectNotFoundException(string key, Type objectType, System.Exception innerException) : base(key, objectType, innerException)
        {
        }

        protected override string Text
        {
            get { return "Object Not Found"; }
        }
    }
}