namespace CongestionTaxServices.Model
{
    public class TaxRate
    {
       public TimeSpan Start { get; set; }
       public TimeSpan End { get; set; }
       public int Amount { get; set; }

    }
}