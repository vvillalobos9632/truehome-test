using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Common.ValidationResults
{
    public static class ValidationResultsExtensions
    {
        public static string GetErrorsText(this ValidationResults validationResults, bool isError, bool redirectMainPage)
        {
            var formatter = new ValidationResultsTextFormatter();
            return validationResults.Format(formatter, isError, redirectMainPage);
        }

        public static ValidationResults AddErrorIfIsNullOrWhiteSpace(this ValidationResults validationResults, string value, string error, string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(value))
                validationResults.AddError(propertyName, error);

            return validationResults;
        }

        public static ValidationResults AddErrorIf(this ValidationResults validationResults, bool condition, string error)
        {
            return AddPropertyErrorIf(validationResults, condition, string.Empty, error);
        }

        public static ValidationResults AddInfoIf(this ValidationResults validationResults, bool condition, string error)
        {
            return AddPropertyInfoIf(validationResults, condition, string.Empty, error);
        }

        private static ValidationResults AddPropertyInfoIf(this ValidationResults validationResults, bool condition, string propertyName, string error)
        {
            if (condition)
                validationResults.AddInfo(propertyName, error);

            return validationResults;
        }

        public static ValidationResults AddPropertyErrorIf(this ValidationResults validationResults, bool condition, string propertyName, string error)
        {
            if (condition)
                validationResults.AddError(propertyName, error);

            return validationResults;
        }

        public static void AssertIsValid(this ValidationResults validationResults, bool redirectMainPage = false)
        {
            if (!validationResults.IsValid)
                throw new ValidationResultsInvalidException(validationResults, redirectMainPage);
        }

        public static void AssertIsValidInfo(this ValidationResults validationResults)
        {
            if (!validationResults.HasInfo)
                throw new ValidationResultsInfoException(validationResults);
        }


        public static ValidationResults AddFluenValidationErrors(this ValidationResult validationResult, ValidationResults validations)
        {
            validationResult.Errors.ForEach(x =>
            {
                AddPropertyErrorIf(validations, true, string.Empty, x.ErrorMessage);
            });

            return validations;
        }

    }

}
