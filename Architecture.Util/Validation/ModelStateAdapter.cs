using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Architecture.Util.Validation
{
    public static class ModelStateAdapter
    {
        public static void Merge(string prefix, Dictionary<string, IList<string>> source, IList<Tuple<string, string>> additionalList)
        {
            if (prefix == null)
                throw new ArgumentNullException("prefix");
            if (source == null)
                throw new ArgumentNullException("source");
            if (additionalList == null)
                throw new ArgumentNullException("additionalList");
            foreach (var tuple in additionalList)
                Add(source, !string.IsNullOrEmpty(tuple.Item1) ? string.Format("{0}.{1}", prefix, tuple.Item1) : string.Empty, tuple.Item2);
        }

        public static Dictionary<string, IList<string>> ToDictionary(string prefix, ICollection<ValidationResult> validationResults)
        {
            if (prefix == null)
                throw new ArgumentNullException("prefix");
            if (validationResults == null)
                throw new ArgumentNullException("validationResults");
            var result = new Dictionary<string, IList<string>>();
            foreach (var validationResult in validationResults)
            {
                if (validationResult.MemberNames.Any())
                    validationResult.MemberNames.Select(memberName => string.Format("{0}.{1}", prefix, memberName)).ToList().ForEach(name => Add(result, name, validationResult.ErrorMessage));                    
                else
                    Add(result, string.Empty, validationResult.ErrorMessage);
            }
            return result;
        }

        private static void Add(IDictionary<string, IList<string>> result, string name, string errorMessage)
        {
            if (result.ContainsKey(name))
                result[name].Add(errorMessage);
            else
                result.Add(name, new List<string> { errorMessage });            
        }
    }
}