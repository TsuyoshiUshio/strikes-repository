using StrikesLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StrikesRepository
{

    public static class IValidatableExtension
    {
        public static (bool isValid, IEnumerable<ValidationResult> validationResults) Validate(this IValidatable obj)
        {
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, new ValidationContext(obj, null, null), results, true);
            return (isValid, results);
        }
    }
}
