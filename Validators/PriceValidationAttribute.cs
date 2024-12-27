using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Manager.Validators
{
    internal class PriceValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var intPrice = (int)value;

            if(intPrice == 0)
            {
                return new ValidationResult("Price can't be zero.");
            }
            return ValidationResult.Success;
        }
    }
}
