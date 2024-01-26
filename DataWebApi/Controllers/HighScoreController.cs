using Azure;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using SnakeLib.Contracts;

namespace DataWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HighScoreController : ControllerBase
{
    private const string StorageUri = "https://snakescores.table.core.windows.net/";
    private const string AccountName = "snakescores";
    private static readonly string StorageAccountKey = Environment.GetEnvironmentVariable("BsnakeStorageAccountKey") ?? string.Empty;
    
    private readonly ILogger<HighScoreController> _logger;
    private readonly TableServiceClient serviceClient;
    private readonly TableClient tableClient;

    public HighScoreController(ILogger<HighScoreController> logger)
    {
        _logger = logger;

        serviceClient = new TableServiceClient(
            new Uri(StorageUri),
            new TableSharedKeyCredential(AccountName, StorageAccountKey));

        tableClient = serviceClient.GetTableClient("HighScores");
    }

    [HttpGet]
    public async Task<IEnumerable<HighScore>> Get()
    {
        var queryResultsFilter = tableClient.QueryAsync<HighScore>(maxPerPage: 5);
        var res = new List<HighScore>();
        // Iterate the <see cref="Pageable"> to access all queried entities.
        await foreach (HighScore score in queryResultsFilter)
        {
            res.Add(score);
        }

        return res;
    }

    [HttpPost]
    public ActionResult<HighScore> Post(HighScore newHighScore)
    {
        tableClient.AddEntity(newHighScore);
        return Ok();
    }
}
