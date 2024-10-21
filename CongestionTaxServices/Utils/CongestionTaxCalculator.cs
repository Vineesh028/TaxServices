
using CongestionTaxServices.Model;


namespace CongestionTaxServices.Utils {

    public class CongestionTaxCalculator
{
    /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day
         * @return - the total congestion tax for that day
         */

    public int GetTax(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            long minutes = diffInMillies / 1000 / 60;

            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.VehicleType;
        return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {

        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        TimeSpan time = date.TimeOfDay;

        List<TaxRate> intervalrates  = JSONReader.cityTaxRates.Intervalrates;

        foreach (var rate in intervalrates) {
            if((time>= rate.Start && time<rate.End)) return rate.Amount;
        }
        return 0;

    //     TimeSpan timeSpan1 = TimeSpan.Parse("06:00:00"); 
    //     TimeSpan timeSpan2 = TimeSpan.Parse("06:30:00"); 
    //     TimeSpan timeSpan3 = TimeSpan.Parse("07:00:00"); 
    //     TimeSpan timeSpan4 = TimeSpan.Parse("08:00:00"); 
    //     TimeSpan timeSpan5 = TimeSpan.Parse("08:30:00"); 
    //     TimeSpan timeSpan6 = TimeSpan.Parse("15:00:00"); 
    //     TimeSpan timeSpan7 = TimeSpan.Parse("15:30:00"); 
    //     TimeSpan timeSpan8 = TimeSpan.Parse("17:00:00"); 
    //     TimeSpan timeSpan9 = TimeSpan.Parse("18:00:00"); 
    //     TimeSpan timeSpan10 = TimeSpan.Parse("18:30:00"); 

        
    //    if((time>=timeSpan1&&time<timeSpan2)||(time>=timeSpan5&&time<timeSpan6)||(time>=timeSpan9&&time<timeSpan10)) return 8;
    //    else if ((time>=timeSpan2&&time<timeSpan3)||(time>=timeSpan4&&time<timeSpan5)||(time>=timeSpan6&&time<timeSpan7)||(time>=timeSpan8&&time<timeSpan9)) return 13;
    //    else if ((time>=timeSpan3&&time<timeSpan4)||(time>=timeSpan7&&time<timeSpan8)) return 18;
    //    else return 0;

        // int hour = date.Hour;
        // int minute = date.Minute;

        // if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        // else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        // else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        // else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        // else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        // else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        // else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        // else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        // else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        // else return 0;
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }

    private enum TollFreeVehicles
    {
        Motorcycle = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}



}