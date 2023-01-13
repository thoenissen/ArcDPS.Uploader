using System;

using ArcDPS.Uploader.Data;

namespace ArcDPS.Uploader.Events;

/// <summary>
/// A new report is available
/// </summary>
public class ReportAvailableEventArgs : EventArgs
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="report">Report</param>
    public ReportAvailableEventArgs(ReportContainer report)
    {
        Report = report;
    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Report
    /// </summary>
    public ReportContainer Report { get; }

    #endregion // Properties
}