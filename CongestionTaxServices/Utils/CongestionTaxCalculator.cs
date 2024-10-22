
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
            if (vehicle == null) throw new ArgumentException(Constants.VEHICLE_NULL_MESSAGE);
            
            if (string.IsNullOrEmpty(vehicle.VehicleType)) throw new ArgumentException(Constants.VEHICLE_TYPE_EMPTY_MESSAGE);
            
            String vehicleType = vehicle.VehicleType;
            if (!Enum.TryParse<TollFreeVehicles>(vehicleType , out TollFreeVehicles v))
            {
                throw new ArgumentException(Constants.INVALID_VEHICLE_TYPE_MESSAGE);
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
        /// Checking if the vehicle is tollfree
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        private bool IsTollFreeVehicle(string vehicleType)
        {

            return vehicleType.Equals(TollFreeVehicles.Motorcycle.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString())||
                   vehicleType.Equals(TollFreeVehicles.Bus.ToString());
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

            return false;
        }

    }



}