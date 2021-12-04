using Pelatis.Helpers.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Pelatis.Dto.Validators
{
    public class EmailValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && ValidatorUtils.IsValidEmail(value.ToString()))
            {
                return null;
            }
            else
            {
                return new ValidationResult("Invalid Email", new[] { validationContext.MemberName });
            }
        }
    }
}
