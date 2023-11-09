using Microsoft.AspNetCore.Mvc;
using TestableApi.Api.Models;

namespace TestableApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TimeZoneController : ControllerBase
{
    private readonly ILogger<TimeZoneController> _logger;

    private readonly List<TimeZoneModel> timeZones = new()
    {
        new() {CountryName = "France", Timezone = "UTC+01:00"},
        new() {CountryName = "Belgique"},
        new() {CountryName = "Russie", Timezone = "UTC+03:00"},
        new() {CountryName = "Chine", Timezone = "UTC+08:00"},
        new() {CountryName = "Costa Rica", Timezone = "UTC-06:00"},
    };

    public TimeZoneController(ILogger<TimeZoneController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetTimeZone/{CountryName}")]
    public IActionResult Get(string CountryName)
    {
        if (string.IsNullOrWhiteSpace(CountryName)) {
            return BadRequest();
        }

        CountryName = CountryName.Trim();

        TimeZoneModel ?Country = timeZones.Find(Country => Country.CountryName.Equals(CountryName));

        if (Country is null) {
            return NotFound("Ce pays n'est pas connu.");
        } else if (Country.Timezone is null) {
            return NotFound("Aucune timezone renseignée pour ce pays.");
        }

        return Ok(Country.Timezone);
    }
}
