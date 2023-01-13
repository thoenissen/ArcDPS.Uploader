namespace ArcDPS.Uploader.Events;

/// <summary>
/// A new report is available
/// </summary>
/// <param name="sender">Sender</param>
/// <param name="e">Argument</param>
public delegate void ReportAvailableEventHandler(object sender, ReportAvailableEventArgs e);