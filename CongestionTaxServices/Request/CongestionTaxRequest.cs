using CongestionTaxServices.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CongestionTaxServices.Request;

public record CongestionTaxRequest([Required]Vehicle vehicle, [Required]DateTime[] dates);

