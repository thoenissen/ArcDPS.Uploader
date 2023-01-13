namespace ArcDPS.Uploader.Data;

/// <summary>
/// Report container
/// </summary>
public class ReportContainer
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="meta">Meta data</param>
    /// <param name="details">Details</param>
    public ReportContainer(ReportMetaData meta, ReportDetailedData details)
    {
        Meta = meta;
        Details = details;
    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Meta data of the report
    /// </summary>
    public ReportMetaData Meta { get; }

    /// <summary>
    /// Detailed report information
    /// </summary>
    public ReportDetailedData Details { get; }

    #endregion // Properties
}