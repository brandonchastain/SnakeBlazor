using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Net.Http;
using SnakeLib.Contracts;
using System.Net.Http.Json;
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
            logger.LogInformation("Sending request...");
            var response = await httpClient.GetAsync("");

            logger.LogInformation("Reading response...");
            var jsonString = await response.Content.ReadAsStringAsync();
            logger.LogInformation("Parsing json...");
            return JsonConvert.DeserializeObject<IEnumerable<HighScore>>(jsonString);
        }

        public async Task SaveHighScore(HighScore newHighScore)
        {
            await httpClient.PostAsJsonAsync("", newHighScore, CancellationToken.None);
        }
    }
}