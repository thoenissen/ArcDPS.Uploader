using System;
using System.Threading;
using System.Threading.Tasks;

using ArcDPS.Uploader.Data;
using ArcDPS.Uploader.Services;

using Blish_HUD;
using Blish_HUD.Content;
using Blish_HUD.Controls;
using Blish_HUD.Graphics.UI;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArcDPS.Uploader.Controls;

/// <summary>
/// Report entries view
/// </summary>
public class ReportEntriesView : View
{
    #region Fields

    /// <summary>
    /// Report manager
    /// </summary>
    private readonly ReportManager _manager;

    /// <summary>
    /// Flow panel
    /// </summary>
    private FlowPanel _flowPanel;

    #endregion // Fields

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="manager">Report manager</param>
    public ReportEntriesView(ReportManager manager)
    {
        _manager = manager;
        _manager.ReportAvailable += OnReportAvailable;
    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Should the next item be tinted?
    /// </summary>
    public bool IsShowTint => _flowPanel?.Children?.Count % 2 == 0;

    #endregion // Properties

    #region Methods

    /// <summary>
    /// A new report is available
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Arguments</param>
    private void OnReportAvailable(object sender, Events.ReportAvailableEventArgs e)
    {
        AddEntry(e.Report);
    }

    /// <summary>
    /// Add new entry
    /// </summary>
    /// <param name="report">Report data</param>
    public void AddEntry(ReportContainer report)
    {
        lock (_flowPanel)
        {
            var viewContainer = new ViewContainer
                                {
                                    Parent = _flowPanel,
                                    Width = _flowPanel.Size.X,
                                    HeightSizingMode = SizingMode.AutoSize,
                                    ShowTint = IsShowTint,
                                };

            var bossLabel = new Label
                            {
                                Text = report.Details?.FightName ?? report.Meta.BossName,
                                AutoSizeWidth = true,
                                AutoSizeHeight = true,
                                Font = GameService.Content.GetFont(ContentService.FontFace.Menomonia, ContentService.FontSize.Size14, ContentService.FontStyle.Bold),
                                Location = new Point(82, 8),
                                Parent = viewContainer
                            };

            if (report.Meta.IsCm == true)
            {
                bossLabel.Text += " [CM]";
            }

            new Label
            {
                Text = "Time:",
                AutoSizeWidth = true,
                AutoSizeHeight = true,
                Font = GameService.Content.DefaultFont14,
                Location = new Point(82, 24),
                Parent = viewContainer,
            };

            new Label
            {
                Text = report.Meta.TimeStamp.ToString("g"),
                AutoSizeWidth = true,
                AutoSizeHeight = true,
                Font = GameService.Content.DefaultFont14,
                Location = new Point(144, 24),
                Parent = viewContainer
            };

            new Label
            {
                Text = "Duration:",
                AutoSizeWidth = true,
                AutoSizeHeight = true,
                Font = GameService.Content.DefaultFont14,
                Location = new Point(82, 40),
                Parent = viewContainer,
            };

            new Label
            {
                Text = TimeSpan.FromSeconds(report.Meta.Duration ?? 0)
                               .ToString("mm\\:ss"),
                AutoSizeWidth = false,
                Width = 80,
                Font = GameService.Content.DefaultFont14,
                Location = new Point(144, 40),
                Parent = viewContainer,
            };

            var fightIconTexture = new AsyncTexture2D();

            new Image(fightIconTexture)
            {
                Parent = viewContainer,
                Size = new Point(64, 64),
                Location = new Point(0, 0),
                Height = 64,
                Width = 64
            }; 

            Task.Run(() =>
                     {
                         try
                         {
                             using var ctx = GameService.Graphics.LendGraphicsDeviceContext();

                             using var httpClient = new System.Net.Http.HttpClient();

                             httpClient.DefaultRequestHeaders.Add("User-Agent", "ArcDPS.Uploader");

                             var texture = Texture2D.FromStream(ctx.GraphicsDevice, httpClient.GetStreamAsync(report.Details.FightIcon).Result);

                             lock (_flowPanel)
                             {
                                 fightIconTexture.SwapTexture(texture);
                             }
                         }
                         catch (Exception ex)
                         {
                             Logger.GetLogger<ReportEntriesView>()
                                   .Warn(ex, "Download fight icon failed.");
                         }
                     });
        }
    }

    #endregion // Methods

    #region View

    /// <inheritdoc />
    protected override void Build(Container buildPanel)
    {
        _flowPanel = new FlowPanel
                     {
                         Parent = buildPanel,
                         FlowDirection = ControlFlowDirection.SingleTopToBottom,
                         ControlPadding = new Vector2(8, 8),
                         Location = Panel.MenuStandard.PanelOffset,
                         Size = buildPanel.Size,
                         CanScroll = true,
                     };

        foreach (var upload in _manager.GetUploads())
        {
            AddEntry(upload);
        }

        base.Build(buildPanel);
    }

    #endregion // View
}