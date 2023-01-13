using System;

namespace ArcDPS.Uploader.Events;

/// <summary>
/// A new report file has been created
/// </summary>
public class ReportCreatedEventArgs : EventArgs
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="path">Report file path</param>
    public ReportCreatedEventArgs(string path)
    {
        Path = path;
    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Report file path
    /// </summary>
    public string Path { get; }

    #endregion // Properties
}