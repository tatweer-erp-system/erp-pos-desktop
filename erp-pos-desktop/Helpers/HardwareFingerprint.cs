using System.Security.Cryptography;
using System.Text;

namespace TatweerPOS.Helpers;

/// <summary>
/// Generates a unique hardware fingerprint for device identification and license binding.
/// NOTE: In production/release builds, requires the System.Management NuGet package
/// for WMI queries (CPU ID, Motherboard Serial, MAC Address).
/// Add to .csproj: <PackageReference Include="System.Management" Version="8.*" />
/// </summary>
public static class HardwareFingerprint
{
    public static string Generate()
    {
#if DEBUG
        return "DEV0-DEV0-DEV0-DEV0";
#else
        try
        {
            string cpuId = GetWmiValue("Win32_Processor", "ProcessorId");
            string motherboardSerial = GetWmiValue("Win32_BaseBoard", "SerialNumber");
            string macAddress = GetPrimaryMacAddress();

            string combined = $"{cpuId}|{motherboardSerial}|{macAddress}";

            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(combined));
            string hex = BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();

            // Format as XXXX-XXXX-XXXX-XXXX (first 16 hex chars)
            return $"{hex[..4]}-{hex[4..8]}-{hex[8..12]}-{hex[12..16]}";
        }
        catch
        {
            return "UNKNOWN";
        }
#endif
    }

#if !DEBUG
    private static string GetWmiValue(string wmiClass, string propertyName)
    {
        try
        {
            using var searcher = new System.Management.ManagementObjectSearcher($"SELECT {propertyName} FROM {wmiClass}");
            foreach (var obj in searcher.Get())
            {
                var value = obj[propertyName]?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    return value.Trim();
            }
        }
        catch
        {
            // Silently fail — caller handles "UNKNOWN" fallback
        }
        return "UNKNOWN";
    }

    private static string GetPrimaryMacAddress()
    {
        try
        {
            using var searcher = new System.Management.ManagementObjectSearcher(
                "SELECT MACAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = TRUE");
            foreach (var obj in searcher.Get())
            {
                var mac = obj["MACAddress"]?.ToString();
                if (!string.IsNullOrWhiteSpace(mac))
                    return mac.Trim();
            }
        }
        catch
        {
            // Silently fail — caller handles "UNKNOWN" fallback
        }
        return "UNKNOWN";
    }
#endif
}
