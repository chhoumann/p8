using BlazorBLE.Data;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;

namespace BlazorBLE.Services;

public sealed class BLEScannerService
{
    public event Action DevicesChanged;

    public IReadOnlyList<IDevice> Devices => devices;

    private readonly List<IDevice> devices = new();
    private readonly IAdapter adapter;

    public BLEScannerService()
    {
        adapter = CrossBluetoothLE.Current.Adapter;
        adapter.ScanMode = ScanMode.LowLatency;
        adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
    }

    ~BLEScannerService()
    {
        adapter.DeviceDiscovered -= Adapter_DeviceDiscovered;
        adapter.StopScanningForDevicesAsync();
    }

    private void Adapter_DeviceDiscovered(object sender, DeviceEventArgs args)
    {
        IDevice device = args.Device;

        if (!KBeaconData.IsProximityBeacon(device)) return;

        Console.WriteLine($"Discovered proximity beacon: {device.Id} {device.Name}");

        devices.Add(device);
        devices.Sort((deviceA, deviceB) => deviceB.Rssi - deviceA.Rssi);

        DevicesChanged?.Invoke();
    }

    public async void BeginScan()
    {
        if (!await AppPermissions.RequestPermissions()) return;

        await adapter.StartScanningForDevicesAsync();
    }

    public IReadOnlyList<IDevice> GetKnownDevices() => adapter.GetSystemConnectedOrPairedDevices();

    public void ConnectToDevice(IDevice device, Action<bool, string> onComplete)
    {
        Task.Run(async () =>
        {
            try
            {
                if (adapter.IsScanning)
                {
                    await adapter.StopScanningForDevicesAsync();
                }

                await Console.Out.WriteLineAsync("BLEService: Connecting to device...");
                await adapter.ConnectToDeviceAsync(device);
                await Console.Out.WriteLineAsync("BLEService: Connected to device.");

                onComplete(true, null);
            }
            catch (DeviceConnectionException ex)
            {
                // specific
                await Console.Out.WriteLineAsync($"BLEService DeviceConnectionException: {ex.Message}");
                onComplete(false, ex.Message);
            }
            catch (Exception ex)
            {
                // generic
                await Console.Out.WriteLineAsync($"BLEService Generic exception: {ex.Message}");
                onComplete(false, ex.Message);
            }
        });
    }
}

