using BlazorBLE.Extensions;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;

namespace BlazorBLE.Services;

public sealed class BLEAdvertisementScanner
{
    public event Action<IDevice> BeaconAdvertised;
    
    private readonly IAdapter adapter;

    public BLEAdvertisementScanner()
    {
        adapter = CrossBluetoothLE.Current.Adapter;
        adapter.DeviceAdvertised += Adapter_DeviceAdvertised;
    }

    ~BLEAdvertisementScanner()
    {
        adapter.DeviceAdvertised -= Adapter_DeviceAdvertised;
    }
    
    private void Adapter_DeviceAdvertised(object sender, DeviceEventArgs e)
    {
        if (e.Device.IsProximityBeacon())
        {
            BeaconAdvertised?.Invoke(e.Device);
        }
    }
}