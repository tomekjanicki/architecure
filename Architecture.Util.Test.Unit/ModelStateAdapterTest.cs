using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Architecture.Util.Test.Unit.Helper;
using Architecture.Util.Validation;
using NUnit.Framework;

namespace Architecture.Util.Test.Unit
{
    public class ModelStateAdapterTest : BaseTest
    {
        [Test]
        public void ToDictionary_ValidInputParameters_ReturnsData()
        {
            var col = ModelStateAdapterTestHelper.GetValidationResults();
            var v1 = string.Format("{0}.{1}", ModelStateAdapterTestHelper.Prefix, ModelStateAdapterTestHelper.ValResMember1);
            var v2 = string.Format("{0}.{1}", ModelStateAdapterTestHelper.Prefix, ModelStateAdapterTestHelper.ValResMember2);

            var result = ModelStateAdapter.ToDictionary(ModelStateAdapterTestHelper.Prefix, col);

            Assert.That(result.ContainsKey(string.Empty) && result[string.Empty].Count == 1 && result[string.Empty].Contains(ModelStateAdapterTestHelper.ValResErrorMessageNoMember2));
            Assert.That(result.ContainsKey(v1) && result[v1].Count == 2 && result[v1].Contains(ModelStateAdapterTestHelper.ValResErrorMessage1) && result[v1].Contains(ModelStateAdapterTestHelper.ValResErrorMessage3));
            Assert.That(result.ContainsKey(v2) && result[v2].Count == 1 && result[v1].Contains(ModelStateAdapterTestHelper.ValResErrorMessage1));
        }

        [Test]
        public void Merge_EmptySource_ReturnsCorrectData()
        {
            var s = ModelStateAdapterTestHelper.GetEmptyDictionary();
            var addList = ModelStateAdapterTestHelper.GetAdditionalList();

            var v1 = string.Format("{0}.{1}", ModelStateAdapterTestHelper.Prefix, ModelStateAdapterTestHelper.AddListMemeber1);

            ModelStateAdapter.Merge(ModelStateAdapterTestHelper.Prefix, s, addList);

            Assert.That(s.ContainsKey(string.Empty) && s[string.Empty].Count == 2 && s[string.Empty].Contains(ModelStateAdapterTestHelper.AddListErrorMessageNoMemeber21) && s[string.Empty].Contains(ModelStateAdapterTestHelper.AddListErrorMessageNoMemeber22));
            Assert.That(s.ContainsKey(v1) && s[v1].Count == 2 && s[v1].Contains(ModelStateAdapterTestHelper.AddListErrorMessage11) && s[v1].Contains(ModelStateAdapterTestHelper.AddListErrorMessage12));
        }

        [Test]
        public void Merge_NonEmptySource_ReturnsCorrectData()
        {
            var s = ModelStateAdapterTestHelper.GetDictionary();
            var addList = ModelStateAdapterTestHelper.GetAdditionalList();

            var v1 = string.Format("{0}.{1}", ModelStateAdapterTestHelper.Prefix, ModelStateAdapterTestHelper.AddListMemeber1);

            ModelStateAdapter.Merge(ModelStateAdapterTestHelper.Prefix, s, addList);

            Assert.That(s.ContainsKey(string.Empty) && s[string.Empty].Count >= 2 && s[string.Empty].Contains(ModelStateAdapterTestHelper.AddListErrorMessageNoMemeber21) && s[string.Empty].Contains(ModelStateAdapterTestHelper.AddListErrorMessageNoMemeber22));
            Assert.That(s.ContainsKey(v1) && s[v1].Count >= 2 && s[v1].Contains(ModelStateAdapterTestHelper.AddListErrorMessage11) && s[v1].Contains(ModelStateAdapterTestHelper.AddListErrorMessage12));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDictionary_PrefixNull_ThrowsArgumentNullException()
        {
            ModelStateAdapter.ToDictionary(null, new Collection<ValidationResult>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToDictionary_ValidationResultsNull_ThrowsArgumentNullException()
        {
            ModelStateAdapter.ToDictionary(string.Empty, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_PrefixNull_ThrowsArgumentNullException()
        {
            ModelStateAdapter.Merge(null, new Dictionary<string, IList<string>>(), new List<Tuple<string, string>>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_SourceNull_ThrowsArgumentNullException()
        {
            ModelStateAdapter.Merge("", null, new List<Tuple<string, string>>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Merge_AdditionalListNull_ThrowsArgumentNullException()
        {
            ModelStateAdapter.Merge("", new Dictionary<string, IList<string>>(), null);
        }

    }
}
