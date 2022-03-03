using Common.DTOs.Settings;

namespace Common.DTOs.Common.ValidationResults
{
    public class ValidationResultsInvalidException : FriendlyException
    {
        public ValidationResultsInvalidException(ValidationResults validationResults, bool redirect)
            : base(validationResults.GetErrorsText(true, redirect))
        {
            ValidationResults = validationResults;
        }

        public ValidationResults ValidationResults { get; }
    }
}
