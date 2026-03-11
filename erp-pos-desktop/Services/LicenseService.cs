namespace TatweerPOS.Services;

using TatweerPOS.Helpers;
using TatweerPOS.Models;

/// <summary>
/// Singleton license service that wraps static LicenseManager and HardwareFingerprint
/// for DI-friendly license validation and activation.
/// Per CLAUDE.md, the app validates the license on every startup.
/// Dev license key DEV-0000-0000-0000 bypasses hardware check in DEBUG builds.
/// </summary>
public class LicenseService
{
    private readonly AuditService _auditService;

    public LicenseInfo? CurrentLicense { get; private set; }

    public bool IsLicensed => CurrentLicense?.IsValid == true;

    public LicenseService(AuditService auditService)
    {
        _auditService = auditService;
    }

    /// <summary>
    /// Returns the hardware fingerprint for this device.
    /// Format: XXXX-XXXX-XXXX-XXXX (e.g., DEV0-DEV0-DEV0-DEV0 in DEBUG).
    /// </summary>
    public string GetFingerprint()
    {
        return HardwareFingerprint.Generate();
    }

    /// <summary>
    /// Called on application startup. Loads the saved license key from disk
    /// and validates it against the current hardware fingerprint.
    /// </summary>
    public (bool Success, string? Error) ValidateOnStartup()
    {
        var savedKey = LicenseManager.LoadLicense();
        if (string.IsNullOrWhiteSpace(savedKey))
        {
            _auditService.Log(
                Models.Enums.AuditAction.LicenseFailure,
                "system",
                "LicenseService",
                "",
                "No license file found on startup.");

            CurrentLicense = null;
            return (false, "No license found. Please activate your license.");
        }

        return ValidateKey(savedKey);
    }

    /// <summary>
    /// Activates TatweerPOS with the given license key.
    /// Validates the key, saves it to disk on success, and updates CurrentLicense.
    /// </summary>
    public (bool Success, string? Error) Activate(string licenseKey)
    {
        if (string.IsNullOrWhiteSpace(licenseKey))
        {
            return (false, "License key cannot be empty.");
        }

        var result = ValidateKey(licenseKey);

        if (result.Success)
        {
            LicenseManager.SaveLicense(licenseKey);

            _auditService.Log(
                Models.Enums.AuditAction.LicenseValidation,
                "system",
                "LicenseService",
                "",
                $"License activated successfully: {licenseKey}");
        }

        return result;
    }

    private (bool Success, string? Error) ValidateKey(string licenseKey)
    {
        var fingerprint = GetFingerprint();
        var validationResult = LicenseManager.Validate(licenseKey, fingerprint);

        if (validationResult.IsValid)
        {
            CurrentLicense = new LicenseInfo
            {
                LicenseKey = licenseKey,
                MachineFingerprint = fingerprint,
                IsValid = true,
                ExpiresAt = validationResult.ExpiresAt,
                BranchName = validationResult.BranchName ?? "",
                ActivatedAt = DateTime.UtcNow
            };

            return (true, null);
        }
        else
        {
            CurrentLicense = null;

            _auditService.Log(
                Models.Enums.AuditAction.LicenseFailure,
                "system",
                "LicenseService",
                "",
                $"License validation failed: {validationResult.ErrorMessage}");

            return (false, validationResult.ErrorMessage);
        }
    }
}
