using Architecture.Util.Test.Unit.Helper;
using Architecture.Util.Validation;
using NUnit.Framework;

namespace Architecture.Util.Test.Unit
{
    public class DataAnnotationsValidatorTest : BaseTest
    {

        [Test]
        public void Validate_ValidObject_ReturnsEmptyResult()
        {
            var data = Header.GetValidObject();

            var result = DataAnnotationsValidator.Validate(data);

            Assert.That(result.Count == 0);
        }

        [Test]
        public void Validate_DefaultObject_ReturnsErrors()
        {
            var data = Header.GetDefaultObject();

            var result = DataAnnotationsValidator.Validate(data);

            Assert.That(result.Contains<Header>(p => p.IntId, DataAnnotationsValidatorTestHelper.Type.Range));
            Assert.That(result.Contains<Header>(p => p.OneToOneProperty, DataAnnotationsValidatorTestHelper.Type.Required));
            Assert.That(result.Contains<Header>(p => p.StringProperty, DataAnnotationsValidatorTestHelper.Type.Required));
        }

        [Test]
        public void Validate_ObjectWithInitializedHeader_ReturnsErrors()
        {
            var data = Header.GetObjectWithInitializedHeader();

            var result = DataAnnotationsValidator.Validate(data);

            Assert.That(result.Contains(Header.ContentRequiredMessage));
            Assert.That(result.Contains<Header>(p => p.OneToOneProperty.IntId, DataAnnotationsValidatorTestHelper.Type.Range));
        }

        [Test]
        public void Validate_ObjectWithInitializedHeaderAndContent_ReturnsErrors()
        {
            var data = Header.GetObjectWithInitializedHeaderAndContent();

            var result = DataAnnotationsValidator.Validate(data);

            Assert.That(result.Contains<Header>(p => p.OneToOneProperty.IntId, DataAnnotationsValidatorTestHelper.Type.Range));
            Assert.That(result.Contains("Contents[0].IntId", DataAnnotationsValidatorTestHelper.Type.Range));
        }

    }
}
