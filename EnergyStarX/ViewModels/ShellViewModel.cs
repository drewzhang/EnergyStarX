﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EnergyStarX.Contracts.Services;
using EnergyStarX.Helpers;
using EnergyStarX.Services;
using EnergyStarX.Views;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace EnergyStarX.ViewModels;

public partial class ShellViewModel : ObservableRecipient
{
    private readonly DispatcherQueue dispatcherQueue = DispatcherQueue.GetForCurrentThread();

    private readonly WindowService windowService;
    private readonly EnergyService energyService;

    public INavigationService NavigationService { get; }
    public INavigationViewService NavigationViewService { get; }

    [ObservableProperty]
    private bool isBackEnabled;

    [ObservableProperty]
    private object? selected;

    [ObservableProperty]
    private string taskbarIconSource = "";

    [ObservableProperty]
    private string taskbarIconToolTip = "";

    public bool ShowTeachingTip
    {
        get => LocalSettings.FirstRun;
        set => SetProperty(ShowTeachingTip, value, x => LocalSettings.FirstRun = x);
    }

    public bool IsOsVersionNotRecommended { get; } = Environment.OSVersion.Version.Build < 22621;

    public string OsVersionNotRecommendedWarningMessage { get; } = string.Format("OsVersionNotRecommendedWarningMessage".GetLocalized(), Environment.OSVersion.Version.Build);

    public ShellViewModel(INavigationService navigationService, INavigationViewService navigationViewService, WindowService windowService, EnergyService energyService)
    {
        NavigationService = navigationService;
        NavigationService.Navigated += OnNavigated;
        NavigationViewService = navigationViewService;
        this.windowService = windowService;
        this.energyService = energyService;

        UpdateTaskbarIcon(this.energyService.Status);
        this.energyService.StatusChanged += EnergyService_StatusChanged;
    }

    [RelayCommand]
    private void ShowAppWindow()
    {
        windowService.ShowAppWindow();
    }

    [RelayCommand]
    private void ExitApp()
    {
        windowService.ExitApp();
    }

    [RelayCommand]
    private void CloseTeachingTip()
    {
        ShowTeachingTip = false;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;

        if (e.SourcePageType == typeof(SettingsPage))
        {
            Selected = NavigationViewService.SettingsItem;
            ShowTeachingTip = false;
            return;
        }

        NavigationViewItem? selectedItem = NavigationViewService.GetSelectedItem(e.SourcePageType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }

    private void UpdateTaskbarIcon(EnergyService.EnergyStatus energyStatus)
    {
        (TaskbarIconSource, TaskbarIconToolTip) = energyStatus.IsThrottling ?
            ("ms-appx:///Assets/WindowIcon.ico", "AppDisplayName".GetLocalized()) :
            ("ms-appx:///Assets/WindowIcon-Gray.ico", $"{"AppDisplayName".GetLocalized()}\n({"Paused".GetLocalized()})");
    }

    private async void EnergyService_StatusChanged(object? sender, EnergyService.EnergyStatus e)
    {
        await CommunityToolkit.WinUI.DispatcherQueueExtensions.EnqueueAsync(dispatcherQueue, () =>
        {
            UpdateTaskbarIcon(e);
        });
    }
}
