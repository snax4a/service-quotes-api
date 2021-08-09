using System;

namespace ServiceQuotes.Application.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ValidateGuidAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must be a GUID";
        public ValidateGuidAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            return System.Guid.TryParse(value.ToString(), out var guid);
        }
    }
}
