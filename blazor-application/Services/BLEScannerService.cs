using BlazorBLE.Extensions;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace BlazorBLE.Services;

public sealed class BLEScannerService
{
    public event Action DevicesChanged;

    public IReadOnlyList<IDevice> Devices => devices;

    private readonly List<IDevice> devices = new();
    private readonly IAdapter adapter;
    private readonly IPromptService promptService;

    public BLEScannerService(IPromptService promptService)
    {
        this.promptService = promptService;

        adapter = CrossBluetoothLE.Current.Adapter;
        adapter.ScanTimeout = Timeout.Infinite;
        adapter.ScanMode = ScanMode.LowLatency;
        adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
    }

    ~BLEScannerService()
    {
        adapter.DeviceDiscovered -= Adapter_DeviceDiscovered;
        adapter.StopScanningForDevicesAsync();
    }

    private void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
    {
        IDevice device = e.Device;

        if (!device.IsProximityBeacon()) return;
        if (devices.Contains(device)) return;

        devices.Add(device);
        // devices.Sort((deviceA, deviceB) => deviceB.Rssi - deviceA.Rssi);
        devices.Sort((deviceA, deviceB) => deviceB.Name.CompareTo(deviceA.Name));

        DevicesChanged?.Invoke();
    }

    public async void BeginScan()
    {
        if (!await AppPermissions.RequestPermissions()) 
        {
            promptService.ShowAlert("Error", "Permissions not granted - cannot run scan.");
            return; 
        }

        await adapter.StartScanningForDevicesAsync();
    }
}

