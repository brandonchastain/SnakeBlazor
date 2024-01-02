using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Net.Http;
using SnakeLib.Contracts;
using System.Net.Http.Json;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SnakeLib;

namespace SnakeLib.Data
{

    public class HighScoreRepository : IHighScoreRepository
    {
        private HttpClient httpClient;
        private ILogger<SnakeGame> logger;

        public HighScoreRepository(ILogger<SnakeGame> logger, HttpClient client)
        {
            this.logger = logger;
            this.httpClient = client;
        }
        
        public async Task<IEnumerable<HighScore>> GetHighScores()
        {
            var response = await httpClient.GetAsync("");
            var jsonString = await response.Content.ReadAsStringAsync();
            var scores = JsonConvert.DeserializeObject<IEnumerable<HighScore>>(jsonString);
            return scores.OrderByDescending(s => s.Score).Take(5);
        }

        public async Task SaveHighScore(HighScore newHighScore)
        {
            await httpClient.PostAsJsonAsync("", newHighScore, CancellationToken.None);
        }
    }
}