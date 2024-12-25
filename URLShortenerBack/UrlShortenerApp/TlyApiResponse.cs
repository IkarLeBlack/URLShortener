using System.Text.Json.Serialization;

namespace URL_Shortener;

public class TlyApiResponse
{
    [JsonPropertyName("short_url")]
    public string ShortUrl { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("long_url")]
    public string LongUrl { get; set; }

    [JsonPropertyName("domain")]
    public string Domain { get; set; }

    [JsonPropertyName("short_id")]
    public string ShortId { get; set; }

    [JsonPropertyName("public_stats")]
    public bool PublicStats { get; set; }

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }
}
