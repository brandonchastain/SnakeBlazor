using Microsoft.Extensions.Logging;
using Moq;
using SnakeLib;
namespace SnakeLibTests;

[TestClass]
public class SnakeGameTests
{
    [TestMethod]
    public void Game_State_Is_NotStarted_After_Ctor()
    {
        var logger = new Mock<ILogger<SnakeGame>>().Object;
        var factory = new Mock<IHttpClientFactory>().Object;
        var game = new SnakeGame(logger);
        
        Assert.IsTrue(game.GameState == GameState.NotStarted);
    }

    [TestMethod]
    public void Game_State_Is_Initializing_After_First_Update()
    {
        var logger = new Mock<ILogger<SnakeGame>>().Object;
        
        bool hPressed = false;
        bool enterPressed = true;
        var playerInput = new Mock<IPlayerInput>();
        playerInput.Setup(p => p.GetHPressed()).Returns(() => hPressed);
        playerInput.Setup(p => p.GetEnterPressed()).Returns(() => enterPressed);

        var factory = new Mock<IHttpClientFactory>().Object;
        var game = new SnakeGame(logger, new HighScoreRepository(factory)m playerInput.Object);
        Assert.IsTrue(game.GameState == GameState.NotStarted);

        game.ReadPlayerInput();
        //game.Update();
        Assert.IsTrue(game.GameState == GameState.Initializing);

        enterPressed = false;
        hPressed = true;
        game.ReadPlayerInput();
        game.Update();
        Assert.IsTrue(game.GameState == GameState.HighScores);
    }
}