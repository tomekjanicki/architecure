using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Architecture.Util.Test.Unit.Helper
{
    public class Header : IValidatableObject
    {
        public const int MinValue = 1;
        public const string ContentRequiredMessage = "At least one position is required";

        [Range(MinValue, int.MaxValue)]
        public int IntId { get; set; }
        
        public IEnumerable<Content> Contents { get; set; }

        [Required]
        public string StringProperty { get; set; }

        [Required]
        public OneToOne OneToOneProperty { get; set; }

        public class Content
        {
            [Range(MinValue, int.MaxValue)]
            public int IntId { get; set; }

            [Required]
            public Header Header { get; set; }
        }

        public class OneToOne
        {
            [Range(MinValue, int.MaxValue)]
            public int IntId { get; set; }

            [Required]
            public Header Header { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Contents == null || !Contents.Any())
                yield return new ValidationResult(ContentRequiredMessage);
        }

        public static Header GetDefaultObject()
        {
            return new Header();
        }

        public static Header GetObjectWithInitializedHeader()
        {
            var h = new Header { IntId = 5, StringProperty = "s", OneToOneProperty = new OneToOne() };
            h.OneToOneProperty.Header = h;
            return h;
        }

        public static Header GetObjectWithInitializedHeaderAndContent()
        {
            var h = new Header { IntId = 5, StringProperty = "s", OneToOneProperty = new OneToOne() };
            var content = new Content { Header = h };
            h.Contents = new List<Content> { content };
            h.OneToOneProperty.Header = h;
            return h;
        }

        public static Header GetValidObject()
        {
            var h = new Header { IntId = 5, StringProperty = "s", OneToOneProperty = new OneToOne { IntId = 5 } };
            var content = new Content { Header = h, IntId = 5 };
            h.Contents = new List<Content> { content };
            h.OneToOneProperty.Header = h;
            return h;
        }

    }
}
