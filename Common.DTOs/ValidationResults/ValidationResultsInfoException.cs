using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Common.ValidationResults
{
    public class ValidationResultsInfoException : InfoFriendlyException
    {
        public ValidationResultsInfoException(ValidationResults validationResults)
            : base(validationResults.GetErrorsText(false, false))
        {
            ValidationResults = validationResults;
        }

        public ValidationResults ValidationResults { get; }
    }
}
