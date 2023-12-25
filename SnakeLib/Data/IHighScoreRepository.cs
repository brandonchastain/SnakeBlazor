using System.Collections;
using System.Collections.Generic;
using SnakeLib.Contracts;

namespace SnakeLib.Data
{

    public interface IHighScoreRepository
    {
        IEnumerable<HighScore> GetHighScores();
    }
}