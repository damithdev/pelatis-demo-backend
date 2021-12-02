using Pelatis.Helpers.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pelatis.Dto.Validators
{
    public class EmailValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (ValidatorUtils.IsValidEmail(value.ToString()))
            {
                return null;
            }
            else
            {
                return new ValidationResult("Invalid Email",new[] { validationContext.MemberName });
            }
        }
    }
}
