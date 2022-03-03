
using FluentValidation.Results;

namespace Common.DTOs.Common.ValidationResults
{
    public class ValidationResults
    {
        public Dictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> Infos { get; } = new Dictionary<string, List<string>>();

        public bool IsValid
        {
            get { return !Errors.Any(); }
        }

        public bool HasInfo {
            get { return !Infos.Any(); }
        }

        public void AddError(string propertyName, string error)
        {
            if (!Errors.ContainsKey(propertyName))
                Errors.Add(propertyName, new List<string>());

            var propertyErrors = Errors[propertyName];
            propertyErrors.Add(error);
        }

        public string Format(IValidationResultsFormatter formatter,bool isError, bool redirectMainPage)
        {
            return formatter.Format(this, isError, redirectMainPage);
        }

        public IEnumerable<string> GetPropertyNames()
        {
            return Errors.Keys;
        }

        public IEnumerable<string> GetPropertyErrors(string propertyName)
        {
            List<string> propertyErrors;
            if (!Errors.TryGetValue(propertyName, out propertyErrors))
                propertyErrors = new List<string>();

            return propertyErrors;
        }

        public static ValidationResults Empty()
        {
            return new ValidationResults();
        }

        public void AddInfo(string propertyName, string info)
        {
            if (!Infos.ContainsKey(propertyName))
                Infos.Add(propertyName, new List<string>());

            var propertyErrors = Infos[propertyName];
            propertyErrors.Add(info);
        }
    }

}
