using System.ComponentModel.DataAnnotations;

namespace Product_Manager.Validators
{
    internal class PriceValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// IsValid Function
        /// ----------------
        /// 1. Validates price is zero or not.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns>
        ///     1. returns ValidationResult as price can't be zero if price is zero.
        ///     2. returns ValidationResult as Success if price is not zero.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int intPrice = (int)value;

            if (intPrice == 0)
            {
                return new ValidationResult("Price can't be zero.");
            }
            return ValidationResult.Success;
        }
    }
}
