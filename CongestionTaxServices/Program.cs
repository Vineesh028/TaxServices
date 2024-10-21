using System.Globalization;
using CongestionTaxServices;
using CongestionTaxServices.Service;
using CongestionTaxServices.Services;
using CongestionTaxServices.TestException;
using CongestionTaxServices.Utils;



var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<ITaxService, CongestionTaxService>();
    JSONReader.readJSON(Environment.GetEnvironmentVariable("CITY_JSON"));
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});


var app = builder.Build();
app.UseExceptionHandler(opt => { });


// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.MapControllers();
app.UseHttpsRedirection();

app.Run();
public partial class Program { }

