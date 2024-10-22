using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CongestionTaxServices.Utils
{

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        /// <summary>
        /// Read DateTime in the given format
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            DateTime dateTime = new DateTime();
            try
            {
                dateTime = DateTime.ParseExact(reader.GetString(), Constants.DATETIME_FORMAT, CultureInfo.InvariantCulture);
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(Constants.INVALID_DATETIME_MESSAGE);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException(Constants.INVALID_DATETIME_FORMAT_MESSAGE);
            }
            return dateTime;
        }
        /// <summary>
        /// Write time value in the given format
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString(Constants.DATETIME_FORMAT));
        }
    }

}