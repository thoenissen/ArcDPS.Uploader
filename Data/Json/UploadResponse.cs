using Newtonsoft.Json;

namespace ArcDPS.Uploader.Data.Json;

/// <summary>
/// Upload
/// </summary>
public class UploadResponse
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Encounter
    /// </summary>
    [JsonProperty("encounter")]
    public Encounter Encounter { get; set; }

    /// <summary>
    /// Error
    /// </summary>
    [JsonProperty("error")]
    public string Error { get; set; }
}