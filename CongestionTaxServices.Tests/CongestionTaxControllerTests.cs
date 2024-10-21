using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using ApiTest.Fixtures;
using CongestionTaxServices.Model;
using CongestionTaxServices.Request;
using CongestionTaxServices.Response;
using CongestionTaxServices.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;
using Xunit.Abstractions;

namespace ApiTest
{

    public class CongestionTaxControllerTests(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        

    //   public class CongestionTaxControllerTests :IClassFixture<WebApplicationFactory<Program>>
    //  {
    //     readonly HttpClient _client;

    // //    // private readonly ITestOutputHelper output;
    //     public CongestionTaxControllerTests(WebApplicationFactory<Program> application, ITestOutputHelper output){
           
    //         _client = application.CreateClient();
    //         Environment.SetEnvironmentVariable("CITY_JSON", "Gothenburg.json");
    //     }

        

        [Fact]
        public async void CalculateSuccessTest()
        {

  
            // Console.SetOut(new ConsoleWriter(output));
            // Assert.True(ToBeTested.Foo("content :" + jsonContent));

 
            var jsonContent = "{\"vehicle\":{\"vehicleType\":\"Car\"},\"dates\":[\"2013-01-15 21:00:00\",\"2013-02-07 06:23:27\",\"2013-02-07 15:27:00\",\"2013-02-08 06:27:00\",\"2013-02-08 06:20:27\",\"2013-02-08 14:35:00\"]}";
  
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/congestion-tax", content);
            
            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadFromJsonAsync<CongestionTaxResponse>();
                responseContent.Should().NotBeNull();
                responseContent.tax.Should().Be("45SEK");


            }
            else
            {
                Assert.Fail("Api call failed.");
            }
        }

        [Fact]
        public async void CalculateMaxTest()
        {

            var jsonContent = "{\"vehicle\":{\"vehicleType\":\"Car\"},\"dates\":[\"2013-02-08 06:27:00\",\"2013-02-08 06:20:27\",\"2013-02-08 14:35:00\",\"2013-02-08 15:29:00\",\"2013-02-08 15:47:00\",\"2013-02-08 16:01:00\",\"2013-02-08 16:48:00\",\"2013-02-08 15:48:00\",\"2013-02-08 16:18:00\"]}";
  
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/congestion-tax", content);
            
            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadFromJsonAsync<CongestionTaxResponse>();
                responseContent.Should().NotBeNull();
                responseContent.tax.Should().Be("60SEK");

            }
            else
            {
                Assert.Fail("Api call failed.");
            }
        }

        [Fact]
        public async void CalculateonWeekendHolidayJulyTest()
        {

            var jsonContent = "{\"vehicle\":{\"vehicleType\":\"Car\"},\"dates\":[\"2013-01-05 15:45:00\",\"2013-03-28 06:27:00\",\"2013-07-12 14:35:00\"]}";
  
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/congestion-tax", content);
            
            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadFromJsonAsync<CongestionTaxResponse>();
                responseContent.Should().NotBeNull();
                responseContent.tax.Should().Be("0SEK");

            }
            else
            {
                Assert.Fail("Api call failed.");
            }
        }

        [Fact]
        public async void TaxExemptionTest()
        {

            var jsonContent = "{\"vehicle\":{\"vehicleType\":\"Motorcycle\"},\"dates\":[\"2013-01-15 21:00:00\",\"2013-02-07 06:23:27\",\"2013-02-07 15:27:00\",\"2013-02-08 06:27:00\",\"2013-02-08 06:20:27\",\"2013-02-08 14:35:00\"]}";
  
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/congestion-tax", content);
            
            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadFromJsonAsync<CongestionTaxResponse>();
                responseContent.Should().NotBeNull();
                responseContent.tax.Should().Be("0SEK");


            }
            else
            {
                Assert.Fail("Api call failed.");
            }
        }

        [Fact]
        public async void SingleChargeRuleTest()
        {

            var jsonContent = "{\"vehicle\":{\"vehicleType\":\"Car\"},\"dates\":[\"2013-02-07 08:23:27\",\"2013-02-07 08:43:27\"]}";
  
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/congestion-tax", content);
            
            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadFromJsonAsync<CongestionTaxResponse>();
                responseContent.Should().NotBeNull();
                responseContent.tax.Should().Be("13SEK");


            }
            else
            {
                Assert.Fail("Api call failed.");
            }
        }
    }

    public class ConsoleWriter : StringWriter
        {
            private ITestOutputHelper output;
            public ConsoleWriter(ITestOutputHelper output)
            {
                this.output = output;
            }

            public override void WriteLine(string m)
            {
                output.WriteLine(m);
            }
        }

           public class ToBeTested
        {
            public static bool Foo(String s)
            {
                Console.WriteLine(s);
                return true;
            }
        }


}