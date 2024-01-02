using SnakeLib.Data;
using SnakeLib;
using Moq;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace SnakeLibTests;

[TestClass]
public class HighScoreRepositoryTests
{
    [TestMethod]
    public async Task TestGetHighScores()
    {
        var repo = new HighScoreRepository(new Mock<ILogger<SnakeGame>>().Object, this.GetHttpClient());

        var scores = await repo.GetHighScores();
        Assert.IsNotNull(scores.First().ETag);
    }

    private HttpClient GetHttpClient()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://snakescores.azurewebsites.net/highscore/");
        return client;
    }
}