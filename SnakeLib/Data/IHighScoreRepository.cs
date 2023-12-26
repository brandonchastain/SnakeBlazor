using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SnakeLib.Contracts;

namespace SnakeLib.Data
{

    public interface IHighScoreRepository
    {
        Task<IEnumerable<HighScore>> GetHighScores();
    }
}