using Newtonsoft.Json;

namespace ArcDPS.Uploader.Data.Json;

/// <summary>
/// Encounter
/// </summary>
public class Encounter
{
    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Duration
    /// </summary>
    [JsonProperty("duration")]
    public double Duration { get; set; }

    /// <summary>
    /// Boss
    /// </summary>
    [JsonProperty("boss")]
    public string Boss { get; set; }

    /// <summary>
    /// Challenge mode
    /// </summary>
    [JsonProperty("isCm")]
    public bool IsCm { get; set; }

    /// <summary>
    /// Are additional information as json available?
    /// </summary>
    [JsonProperty("jsonAvailable")]
    public bool JsonAvailable { get; set; }
}