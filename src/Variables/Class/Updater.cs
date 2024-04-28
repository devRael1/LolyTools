using Newtonsoft.Json;

namespace Loly.src.Variables.Class;

internal class UpdaterResponse
{
    [JsonProperty("tag_name")] internal string TagName { get; set; }
    internal List<Asset> Assets { get; set; }
}

internal class Asset
{
    internal long Size { get; set; }
    [JsonProperty("download_count")] internal int DownloadCount { get; set; }
    [JsonProperty("created_at")] internal string CreatedAt { get; set; }
    [JsonProperty("browser_download_url")] internal string BrowserDownloadUrl { get; set; }
}