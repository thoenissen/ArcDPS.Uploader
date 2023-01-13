namespace ArcDPS.Uploader.Events;

/// <summary>
/// A new report file has been created
/// </summary>
/// <param name="sender">Sender</param>
/// <param name="e">Argument</param>
public delegate void ReportCreatedEventHandler(object sender, ReportCreatedEventArgs e);