namespace BlazorBLE.Services;

public static class AppPermissions
{
    public static async Task<bool> RequestPermissions()
    {
        if (!await RequestLocationPermission()) return false;
        if (!await RequestBluetoothPermissions()) return false;

        return true;
    }

    private static async Task<bool> RequestLocationPermission()
    {
        PermissionStatus permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        await Console.Out.WriteLineAsync($"Location permission status: {permissionStatus}.");

        if (permissionStatus != PermissionStatus.Granted)
        {
            permissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            await Console.Out.WriteLineAsync($"Location permission status after requesting: {permissionStatus}.");

            if (permissionStatus != PermissionStatus.Granted)
            {
                await Console.Out.WriteLineAsync("Location permission not granted.");

                return false;
            }
        }

        return true;
    }

    private static async Task<bool> RequestBluetoothPermissions()
    {
        bool hasBluetoothPermissions = await CheckBluetoothStatus();

        await Console.Out.WriteLineAsync($"Has Bluetooth permissions: {hasBluetoothPermissions}.");

        if (!hasBluetoothPermissions)
        {
            hasBluetoothPermissions = await RequestBluetoothAccess();

            if (!hasBluetoothPermissions)
            {
                await Console.Out.WriteLineAsync("Bluetooth permissions not granted.");

                return false;
            }
        }

        return true;
    }

    private static async Task<bool> CheckBluetoothStatus()
    {
        try
        {
            PermissionStatus requestStatus = await new BluetoothPermissions().CheckStatusAsync();

            return requestStatus == PermissionStatus.Granted;
        }
        catch
        {
            return false;
        }
    }

    private static async Task<bool> RequestBluetoothAccess()
    {
        try
        {
            PermissionStatus requestStatus = await new BluetoothPermissions().RequestAsync();

            return requestStatus == PermissionStatus.Granted;
        }
        catch
        {
            return false;
        }
    }
}
