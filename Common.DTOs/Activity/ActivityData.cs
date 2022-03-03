namespace Common.DTOs.Activity
{
    public class ActivityData
    {
        public int id { get; set; }
        public int property_id { get; set; }
        public string scheduleString { get; set; }
        public DateTime? schedule { get; set; }
        public string title { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_ad { get; set; }
        public string? status { get; set; }

        public string Condition
        {
            get
            {
                if (status == null || status == "CANCELADA")
                    return "";

                if (status == "REALIZADA")
                    return "FINALIZADA";

                var currentDate = DateTime.Now;

                if (schedule.GetValueOrDefault() < currentDate)
                    return "ATRASADA";

                return "PENDIENTE DE REALIZAR";
            }
        }
    }
}