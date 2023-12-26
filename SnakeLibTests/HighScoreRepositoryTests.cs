using SnakeLib.Data;
namespace SnakeLibTests;

[TestClass]
public class HighScoreRepositoryTests
{
    [TestMethod]
    public async Task TestGetHighScores()
    {
        var repo = HighScoreRepository.Instance;
        var scores = await repo.GetHighScores();
        Assert.IsNotNull(scores);
    }
}