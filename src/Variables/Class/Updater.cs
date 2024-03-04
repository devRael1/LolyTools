using Newtonsoft.Json;

namespace Loly.src.Variables.Class
{
    public class UpdaterResponse
    {
        [JsonProperty("tag_name")] public string TagName { get; set; }
        public List<Asset> Assets { get; set; }
    }

    public class Asset
    {
        public long Size { get; set; }
        [JsonProperty("download_count")] public int DownloadCount { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("browser_download_url")] public string BrowserDownloadUrl { get; set; }
    }
}
