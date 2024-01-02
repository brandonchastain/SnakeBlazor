using Microsoft.Extensions.Logging;
using Moq;
using SnakeLib;
using SnakeLib.Data;
namespace SnakeLibTests;

[TestClass]
public class SnakeGameTests
{
    [TestMethod]
    public void Game_State_Is_NotStarted_After_Ctor()
    {
        var logger = new Mock<ILogger<SnakeGame>>().Object;
        var client = this.GetHttpClient();
        var game = new SnakeGame(logger, new HighScoreRepository(logger, client));
        
        Assert.IsTrue(game.GameState == GameState.NotStarted);
    }

    [TestMethod]
    public void Game_State_Is_InProgress_After_Pressing_Enter()
    {
        var logger = new Mock<ILogger<SnakeGame>>().Object;
        var playerInput = new Mock<IPlayerInput>();

        // simulate user input
        playerInput.Setup(p => p.GetEnterPressed()).Returns(true);

        var client = this.GetHttpClient();
        var game = new SnakeGame(logger, new HighScoreRepository(logger, client), playerInput.Object);

        Assert.IsTrue(game.GameState == GameState.NotStarted);

        game.ReadPlayerInput();
        Assert.IsTrue(game.GameState == GameState.InProgress);
    }

    [TestMethod]
    public void Game_State_Is_HighScores_After_Pressing_H()
    {
        var logger = new Mock<ILogger<SnakeGame>>().Object;
        var playerInput = new Mock<IPlayerInput>();

        // simulate user input
        playerInput.Setup(p => p.GetHPressed()).Returns(true);

        var client = this.GetHttpClient();
        var game = new SnakeGame(logger, new HighScoreRepository(logger, client), playerInput.Object);

        game.ReadPlayerInput();
        Assert.IsTrue(game.GameState == GameState.HighScores);
    }

    private HttpClient GetHttpClient()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://snakescores.azurewebsites.net/highscore/");
        return client;
    }
}