using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceQuotes.Application.Helpers
{
    public class NullableStringLength : ValidationAttribute
    {
        public NullableStringLength(int minValue, int maxValue, string errorMessage) : base(errorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is not string)
            {
                return false;
            }

            string stringValue = value as string;

            if (stringValue.Length < int.MinValue || stringValue.Length > int.MaxValue)
            {
                return false;
            }

            return true;
        }
    }
}
