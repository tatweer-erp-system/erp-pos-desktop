using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using MaterialDesignThemes.Wpf;
using TatweerPOS.Models.Enums;

namespace TatweerPOS.Resources.Themes;

/// <summary>
/// Manages runtime theme switching between Dark and Light modes.
/// Persists the user's choice to a local JSON settings file.
/// </summary>
public static class ThemeManager
{
    private const string DarkThemeUri  = "Resources/Themes/DarkTheme.xaml";
    private const string LightThemeUri = "Resources/Themes/LightTheme.xaml";

    private static readonly string SettingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "TatweerPOS",
        "theme-settings.json");

    /// <summary>
    /// The currently active theme.
    /// </summary>
    public static AppTheme CurrentTheme { get; private set; } = AppTheme.Dark;

    /// <summary>
    /// Raised after a theme change has been applied.
    /// </summary>
    public static event Action<AppTheme>? ThemeChanged;

    /// <summary>
    /// Loads the persisted theme preference and applies it.
    /// Call once during application startup.
    /// </summary>
    public static void Initialize()
    {
        var saved = LoadPersistedTheme();
        Apply(saved);
    }

    /// <summary>
    /// Applies the specified theme to the running application.
    /// </summary>
    public static void Apply(AppTheme theme)
    {
        if (Application.Current is null) return;

        var mergedDicts = Application.Current.Resources.MergedDictionaries;

        // Determine URIs
        var oldUri = theme == AppTheme.Dark ? LightThemeUri : DarkThemeUri;
        var newUri = theme == AppTheme.Dark ? DarkThemeUri  : LightThemeUri;

        // Remove existing theme dictionary (if any)
        var existing = mergedDicts
            .FirstOrDefault(d => d.Source != null &&
                (d.Source.OriginalString.Contains("DarkTheme.xaml") ||
                 d.Source.OriginalString.Contains("LightTheme.xaml")));

        if (existing != null)
        {
            mergedDicts.Remove(existing);
        }

        // Add the new theme dictionary
        var newDict = new ResourceDictionary
        {
            Source = new Uri(newUri, UriKind.Relative)
        };
        mergedDicts.Add(newDict);

        // Update MaterialDesignInXaml BundledTheme base theme
        ApplyMaterialDesignBaseTheme(theme);

        // Track state
        CurrentTheme = theme;

        // Persist
        PersistTheme(theme);

        // Notify subscribers
        ThemeChanged?.Invoke(theme);
    }

    /// <summary>
    /// Toggles between Dark and Light themes.
    /// </summary>
    public static void Toggle()
    {
        var next = CurrentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
        Apply(next);
    }

    // ─── Private helpers ───────────────────────────────────────────

    private static void ApplyMaterialDesignBaseTheme(AppTheme theme)
    {
        try
        {
            var paletteHelper = new PaletteHelper();
            var mdTheme = paletteHelper.GetTheme();

            mdTheme.SetBaseTheme(
                theme == AppTheme.Dark
                    ? BaseTheme.Dark
                    : BaseTheme.Light);

            paletteHelper.SetTheme(mdTheme);
        }
        catch
        {
            // MaterialDesign may not be initialized yet during early startup.
        }
    }

    private static void PersistTheme(AppTheme theme)
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsFilePath)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var json = JsonSerializer.Serialize(new ThemeSettings { Theme = theme.ToString() });
            File.WriteAllText(SettingsFilePath, json);
        }
        catch
        {
            // Non-critical — swallow silently.
        }
    }

    private static AppTheme LoadPersistedTheme()
    {
        try
        {
            if (!File.Exists(SettingsFilePath))
                return AppTheme.Dark;

            var json = File.ReadAllText(SettingsFilePath);
            var settings = JsonSerializer.Deserialize<ThemeSettings>(json);

            if (settings != null && Enum.TryParse<AppTheme>(settings.Theme, out var parsed))
                return parsed;
        }
        catch
        {
            // Corrupted file — fall back to default.
        }

        return AppTheme.Dark;
    }

    private sealed class ThemeSettings
    {
        public string Theme { get; set; } = nameof(AppTheme.Dark);
    }
}
