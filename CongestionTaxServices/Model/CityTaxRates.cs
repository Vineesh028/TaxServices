namespace CongestionTaxServices.Model
{
    public class CityTaxRates
    {
       public String Currency { get; set; }
       public List<TaxRate> Intervalrates { get; set; }

    }
}