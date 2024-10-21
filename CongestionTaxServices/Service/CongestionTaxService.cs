using CongestionTaxServices.Request;
using CongestionTaxServices.Response;
using CongestionTaxServices.Services;
using CongestionTaxServices.Utils;

namespace CongestionTaxServices.Service
{

    public class CongestionTaxService : ITaxService
    {

        private readonly CongestionTaxCalculator _congestionTaxCalculator = new();

        /// <summary>
        /// Calculates tax and returs response
        /// </summary>
        /// <param name="congestionTaxRequest"></param>
        /// <returns></returns>
        public CongestionTaxResponse CalculateTax(CongestionTaxRequest congestionTaxRequest)
        {
            var response = new CongestionTaxResponse(_congestionTaxCalculator.GetTax(congestionTaxRequest.vehicle, congestionTaxRequest.dates).ToString()+ JSONReader.cityTaxRates.Currency);
        
            return response;
        }
    }

}