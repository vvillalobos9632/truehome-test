using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs.Common.ValidationResults
{
    public class ValidationResultsTextFormatter
: IValidationResultsFormatter
    {
        public string Format(ValidationResults validationResults, bool isError, bool RedirectMainPage)
        {

            //  Cras justo odio</li>
            //  <li class="list-group-item">Dapibus ac facilisis in</li>
            //  <li class="list-group-item">Morbi leo risus</li>

            //  <li class="list-group-item">Vestibulum at eros</li>
            //</ul>
            var errorsTextBuilder = new StringBuilder();
            //errorsTextBuilder.Append("###ERROR###");
            //errorsTextBuilder.Append("<ul class=\"list-group list-group-flush\">");
            var indexError = 1;
            var list = isError ? validationResults.Errors : validationResults.Infos;
            foreach (var errorKeyPair in list)
            {
                foreach (var error in errorKeyPair.Value)
                {
                    AppenLineOfNotNew(errorsTextBuilder, error, indexError);
                    indexError++;
                }
            }
            //errorsTextBuilder.Append("</ul>");

            if (RedirectMainPage)
            {
                errorsTextBuilder.Append("<br/>");
                errorsTextBuilder.Append("<a href='https://www.gasnn.com/'>Regresar</a>");
            }
            //errorsTextBuilder.Append("###ERROR###");

            return errorsTextBuilder.ToString();
        }

        public StringBuilder AppenLineOfNotNew(StringBuilder stringBuilder, string value, int indexError)
        {
            if (stringBuilder.Length > 0)
                stringBuilder.AppendLine();

            return stringBuilder.Append($"{value}{Environment.NewLine}");
        }
    }

}
