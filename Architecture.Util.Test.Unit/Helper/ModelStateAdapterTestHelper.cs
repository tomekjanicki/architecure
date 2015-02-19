using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Architecture.Util.Test.Unit.Helper
{
    public static class ModelStateAdapterTestHelper
    {
        public const string Prefix = "prefix";

        public const string ValResErrorMessage1 = "ValResErrorMessage1";
        public const string ValResErrorMessageNoMember2 = "ValResErrorMessageNoMember2";
        public const string ValResErrorMessage3 = "ValResErrorMessage3";
        public const string ValResMember1 = "ValResMember1";
        public const string ValResMember2 = "ValResMember2";

        public const string AddListErrorMessage11 = "AddListErrorMessage11";
        public const string AddListErrorMessage12 = "AddListErrorMessage12";
        public const string AddListErrorMessageNoMemeber21 = "AddListErrorMessageNoMemeber21";
        public const string AddListErrorMessageNoMemeber22 = "AddListErrorMessageNoMemeber22";

        public const string AddListMemeber1 = "AddListMemeber1";

        public static List<ValidationResult> GetValidationResults()
        {
            return new List<ValidationResult>
            {
                new ValidationResult(ValResErrorMessage1, new[] {ValResMember1, ValResMember2}),
                new ValidationResult(ValResErrorMessageNoMember2),
                new ValidationResult(ValResErrorMessage3, new[] {ValResMember1}),
            };            
        }

        public static List<Tuple<string, string>> GetAdditionalList()
        {
            return new List<Tuple<string, string>>
            {
                new Tuple<string, string>(AddListMemeber1, AddListErrorMessage11),
                new Tuple<string, string>(AddListMemeber1, AddListErrorMessage12),
                new Tuple<string, string>(string.Empty, AddListErrorMessageNoMemeber21),
                new Tuple<string, string>(string.Empty, AddListErrorMessageNoMemeber22)
            };
        }

        public static Dictionary<string, IList<string>> GetEmptyDictionary()
        {
            return new Dictionary<string, IList<string>>();
        }

        public static Dictionary<string, IList<string>> GetDictionary()
        {
            return new Dictionary<string, IList<string>>
            {
                {string.Format("{0}.{1}", Prefix, AddListMemeber1), new List<string> {"M1", "M2"}},
                {string.Empty, new List<string> {"M1", "M2"}}
            };
        }

    }
}