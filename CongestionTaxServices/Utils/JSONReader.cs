using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using CongestionTaxServices.Model;

namespace CongestionTaxServices.Utils {

  public static class JSONReader
{

    public static CityTaxRates cityTaxRates {get;set;}
    public static void readJSON(String cityJson)
        {

        using (StreamReader r = new StreamReader(Path.Combine(Environment.CurrentDirectory, cityJson)))
            {
                string json = r.ReadToEnd();

                cityTaxRates = JsonConvert.DeserializeObject<CityTaxRates>(json);
                Console.WriteLine("cityTaxRates.."+cityTaxRates.Currency);

            }
    }

}

}