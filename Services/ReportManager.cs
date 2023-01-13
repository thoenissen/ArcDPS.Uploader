using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using ArcDPS.Uploader.Data;
using ArcDPS.Uploader.Data.Json;
using ArcDPS.Uploader.Events;

using Blish_HUD;

using Newtonsoft.Json;

namespace ArcDPS.Uploader.Services;

/// <summary>
/// Report manager
/// </summary>
public class ReportManager
{
    #region Fields

    /// <summary>
    /// Logging
    /// </summary>
    private readonly Logger _logger;

    /// <summary>
    /// Uploads
    /// </summary>
    private readonly  List<ReportContainer> _uploads;

    /// <summary>
    /// Current report watcher
    /// </summary>
    private ReportDirectoryWatcher _reportWatcher;

    /// <summary>
    /// Current uploader;
    /// </summary>
    private ReportUploader _reportUploader;

    #endregion // Fields

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public ReportManager()
    {
        _uploads = new List<ReportContainer>();
        _logger = Logger.GetLogger<ReportManager>();
    }

    #endregion // Constructor

    #region Events

    /// <summary>
    /// A new report is available
    /// </summary>
    public event ReportAvailableEventHandler ReportAvailable;

    #endregion // Events

    #region Public methods

    /// <summary>
    /// Setting new report directory path
    /// </summary>
    /// <param name="path">Reports directory path</param>
    public void SetDirectory(string path)
    {
        if (_reportWatcher != null)
        {
            _reportWatcher.ReportCreated -= OnReportCreated;
            _reportWatcher.Dispose();
        }

        _reportWatcher = new ReportDirectoryWatcher(path);
        _reportWatcher.ReportCreated += OnReportCreated;
    }

    /// <summary>
    /// Setting user token to upload reports to dps.report
    /// </summary>
    /// <param name="userToken"></param>
    public void SetUserToken(string userToken)
    {
        if (_reportUploader == null)
        {
            _reportUploader = new ReportUploader();
            _reportUploader.ReportUploaded += OnReportUploaded;
        }

        _reportUploader.UserToken = userToken;
    }

    /// <summary>
    /// Get current uploads
    /// </summary>
    /// <returns>List of uploads</returns>
    public List<ReportContainer> GetUploads()
    {
        lock (_uploads)
        {
            return _uploads.ToList();
        }
    }

    #endregion // Public methods

    #region Private methods

    /// <summary>
    /// A new report file has been created
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    private async void OnReportCreated(object sender, ReportCreatedEventArgs e)
    {
        if (_reportUploader != null)
        {
            try
            {
                await _reportUploader.UploadFileAsync(e.Path);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "Failed to upload report ({0}).", e.Path);
            }
        }
    }

    /// <summary>
    /// A new report has been uploaded
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    private void OnReportUploaded(object sender, ReportUploadedEventArgs e)
    {
        AddUploadAsync(e.Report);
    }

    /// <summary>
    /// Add Upload
    /// </summary>
    /// <param name="metaData"></param>
    /// <returns></returns>
    private async void AddUploadAsync(ReportMetaData metaData)
    {
        var details = await GetReportDetails(metaData.Id);
        var container = new ReportContainer(metaData, details);

        lock (_uploads)
        {
            _uploads.Add(container);

            RaiseReportAvailable(container);
        }
    }

    /// <summary>
    /// Raise
    /// </summary>
    /// <param name="report">Report</param>
    private void RaiseReportAvailable(ReportContainer report)
    {
        ReportAvailable?.Invoke(this, new ReportAvailableEventArgs(report));
    }

    /// <summary>
    /// Get report details
    /// </summary>
    /// <param name="id">Id</param>
    private async Task<ReportDetailedData> GetReportDetails(string id)
    {
        using var httpClient = new HttpClient();

        var response = await httpClient.GetAsync($"https://dps.report/getJson?id={Uri.EscapeUriString(id)}");
        if (response.IsSuccessStatusCode)
        {
            var responseJson =  await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseJson) == false)
            {
                var responseData = JsonConvert.DeserializeObject<ReportData>(responseJson);

                if (responseData != null)
                {
                    return new ReportDetailedData
                           {
                               FightName = responseData.FightName,
                               FightIcon = responseData.FightIcon,
                           };
                }
            }
        }

        return null;
    }

    #endregion // Private methods
}