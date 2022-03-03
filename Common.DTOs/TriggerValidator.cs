using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs
{
    public class TriggerValidator<T> : AbstractValidator<T> where T : class
    {
        public ValidationResult ValidateObject(T instance)
        {
            return Validate(instance);
        }
    }
}
