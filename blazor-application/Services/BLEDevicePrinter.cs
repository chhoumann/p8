using BlazorBLE.Data;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Extensions;

namespace BlazorBLE.Services;

public static class BLEDevicePrinter
{
    public static async Task PrintDeviceAdvertisementRecordsAndServices(this IDevice device)
    {
        await PrintAdvertisementRecords(device);

        IReadOnlyList<IService> services = await device.GetServicesAsync();

        foreach (IService service in services)
        {
            await Console.Out.WriteLineAsync($"BLEService: Found service {service.Name} with ID {service.Id}.");

            IReadOnlyList<ICharacteristic> chararcteristics = await service.GetCharacteristicsAsync();

            if (chararcteristics != null)
            {
                await PrintCharacteristics(chararcteristics);
            }
        }
    }

    private static async Task PrintAdvertisementRecords(IDevice device)
    {
        await Console.Out.WriteLineAsync("AdvertisementRecords:");

        foreach (AdvertisementRecord advertisement in device.AdvertisementRecords)
        {
            await Console.Out.WriteLineAsync($"Byte data array length: {advertisement.Data.Length}");
            await Console.Out.WriteLineAsync($"Advertisement ToString(): {advertisement}");
            await Console.Out.WriteLineAsync($"Byte array as a string: {System.Text.Encoding.Default.GetString(advertisement.Data)}");

            if (advertisement.Type.HasFlag(AdvertisementRecordType.ManufacturerSpecificData) && KBeaconData.IsProximityBeacon(advertisement.Data))
            {
                await Console.Out.WriteLineAsync($"BeaconData: {new KBeaconData(advertisement.Data)}");
            }
        }
    }

    private static async Task PrintCharacteristics(IReadOnlyList<ICharacteristic> chararcteristics)
    {
        await Console.Out.WriteLineAsync($"Found characteristics: ");

        foreach (ICharacteristic characteristic in chararcteristics)
        {
            await Console.Out.WriteLineAsync($"- Name: {characteristic.Name}");

            if (characteristic.Value != null)
            {
                await Console.Out.WriteLineAsync($"- Uuid: {characteristic.Uuid}");
                await Console.Out.WriteLineAsync($"- Property type: {characteristic.Properties}");
                await Console.Out.WriteLineAsync($"- Write type: {characteristic.WriteType}");

                if (characteristic.CanRead)
                {
                    byte[] bytes = await characteristic.ReadAsync();
                    await Console.Out.WriteLineAsync($"- ReadAsync bytes: {bytes.ToHexString()}");
                }
            }

            IReadOnlyList<IDescriptor> descriptors = await characteristic.GetDescriptorsAsync();

            if (descriptors is { Count: > 0 })
            {
                await PrintDescriptors(descriptors);
            }
        }

        await Console.Out.WriteLineAsync("");
    }

    private static async Task PrintDescriptors(IReadOnlyList<IDescriptor> descriptors)
    {
        await Console.Out.WriteLineAsync($"Found descriptors:");

        foreach (IDescriptor descriptor in descriptors)
        {
            await Console.Out.WriteLineAsync($"Name: {descriptor.Name}");
            await Console.Out.WriteLineAsync($"Id: {descriptor.Id}");
        }
    }
}
