﻿// This file is generated by a T4 template. Do not edit manually.

// Note:
// Some types can be directly stored in "ApplicationData.Current.LocalSettings": 
// https://docs.microsoft.com/windows/apps/design/app-settings/store-and-retrieve-app-data#settings
// Unfortunately there's no way to constraint a generic type "T" to "T1 or T2" in C#, so I have to use a T4 template to generate these methods.

using Windows.Foundation.Collections;
using Windows.Storage;

namespace EnergyStarX.Helpers;

public static class SettingsHelper
{
    private static IPropertySet LocalSettings => ApplicationData.Current.LocalSettings.Values;

    private static T GetSettingPrivate<T>(string settingKey, T defaultValue)
    {
        if (LocalSettings.TryGetValue(settingKey, out object? settingObject))
        {
            if (settingObject is T settingValue)
            {
                return settingValue;
            }
        }
        return defaultValue;
    }

    // int
    public static int GetSetting(string settingKey, int defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, int value) => LocalSettings[settingKey] = value;

    // long
    public static long GetSetting(string settingKey, long defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, long value) => LocalSettings[settingKey] = value;

    // float
    public static float GetSetting(string settingKey, float defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, float value) => LocalSettings[settingKey] = value;

    // double
    public static double GetSetting(string settingKey, double defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, double value) => LocalSettings[settingKey] = value;

    // bool
    public static bool GetSetting(string settingKey, bool defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, bool value) => LocalSettings[settingKey] = value;

    // char
    public static char GetSetting(string settingKey, char defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, char value) => LocalSettings[settingKey] = value;

    // string
    public static string GetSetting(string settingKey, string defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, string value) => LocalSettings[settingKey] = value;

    // DateTime
    public static DateTime GetSetting(string settingKey, DateTime defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, DateTime value) => LocalSettings[settingKey] = value;

    // TimeSpan
    public static TimeSpan GetSetting(string settingKey, TimeSpan defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, TimeSpan value) => LocalSettings[settingKey] = value;

    // Guid
    public static Guid GetSetting(string settingKey, Guid defaultValue) => GetSettingPrivate(settingKey, defaultValue);
    public static void SetSetting(string settingKey, Guid value) => LocalSettings[settingKey] = value;

}