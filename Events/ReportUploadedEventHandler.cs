namespace ArcDPS.Uploader.Events;

/// <summary>
/// A new report has been uploaded
/// </summary>
/// <param name="sender">Sender</param>
/// <param name="e">Arguments</param>
public delegate void ReportUploadedEventHandler(object sender, ReportUploadedEventArgs e);