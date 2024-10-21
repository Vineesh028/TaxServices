using CongestionTaxServices.Request;
using CongestionTaxServices.Response;
using CongestionTaxServices.Services;
using Microsoft.AspNetCore.Mvc;
namespace CongestionTaxServices.Controller
{
    [Route("congestion-tax")]
    [ApiController]
    public class CongestionTaxController : ControllerBase
    {

        private readonly ITaxService _congestionTaxService;

        public CongestionTaxController(ITaxService congestionTaxService)
        {
            _congestionTaxService = congestionTaxService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CalculateCongestionTax([FromBody] CongestionTaxRequest congestionTaxRequest)
        {
            if (congestionTaxRequest == null)
                return BadRequest(ModelState);
         
            var response  = _congestionTaxService.CalculateTax(congestionTaxRequest);

            return Ok(response);
        }

    }

}