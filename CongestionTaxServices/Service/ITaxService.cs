

using CongestionTaxServices.Request;
using CongestionTaxServices.Response;

namespace CongestionTaxServices.Services;

public interface ITaxService
{
    CongestionTaxResponse CalculateTax(CongestionTaxRequest congestionTaxRequest);
}