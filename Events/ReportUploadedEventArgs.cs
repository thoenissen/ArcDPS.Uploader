using ArcDPS.Uploader.Data;

namespace ArcDPS.Uploader.Events;

/// <summary>
/// A new report has been uploaded
/// </summary>
public class ReportUploadedEventArgs
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="report">Report</param>
    public ReportUploadedEventArgs(ReportMetaData report)
    {
        Report = report;
    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Report
    /// </summary>
    public ReportMetaData Report { get; set; }

    #endregion // Properties
}