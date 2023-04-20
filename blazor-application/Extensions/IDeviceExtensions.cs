using BlazorBLE.Data;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;

namespace BlazorBLE.Extensions;

public static class IDeviceExtensions
{   
    public static string GetDeviceDisplayName(this IDevice device)
    {
        return string.IsNullOrEmpty(device.Name) ? "Unknown device" : device.Name;
    }
    
    public static bool IsProximityBeacon(this IDevice device)
    {
        foreach (AdvertisementRecord record in device.AdvertisementRecords)
        {
            if (record.Type == AdvertisementRecordType.ManufacturerSpecificData)
            {
                return IsProximityBeacon(record.Data);
            }
        }

        return false;
    }

    public static KBeaconData GetBeaconData(this IDevice device)
    {
        foreach (AdvertisementRecord record in device.AdvertisementRecords)
        {
            if (record.Type == AdvertisementRecordType.ManufacturerSpecificData)
            {
                return new KBeaconData(record.Data);
            }
        }

        return null;
    }

    private static bool IsProximityBeacon(this byte[] data)
    {
        if (data.Length < 4)
        {
            throw new ArgumentException($"Expected 4 bytes or more, got {data.Length} bytes.");
        }

        bool isCompanyApple = data[0] == 0x4C && data[1] == 0x00;
        bool isProximityBeacon = data[2] == 0x02 && data[3] == 0x15;

        return isCompanyApple && isProximityBeacon;
    }
}