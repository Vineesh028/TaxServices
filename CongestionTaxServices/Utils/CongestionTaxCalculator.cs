
using CongestionTaxServices.Model;


namespace CongestionTaxServices.Utils
{

    public class CongestionTaxCalculator
    {

        /// <summary>
        /// Calculate the total toll fee for one day
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="dates"></param>
        /// <returns></returns>
        public int GetTax(Vehicle vehicle, DateTime[] dates)
        {
            if (vehicle == null) throw new System.Exception("Vehicle field is null");
            if (string.IsNullOrEmpty(vehicle.VehicleType)) throw new System.Exception("Vehicle type field is empty");
            String vehicleType = vehicle.VehicleType;
            // if(Enum.IsDefined(typeof(TollFreeVehicles), vehicleType)) throw new System.Exception("Invalid vehicle type"); 

            if (!Enum.TryParse<TollFreeVehicles>(vehicleType , out TollFreeVehicles v))
            {
                throw new System.Exception("Invalid vehicle type");
            }
            if (IsTollFreeVehicle(vehicleType)) return 0;

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
        /// <summary>
        /// Checking if the vehicle is tollfree
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        private bool IsTollFreeVehicle(string vehicleType)
        {

            return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString());
        }
        /// <summary>
        /// Getting toll fee for the time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        public int GetTollFee(DateTime date, Vehicle vehicle)
        {

            if (IsTollFreeDate(date)) return 0;

            TimeSpan time = date.TimeOfDay;

            List<TaxRate> intervalrates = JSONReader.cityTaxRates.Intervalrates;

            foreach (var rate in intervalrates)
            {
                if ((time >= rate.Start && time < rate.End)) return rate.Amount;
            }
            return 0;

        }
        /// <summary>
        /// Checking whether toll free date  
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private Boolean IsTollFreeDate(DateTime date)
        {

            DateTime day = date.Date;

            var holidays = new List<DateTime>(){new DateTime(2013, 1, 1),
            new DateTime(2013, 3, 28),
            new DateTime(2013, 3, 29),
            new DateTime(2013, 4, 1),
            new DateTime(2013, 4, 30),
            new DateTime(2013, 5, 1),
            new DateTime(2013, 5, 8),
            new DateTime(2013, 5, 9),
            new DateTime(2013, 6, 5),
            new DateTime(2013, 6, 6),
            new DateTime(2013, 6, 21),
            new DateTime(2013, 11, 1),
            new DateTime(2013, 12, 24),
            new DateTime(2013, 12, 25),
            new DateTime(2013, 12, 26),
            new DateTime(2013, 12, 31),
            };


            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
            foreach (var d in holidays)
            {
                if (day == d || day.Month == 7)
                {
                    return true;
                }

            }
            // int year = date.Year;
            // int month = date.Month;
            // int day = date.Day;

            // if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            // if (year == 2013)
            // {
            //     if (month == 1 && day == 1 ||
            //         month == 3 && (day == 28 || day == 29) ||
            //         month == 4 && (day == 1 || day == 30) ||
            //         month == 5 && (day == 1 || day == 8 || day == 9) ||
            //         month == 6 && (day == 5 || day == 6 || day == 21) ||
            //         month == 7 ||
            //         month == 11 && day == 1 ||
            //         month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            //     {
            //         return true;
            //     }
            // }
            return false;
        }

        private enum TollFreeVehicles
        {
            Motorcycle = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5,
            Car = 6
        }
    }



}