using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;

namespace Architecture.Util.Mail
{
    [Serializable]
    public class MailDefinition
    {
        public string[] Recipients { get; set; }
        public string[] CcRecipients { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string From { get; set; }

        public MailMessage ConvertToMailMessage()
        {
            var message = new MailMessage { From = new MailAddress(From), IsBodyHtml = false };
            if (Recipients != null)
                Recipients.ToList().ForEach(x => message.To.Add(x));
            if (CcRecipients != null)
                CcRecipients.ToList().ForEach(x => message.CC.Add(x));
            message.Subject = Subject;
            message.Body = Content;
            return message;            
        }

        public byte[] ToBytes()
        {
            using (var memoryStream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(memoryStream, this);
                return memoryStream.GetBuffer();
            }
        }

        public static MailDefinition FromBytes(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
                return (MailDefinition) new BinaryFormatter().Deserialize(memoryStream);
        }

    }
}
