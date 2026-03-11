namespace TatweerPOS.Helpers;

using TatweerPOS.Models;

/// <summary>
/// Manages license validation, storage, and retrieval for TatweerPOS.
/// In DEBUG mode, a dev license key is always accepted.
/// </summary>
public static class LicenseManager
{
    private const string DevLicenseKey = "DEV-0000-0000-0000";
    private const string LicenseFileName = "license.dat";

    /// <summary>
    /// Validates a license key against the device fingerprint.
    /// </summary>
    public static LicenseValidationResult Validate(string licenseKey, string fingerprint)
    {
#if DEBUG
        if (licenseKey == DevLicenseKey)
        {
            return new LicenseValidationResult
            {
                IsValid = true,
                ExpiresAt = DateTime.MaxValue,
                BranchName = "Development Branch",
                ErrorMessage = null
            };
        }
#endif

        // Validate key format: XXXX-XXXX-XXXX-XXXX
        if (string.IsNullOrWhiteSpace(licenseKey))
        {
            return new LicenseValidationResult
            {
                IsValid = false,
                ErrorMessage = "License key is empty."
            };
        }

        var parts = licenseKey.Split('-');
        if (parts.Length != 4 || !parts.All(p => p.Length == 4))
        {
            return new LicenseValidationResult
            {
                IsValid = false,
                ErrorMessage = "Invalid license key format. Expected: XXXX-XXXX-XXXX-XXXX"
            };
        }

        // In release builds, check stored fingerprint matches current device
        if (string.IsNullOrWhiteSpace(fingerprint) || fingerprint == "UNKNOWN")
        {
            return new LicenseValidationResult
            {
                IsValid = false,
                ErrorMessage = "Unable to verify device fingerprint."
            };
        }

        // Mock validation — in production this would verify against a server or embedded signature
        return new LicenseValidationResult
        {
            IsValid = true,
            ExpiresAt = DateTime.UtcNow.AddYears(1),
            BranchName = "Licensed Branch",
            ErrorMessage = null
        };
    }

    /// <summary>
    /// Returns the file path for the license file in %APPDATA%\TatweerPOS\.
    /// </summary>
    public static string GetLicenseFilePath()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return Path.Combine(appData, "TatweerPOS", LicenseFileName);
    }

    /// <summary>
    /// Saves a license key to the license file.
    /// </summary>
    public static void SaveLicense(string key)
    {
        var filePath = GetLicenseFilePath();
        var directory = Path.GetDirectoryName(filePath)!;

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(filePath, key);
    }

    /// <summary>
    /// Loads a previously saved license key from disk.
    /// Returns null if no license file exists or it cannot be read.
    /// </summary>
    public static string? LoadLicense()
    {
        try
        {
            var filePath = GetLicenseFilePath();
            if (!File.Exists(filePath))
                return null;

            var content = File.ReadAllText(filePath).Trim();
            return string.IsNullOrWhiteSpace(content) ? null : content;
        }
        catch
        {
            return null;
        }
    }
}

/// <summary>
/// Result of a license validation check.
/// </summary>
public class LicenseValidationResult
{
    public bool IsValid { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? BranchName { get; set; }
    public string? ErrorMessage { get; set; }
}
