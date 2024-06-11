using Microsoft.AspNetCore.Mvc;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opts => opts.AddPolicy(name: "allowAll", builder => { builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("allowAll");
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing",
    "Bracing",
    "Chilly",
    "Cool",
    "Mild",
    "Warm",
    "Balmy",
    "Hot",
    "Sweltering",
    "Scorching"
};

app.MapGet(
        "/weatherforecast",
        ([FromQuery] int pageIndex = 1, int pageSize = 10) =>
        {
            var forecast = Enumerable
                .Range(1, 30)
                .Select(index => new ForecastDto
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Temperature = Random.Shared.Next(-10, 25),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .AsQueryable()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToArray();
            return new PagedData<ForecastDto> { TotalPages = 30/ pageSize, TotalRows= 30, Data =forecast};
        }
    )
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

public class ForecastDto
{
    public DateOnly Date { get; set; }
    public string? Summary { get; set; }
    public int Temperature { get; set; }
}

public class Query
{
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
}

public class PagedData<T>
{
    public int TotalRows { get; set; }
    public int TotalPages { get; set; }
    public T[] Data { get; set; } = [];
}