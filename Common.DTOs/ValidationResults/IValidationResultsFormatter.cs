using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Common.ValidationResults
{
    public interface IValidationResultsFormatter
    {
        string Format(ValidationResults validationResults,bool isError, bool redirectMainPage);
    }
}
