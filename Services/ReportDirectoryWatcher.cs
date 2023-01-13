using System;
using System.IO;
using System.Threading.Tasks;

using ArcDPS.Uploader.Events;

namespace ArcDPS.Uploader.Services;

/// <summary>
/// Watching if any new reports are created
/// </summary>
public class ReportDirectoryWatcher : IDisposable
{
    #region Fields

    /// <summary>
    /// File system watcher
    /// </summary>
    private FileSystemWatcher _watcher;

    #endregion // Fields

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="path">Reports path</param>
    public ReportDirectoryWatcher(string path)
    {
        _watcher = new FileSystemWatcher(path, "*.zevtc")
                   {
                       IncludeSubdirectories = true,
                       EnableRaisingEvents = true,
                       NotifyFilter = NotifyFilters.FileName
                   };

        _watcher.Renamed += OnFileRenamed;
    }

    #endregion // Constructor

    #region Events

    /// <summary>
    /// Report created
    /// </summary>
    public event ReportCreatedEventHandler ReportCreated;

    #endregion // Events

    #region Methods

    /// <summary>
    /// A file hast been renamed
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    private void OnFileRenamed(object sender, RenamedEventArgs e)
    {
        Task.Delay(2_000)
            .ContinueWith(t => ReportCreated?.Invoke(this, new ReportCreatedEventArgs(e.FullPath)));
    }

    #endregion // Methods

    #region IDisposable

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (_watcher != null)
        {
            _watcher.Renamed -= OnFileRenamed;
            _watcher.Dispose();
        }

        _watcher = null;
    }

    #endregion // IDisposable
}