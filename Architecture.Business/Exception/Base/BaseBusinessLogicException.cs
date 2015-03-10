using System;
using System.Runtime.Serialization;
using Architecture.Util.Exception;

namespace Architecture.Business.Exception.Base
{
    [Serializable]
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

        protected BaseBusinessLogicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Key = info.GetString("Key");
            ObjectType = (Type)info.GetValue("ObjectType", typeof(Type));
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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Key", Key);
            info.AddValue("ObjectType", ObjectType);
        }
    }
}
