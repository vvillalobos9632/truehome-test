namespace Common.DTOs.Property
{
    public class PropertyData 
	{
       public int id { get; set; }
       public string title { get; set; }
       public string address { get; set; }
       public string description { get; set; }
       public DateTime create_at { get; set; }
       public DateTime update_at { get; set; }
       public DateTime? disable_at { get; set; }
       public string status { get; set; }
	}
}