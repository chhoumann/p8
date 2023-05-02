using Plugin.Maui.Audio;

namespace BlazorBLE.Services;

public sealed class SoundEffect
{
    private readonly string fileName;

    public SoundEffect(string fileName) => this.fileName = fileName;

    public void Play()
    {
        Task.Run(async () =>
        {
            Stream sound = await FileSystem.OpenAppPackageFileAsync($"Sounds/{fileName}");
            
            IAudioPlayer player = AudioManager.Current.CreatePlayer(sound);
            player.Play();
        });
    }
}