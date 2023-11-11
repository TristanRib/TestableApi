namespace TestableApi.Tests;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

public class UnitTest1
{
    //Scenario : Je demande un fuseau horaire avec un mauvais input
    /*
    Scenario : Je récupère le fuseau horaire d'un pays inexistant
    Given - le pays que je demande est '  '
    When - demande le fuseau avec un mauvais input
    Then - 400 - Pays introuvable
    */
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

    /*
    Scenario : Je récupère le fuseau horaire d'un pays inexistant
    Given - le pays que je demande est 'test'
    When - demande le fuseau avec un pays inexistant
    Then - 404/400 - Ce pays n'est pas connu
    */
    [Fact]
    public async Task GetUnknownCountry()
    {
        await using var _factory = new WebApplicationFactory<Program>();
        var client = _factory.CreateClient();

        var response = await client.GetAsync("TimeZone/GetTimeZone/test");
        string stringResponse = await response.Content.ReadAsStringAsync();
        
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Ce pays n'est pas connu.", stringResponse);
    }

    /*
    Scenario : Je récupère le fuseau horaire d'un pays qui n'a pas son fuseau horaire
    Given - le pays que je demande est 'Belgique'
    When - demande le fuseau avec un pays sans fuseau horaire
    Then - 404 - Aucune timezone renseignée pour ce pays
    */
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

    /*
    Scenario : Je récupère le fuseau horaire d'un pays qui possède son fuseau horaire renseigné
    Given - le pays que je demande est 'France'
    When - demande le fuseau avec un pays renseigné
    Then - 202 - Timezone
    */
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