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
        private static readonly string FilePath = @"%HOME%\highscores.csv";

        private HighScoreRepository()
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
                File.WriteAllLines(FilePath, new[] { "BCC,200" });
            }
        }
        public IEnumerable<HighScore> GetHighScores()
        {
            var scores = new List<HighScore>();
            string[] contents = File.ReadAllLines(FilePath);

            foreach (string line in contents)
            {
                string[] parts = line.Split(",");
                string username = parts[0];
                int score = int.Parse(parts[1]);

                scores.Add(new HighScore()
                {
                    Username = username,
                    Score = score,
                });
            }

            return scores;
        }

        public void SaveHighScores(IEnumerable<HighScore> scores)
        {
            var sb = new StringBuilder();
            foreach (var score in scores)
            {
                sb.Append($"{score.Username},{score.Score}\n");
            }

            File.WriteAllText(FilePath, sb.ToString());
        }
    }
}