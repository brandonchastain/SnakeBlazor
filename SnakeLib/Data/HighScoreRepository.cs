using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Net.Http;
using SnakeLib.Contracts;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SnakeLib.Data
{

    public class HighScoreRepository : IHighScoreRepository
    {
        private HttpClient httpClient;
        public HighScoreRepository(IHttpClientFactory clientFactory)
        {
            this.httpClient = clientFactory.CreateClient();
        }
        
        public async Task<IEnumerable<HighScore>> GetHighScores()
        {			
            var scores = await httpClient.GetFromJsonAsync<IEnumerable<HighScore>>("https://snakescores.azurewebsites.net/highscore/");
            return scores;
            //return new List<HighScore>(){ new HighScore() };
        }

        public void SaveHighScores(IEnumerable<HighScore> scores)
        {
        }
    }
}