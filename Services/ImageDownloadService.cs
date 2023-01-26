using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

using ArcDPS.Uploader.Controls;

using Blish_HUD;

namespace ArcDPS.Uploader.Services;

/// <summary>
/// 
/// </summary>
internal class ImageDownloadService
{
    #region Fields

    /// <summary>
    /// Images
    /// </summary>
    public readonly Dictionary<string, byte[]> _images = new();

    /// <summary>
    ///  Http-Client
    /// </summary>
    public readonly HttpClient _httpClient = new();

    /// <summary>
    /// Locking
    /// </summary>
    public readonly SemaphoreSlim _semaphore = new (1, 1);

    #endregion // Fields

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public ImageDownloadService()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "ArcDPS.Uploader");
    }

    #endregion // Constructor

    #region Methods

    /// <summary>
    /// Get image
    /// </summary>
    /// <param name="url">Url</param>
    /// <returns>Image data</returns>
    public async Task<byte[]> GetImage(string url)
    {
        byte[] data = null;

        await _semaphore.WaitAsync();

        try
        {
            if (_images.TryGetValue(url, out data) == false)
            {
                _images[url] = data = await _httpClient.GetByteArrayAsync(url);
            }
        }
        catch (Exception ex)
        {
            Logger.GetLogger<ReportEntriesView>()
                  .Warn(ex, "Download image failed.");
        }
        finally
        {
            _semaphore.Release();
        }

        return data;
    }

    #endregion // Methods
}