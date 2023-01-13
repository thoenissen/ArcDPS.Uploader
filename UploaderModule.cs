using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;

using ArcDPS.Uploader.Controls;
using ArcDPS.Uploader.Services;

using Blish_HUD;
using Blish_HUD.Controls;
using Blish_HUD.Modules;
using Blish_HUD.Settings;

using Microsoft.Xna.Framework.Graphics;

namespace ArcDPS.Uploader;

/// <summary>
/// Module definition
/// </summary>
[Export(typeof(Module))]
public class UploaderModule : Module
{
    #region Fields

    /// <summary>
    /// Current instance
    /// </summary>
    private static UploaderModule _currentInstance;

    /// <summary>
    /// Tab icon
    /// </summary>
    private Texture2D _tabIcon;

    /// <summary>
    /// Report entries tab
    /// </summary>
    private WindowTab _tab;

    /// <summary>
    /// Reports directory settings
    /// </summary>
    private SettingEntry<string> _reportsDirectorySettings;

    /// <summary>
    /// User token
    /// </summary>
    private SettingEntry<string> _userTokenSettings;

    /// <summary>
    /// Reports manager
    /// </summary>
    private ReportManager _reportManager;

    #endregion // Fields

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="moduleParameters">Module parameters</param>
    [ImportingConstructor]
    public UploaderModule([Import("ModuleParameters")] ModuleParameters moduleParameters)
        : base(moduleParameters)
    {
        if (_currentInstance != null)
        {
            throw new InvalidOperationException("Module already created.");
        }

        _currentInstance = this;
    }

    #endregion // Constructor

    #region Methods

    /// <summary>
    /// The reports directory has been changed
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    private void OnReportsDirectoryChanged(object sender, ValueChangedEventArgs<string> e)
    {
        _reportManager.SetDirectory(e.NewValue);
    }

    /// <summary>
    /// The user token has been changed
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    private void OnUserTokenChanged(object sender, ValueChangedEventArgs<string> e)
    {
        _reportManager.SetUserToken(e.NewValue);
    }

    #endregion // Methods

    #region Module

    /// <inheritdoc />
    protected override void DefineSettings(SettingCollection settings)
    {
        _reportsDirectorySettings = settings.DefineSetting("AD1724B0-BFBA-4C91-A962-29291A33FFAA",
                                                           Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Guild Wars 2", "addons", "arcdps", "arcdps.cbtlogs"),
                                                           () => "Reports directory",
                                                           () => "Combat reports directory which is used by ArcDPS");

        _reportsDirectorySettings.SettingChanged += OnReportsDirectoryChanged;

        _userTokenSettings = settings.DefineSetting("89798390-AFAD-46DE-9BC7-9DB7C5A95E72",
                                                    string.Empty,
                                                    () => "User token");

        _userTokenSettings.SettingChanged += OnUserTokenChanged;
    }

    /// <inheritdoc />
    protected override async Task LoadAsync()
    {
        await Task.Run(() => _tabIcon = ModuleParameters.ContentsManager.GetTexture("upload.png"));
    }

    /// <inheritdoc />
    protected override void OnModuleLoaded(EventArgs e)
    {
        _reportManager = new ReportManager();

        if (_reportsDirectorySettings.IsNull == false)
        {
            _reportManager.SetDirectory(_reportsDirectorySettings.Value);
        }

        if (_userTokenSettings.IsNull == false)
        {
            _reportManager.SetUserToken(_userTokenSettings.Value);
        }

        _tab = GameService.Overlay.BlishHudWindow.AddTab("ArcDPS.Uploader.Tab",
                                                         _tabIcon,
                                                         () => new ReportEntriesView(_reportManager));

        base.OnModuleLoaded(e);
    }

    /// <inheritdoc />
    protected override void Unload()
    {
        _reportsDirectorySettings.SettingChanged -= OnReportsDirectoryChanged;
        _reportsDirectorySettings = null;

        GameService.Overlay.BlishHudWindow.RemoveTab(_tab);

        _currentInstance = null;
    }

    #endregion // Module
}