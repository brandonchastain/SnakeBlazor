namespace SnakeLib.Contracts
{
    using System;
    using Azure;
    using Azure.Data.Tables;
    using Newtonsoft.Json;
    
    public class HighScore : ITableEntity
    {
        public string Username { get; set; }
        public int Score { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

        [JsonProperty("@odata.etag")]
        public ETag ETag { get; set; }
    }
}