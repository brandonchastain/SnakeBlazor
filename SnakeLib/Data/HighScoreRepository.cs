using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using SnakeLib.Contracts;

namespace SnakeLib.Data
{

    public class HighScoreRepository : IHighScoreRepository
    {
        public static HighScoreRepository Instance = new HighScoreRepository();

        private HighScoreRepository()
        {
        }
        
        public IEnumerable<HighScore> GetHighScores()
        {
            var a = new HighScore();
            a.Username = "abc";
            a.Score = 200;
            return new List<HighScore>(){a};
        }

        public void SaveHighScores(IEnumerable<HighScore> scores)
        {
        }
    }
}