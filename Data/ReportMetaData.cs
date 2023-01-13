using System;

namespace ArcDPS.Uploader.Data;

/// <summary>
/// Report meta data
/// </summary>
public class ReportMetaData
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="bossName">Boss name</param>
    /// <param name="isCm">Challenge mode</param>
    /// <param name="isSuccess">Success</param>
    /// <param name="duration">Duration</param>
    public ReportMetaData(string id, string bossName, bool isCm, bool isSuccess, long? duration)
    {
        Id = id;
        BossName = bossName;
        IsCm = isCm;
        IsSuccess = isSuccess;
        Duration = duration;
    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Time stamp
    /// </summary>
    public DateTime TimeStamp { get; } = DateTime.Now;

    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Boss name
    /// </summary>
    public string BossName { get; }

    /// <summary>
    /// Challenge mode
    /// </summary>
    public bool? IsCm { get; }

    /// <summary>
    /// Success
    /// </summary>
    public bool? IsSuccess { get; }

    /// <summary>
    /// Duration
    /// </summary>
    public long? Duration { get; }

    #endregion // Properties
}