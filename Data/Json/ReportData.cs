using Newtonsoft.Json;

namespace ArcDPS.Uploader.Data.Json;

/// <summary>
/// Report data
/// </summary>
public class ReportData
{
    /// <summary>
    /// Name of the fight
    /// </summary>
    [JsonProperty("fightName")]
    public string FightName { get; set; }

    /// <summary>
    /// Icon if the fight
    /// </summary>
    [JsonProperty("fightIcon")]
    public string FightIcon { get; set; }
}