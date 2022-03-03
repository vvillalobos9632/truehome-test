using Common.DTOs.Property;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTOs.FluentValidations
{
    public class PropertyDataValidator : AbstractValidator<PropertyData>
    {
        public PropertyDataValidator()
        {
            RuleFor(x => x.title).NotEmpty().MaximumLength(255);
            RuleFor(x => x.address).NotEmpty();
            RuleFor(x => x.description).NotEmpty();
            RuleFor(x => x.status).NotEmpty();
        }
    }
}
