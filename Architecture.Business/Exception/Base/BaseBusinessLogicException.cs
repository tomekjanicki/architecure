using System;
using Architecture.Util.Exception;

namespace Architecture.Business.Exception.Base
{
    public abstract class BaseBusinessLogicException : BaseException
    {
        public string Key { get; private set; }
        public Type ObjectType { get; set; }
        protected abstract string Text { get; }

        protected BaseBusinessLogicException(string key, Type objectType) 
            : base(string.Empty)
        {
            Key = key;
            ObjectType = objectType;
        }

        protected BaseBusinessLogicException(string key, Type objectType, System.Exception innerException)
            : base(string.Empty, innerException)
        {
            Key = key;
            ObjectType = objectType;
        }

        public override string Message
        {
            get { return string.Format("{0}\r\nObjectType: {1}\r\nKey: {2}", Text, ObjectType.FullName, Key); }
        }
    }
}
