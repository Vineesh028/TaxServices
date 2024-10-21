using CongestionTaxServices.Model;
using System;

namespace CongestionTaxServices.Request;

public record CongestionTaxRequest(Vehicle vehicle, DateTime[] dates);

//DateTime myDate = DateTime.ParseExact(expiredDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);