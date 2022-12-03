﻿using CommunityToolkit.WinUI;
using EnergyStarX.Helpers;
using H.NotifyIcon.Core;
using Microsoft.UI.Dispatching;
using Windows.ApplicationModel;

namespace EnergyStarX.Services;

public class SystemTrayIconService
{
    private readonly System.Drawing.Icon ThrottlingIcon = new(Path.Combine(Package.Current.InstalledPath, "Assets/WindowIcon.ico"));
    private readonly string ThrottlingToolTip = "AppDisplayName".ToLocalized();

    private readonly System.Drawing.Icon NotThrottlingIcon = new(Path.Combine(Package.Current.InstalledPath, "Assets/WindowIcon-Gray.ico"));
    private readonly string NotThrottlingToolTip = $"{"AppDisplayName".ToLocalized()}\n({"Paused".ToLocalized()})";

    private readonly TrayIconWithContextMenu trayIcon = new();

    private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    private readonly WindowService windowsService;
    private readonly EnergyService energyService;

    public SystemTrayIconService(WindowService windowsService, EnergyService energyService)
    {
        this.windowsService = windowsService;
        this.energyService = energyService;
    }

    public async Task Initialize()
    {
        UpdateTrayIconImageAndToolTip();

        trayIcon.ContextMenu = new PopupMenu()
        {
            Items =
            {
                new PopupMenuItem("Open".ToLocalized(), async (s, e) => await dispatcherQueue.EnqueueAsync(() => windowsService.ShowAppWindow())),
                new PopupMenuItem("Exit".ToLocalized(), async (s, e) => await dispatcherQueue.EnqueueAsync(() => windowsService.ExitApp()))
            }
        };

        trayIcon.MessageWindow.MouseEventReceived += async (s, e) =>
        {
            if (e.MouseEvent == MouseEvent.IconDoubleClick)
            {
                await dispatcherQueue.EnqueueAsync(() => windowsService.ShowAppWindow());
            }
        };

        trayIcon.Create();

        windowsService.AppExiting += WindowsService_AppExiting;
        energyService.StatusChanged += EnergyService_StatusChanged;

        // If user is using the taskbar enhancement tool StartAllBack, at this line the tray icon image may be wrong.
        // So I have to wait 0.1 second and update tray icon image and tooltip again.
        await Task.Delay(TimeSpan.FromMilliseconds(100));
        UpdateTrayIconImageAndToolTip();
    }

    private async void EnergyService_StatusChanged(object? sender, EnergyService.EnergyStatus e)
    {
        await dispatcherQueue.EnqueueAsync(() => UpdateTrayIconImageAndToolTip());
    }

    private void WindowsService_AppExiting(object? sender, EventArgs e)
    {
        trayIcon.Remove();
        trayIcon.Dispose();

        ThrottlingIcon.Dispose();
        NotThrottlingIcon.Dispose();
    }

    private void UpdateTrayIconImageAndToolTip()
    {
        (System.Drawing.Icon icon, string toolTip) = GetTrayIconImageAndToolTip(energyService.Status);
        trayIcon.UpdateIcon(icon.Handle);
        trayIcon.UpdateToolTip(toolTip);
    }

    private (System.Drawing.Icon Icon, string toolTip) GetTrayIconImageAndToolTip(EnergyService.EnergyStatus energyStatus) =>
        energyStatus.IsThrottling ?
        (ThrottlingIcon, ThrottlingToolTip) :
        (NotThrottlingIcon, NotThrottlingToolTip);
}