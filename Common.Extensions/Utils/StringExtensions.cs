using System.Globalization;

namespace Common.Extensions.Utils
{
    public static class StringExtensions
    {
        public static DateTime ConvertToDateTime(this string dateTimeString)
        {
            var result = new DateTime();
            try
            {
                result = DateTime.ParseExact(dateTimeString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

                try
                {
                    result = DateTime.ParseExact(dateTimeString, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {

                    try
                    {
                        result = DateTime.ParseExact(dateTimeString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return result;
        }
    }
}
