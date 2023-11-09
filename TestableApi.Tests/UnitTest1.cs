namespace TestableApi.Tests;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

public class UnitTest1
{
    //Scenario : Je demande un fuseau horaire avec un mauvais input
    [Fact]
    public async Task GetTimezoneBadRequest()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("TimeZone/GetTimeZone/%20%20");
        string stringResponse = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    //Scenario : Je récupère le fuseau horaire d'un pays inexistant
    [Fact]
    public async Task GetUnknownCountry()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("TimeZone/GetTimeZone/_");
        string stringResponse = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Ce pays n'est pas connu.", stringResponse);
    }

    //Scenario : Je demande un pays qui n'a pas son fuseau horaire renseigné
    [Fact]
    public async Task GetEmptyTimeZone()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("TimeZone/GetTimeZone/Belgique");
        string stringResponse = await response.Content.ReadAsStringAsync();

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Aucune timezone renseignée pour ce pays.", stringResponse);
    }

    //Scenario : Je récupère le fuseau horaire d'un pays qui existe
    [Fact]
    public async Task GetExistingTimeZone()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("TimeZone/GetTimeZone/France");
        string stringResponse = await response.Content.ReadAsStringAsync();

        //Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("UTC+01:00", stringResponse);
    }
}