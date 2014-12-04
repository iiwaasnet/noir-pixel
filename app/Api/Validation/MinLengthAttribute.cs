using System.ComponentModel.DataAnnotations;

namespace Api.Validation
{
    public class MinLengthAttribute : StringLengthAttribute
    {
        public MinLengthAttribute(int maximumLength) 
            : base(maximumLength)
        {
        }

        public override bool IsValid(object value)
        {
            return false;
            return base.IsValid(value);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }

        public override string FormatErrorMessage(string name)
        {
            return "Whatever bla" + name;
        }


    }
}