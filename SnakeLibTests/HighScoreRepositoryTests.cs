using SnakeLib.Data;
using Moq;
using System.Net.Http;
namespace SnakeLibTests;

[TestClass]
public class HighScoreRepositoryTests
{
    [TestMethod]
    public async Task TestGetHighScores()
    {
        var repo = new HighScoreRepository(this.GetHttpClient());

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