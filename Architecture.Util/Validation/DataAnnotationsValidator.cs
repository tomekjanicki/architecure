using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Architecture.Util.Validation
{
    public static class DataAnnotationsValidator
    {
        private static IEnumerable<ValidationResult> ValidateCurrentObject(object obj)
        {
            var context = new ValidationContext(obj, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, results, true);
            return results;
        }

        private static IEnumerable<Tuple<string, object>> GetComplexPropertyNamesWithPropertyValues(object obj)
        {
            return obj.GetType().GetProperties()
                .Where(p => (!(p.GetValue(obj) is IEnumerable)) && p.PropertyType.IsClass && p.PropertyType.MemberType == MemberTypes.NestedType)
                .Select(p => new Tuple<string, object>(p.Name, p.GetValue(obj)))
                .Where(tuple => tuple.Item2 != null);
        }

        private static IEnumerable<Tuple<string, IEnumerable>> GetEnumerablePropertyNamesWithPropertyValues(object obj)
        {
            return obj.GetType().GetProperties()
                .Where(p => (p.GetValue(obj) is IEnumerable))
                .Select(p => new Tuple<string, IEnumerable>(p.Name, (IEnumerable)p.GetValue(obj)));
        }

        private static IEnumerable<string> GetProccessedMemeberNames(IEnumerable<string> memberNames, int? position, string property)
        {
            return memberNames.Select(s => string.Format("{0}{1}.{2}", property, position == null ? string.Empty : string.Format("[{0}]", position), s));
        }

        private static IEnumerable<ValidationResult> GetMergedValidationResults(IEnumerable<ValidationResult> sourceValidationResults, string property, int? position)
        {
            var destinationValidationResults = new Collection<ValidationResult>();
            foreach (var validationResult in sourceValidationResults)
                destinationValidationResults.Add(new ValidationResult(validationResult.ErrorMessage, GetProccessedMemeberNames(validationResult.MemberNames, position, property)));
            return destinationValidationResults;
        }

        private static IEnumerable<ValidationResult> ValidateNestedObjects(IEnumerable<Tuple<string, IEnumerable>> enumerablePropertyNamesWithPropertyValues, IEnumerable<Tuple<string, object>> complexPropertyNamesWithPropertyValues)
        {
            var results = new List<ValidationResult>();
            foreach (var enumerablePropertyNameWithPropertyValue in enumerablePropertyNamesWithPropertyValues)
            {
                var counter = 0;
                foreach (var enumerable in enumerablePropertyNameWithPropertyValue.Item2)
                {
                    var a = GetMergedValidationResults(ValidateCurrentObject(enumerable), enumerablePropertyNameWithPropertyValue.Item1, counter);
                    var b = GetMergedValidationResults(ValidateNestedObjects(GetEnumerablePropertyNamesWithPropertyValues(enumerable), GetComplexPropertyNamesWithPropertyValues(enumerable)), enumerablePropertyNameWithPropertyValue.Item1, counter);
                    results.AddRange(a.Union(b));
                    counter++;
                }
            }
            foreach (var complexPropertyNameWithPropertyValue in complexPropertyNamesWithPropertyValues)
            {
                var a = GetMergedValidationResults(ValidateCurrentObject(complexPropertyNameWithPropertyValue.Item2), complexPropertyNameWithPropertyValue.Item1, null);
                var b = GetMergedValidationResults(ValidateNestedObjects(GetEnumerablePropertyNamesWithPropertyValues(complexPropertyNameWithPropertyValue.Item2), GetComplexPropertyNamesWithPropertyValues(complexPropertyNameWithPropertyValue.Item2)), complexPropertyNameWithPropertyValue.Item1, null);
                results.AddRange(a.Union(b));
            }
            return results;
        }

        public static ICollection<ValidationResult> Validate(object obj)
        {
            Extension.EnsureIsNotNull(obj, "obj");
            return ValidateCurrentObject(obj).Union(ValidateNestedObjects(GetEnumerablePropertyNamesWithPropertyValues(obj), GetComplexPropertyNamesWithPropertyValues(obj))).ToList();
        }

    }
}
