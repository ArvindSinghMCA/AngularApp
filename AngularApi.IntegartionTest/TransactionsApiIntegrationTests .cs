using System.Net;
using System.Net.Http.Json;
using AngularApp.Server.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

public class TransactionsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TransactionsApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        // Create HttpClient to call the in-memory API server
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostTransaction_ValidTransaction_ReturnsSuccessMessage()
    {
        // Arrange
        var transaction = new Transaction
        {
            TradeId = 1,
            Version = 1,
            SecurityCode = "REL",
            Quantity = 50,
            Action = "INSERT",
            BuySell = "BUY"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/transactions", transaction);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        content.Should().ContainKey("message");
        content["message"].Should().Be("Transaction processed successfully.");
    }

    [Fact]
    public async Task GetPositions_AfterPostingTransaction_ReturnsCorrectPosition()
    {
        // Arrange
        var transaction = new Transaction
        {
            TradeId = 2,
            Version = 1,
            SecurityCode = "XYZ",
            Quantity = 100,
            Action = "INSERT",
            BuySell = "BUY"
        };

        await _client.PostAsJsonAsync("/api/transactions", transaction);

        // Act
        var response = await _client.GetAsync("/api/transactions/positions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var positions = await response.Content.ReadFromJsonAsync<List<Position>>();

        positions.Should().NotBeNull();
        positions.Should().ContainSingle(p => p.SecurityCode == "XYZ" && p.Quantity == 100);
    }
}
