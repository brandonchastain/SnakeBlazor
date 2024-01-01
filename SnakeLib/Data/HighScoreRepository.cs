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

namespace SnakeLib.Data
{

    public class HighScoreRepository : IHighScoreRepository
    {
        private HttpClient httpClient;
        public HighScoreRepository(HttpClient client)
        {
            this.httpClient = client;
        }
        
        public async Task<IEnumerable<HighScore>> GetHighScores()
        {			
            var response = await httpClient.GetAsync("");
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<HighScore>>(jsonString);
        }

        public async void SaveHighScore(HighScore newHighScore)
        {
            await httpClient.PostAsJsonAsync("", newHighScore, CancellationToken.None);
        }
    }
}