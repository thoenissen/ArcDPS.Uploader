using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using ArcDPS.Uploader.Data;
using ArcDPS.Uploader.Data.Json;
using ArcDPS.Uploader.Events;

using Newtonsoft.Json;

namespace ArcDPS.Uploader.Services;

/// <summary>
/// Uploading of reports to dps.report
/// </summary>
public class ReportUploader
{
    #region Events

    /// <summary>
    /// A new report has been uploaded
    /// </summary>
    public event ReportUploadedEventHandler ReportUploaded;

    #endregion // Events

    #region Properties

    /// <summary>
    /// User token
    /// </summary>
    public string UserToken { get; set; }

    /// <summary>
    /// Is everything configured for uploading reports?
    /// </summary>
    public bool IsUploadConfigured => string.IsNullOrWhiteSpace(UserToken) == false;

    #endregion // Properties

    #region Methods

    /// <summary>
    /// Upload new report
    /// </summary>
    /// <param name="path">Path of the report file</param>
    /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
    public async Task UploadFileAsync(string path)
    {
        if (IsUploadConfigured)
        {
            var jsonResponse = await PostFileToDpsReport(path);
            if (string.IsNullOrWhiteSpace(jsonResponse) == false)
            {
                var data = JsonConvert.DeserializeObject<UploadResponse>(jsonResponse);

                if (IsUploadSuccessful(data))
                {
                    RaiseReportUploaded(data);
                }
            }
        }
    }

    /// <summary>
    /// Post file to dps.report
    /// </summary>
    /// <param name="path">Path of the report file</param>
    /// <returns>Response content</returns>
    private async Task<string> PostFileToDpsReport(string path)
    {
        using var httpClient = new HttpClient();
        using var content = new MultipartFormDataContent();
        using var contentStream = new StreamContent(File.OpenRead(path));

        content.Add(contentStream, "file", Path.GetFileName(path));

        var response = await httpClient.PostAsync($"https://dps.report/uploadContent?json=1&generator=ei&userToken={Uri.EscapeUriString(UserToken)}", content);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return null;
    }

    /// <summary>
    /// Checking if the report hast been successfully uploaded
    /// </summary>
    /// <param name="response">Upload data</param>
    /// <returns>Has the report successfully been uploaded?</returns>
    private bool IsUploadSuccessful(UploadResponse response)
    {
        return response != null
            && string.IsNullOrWhiteSpace(response.Error);
    }

    /// <summary>
    /// Raise the <see cref="ReportUploaded"/> event
    /// </summary>
    /// <param name="response">Upload data</param>
    private void RaiseReportUploaded(UploadResponse response)
    {
        ReportUploaded?.Invoke(this,
                               new ReportUploadedEventArgs(new ReportMetaData(response.Id,
                                                                              response.Encounter?.Boss,
                                                                              response.Encounter?.IsCm ?? false,
                                                                              response.Encounter?.Success ?? false,
                                                                              (long)(response.Encounter?.Duration ?? 0d))));
    }

    #endregion // Methods
}