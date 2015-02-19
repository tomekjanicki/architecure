using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Architecture.Util.Test.Unit.Helper
{
    public static class DataAnnotationsValidatorTestHelper
    {
        public enum Type
        {
            Required,
            Range
        }

        public static bool Contains(this ICollection<ValidationResult> coll, string property, Type type)
        {
            return coll.FirstOrDefault(result => result.ErrorMessage == GetMessage(property, type) && result.MemberNames.FirstOrDefault(s => s == property) != null) != null;
        }

        public static bool Contains<T>(this ICollection<ValidationResult> coll, Expression<Func<T, object>> expr, Type type)
        {
            return coll.FirstOrDefault(result => result.ErrorMessage == GetMessage(expr, type) && result.MemberNames.FirstOrDefault(s => s == Extension.GetPropertyName(expr)) != null) != null;
        }

        public static bool Contains(this ICollection<ValidationResult> coll, string message)
        {
            return coll.FirstOrDefault(result => result.ErrorMessage == message) != null;
        }

        private static string GetMessage(string property, Type type)
        {
            var properties = property.Split('.');
            switch (type)
            {
                case Type.Range:
                    return string.Format(@"The field {0} must be between {1} and {2}.", properties.Last(), Header.MinValue, int.MaxValue);
                case Type.Required:
                    return string.Format(@"The {0} field is required.", properties.Last());
            }
            throw new ArgumentException();
        }

        private static string GetMessage<T>(Expression<Func<T, object>> expr, Type type)
        {
            return GetMessage(Extension.GetPropertyName(expr), type);
        }
    }
}