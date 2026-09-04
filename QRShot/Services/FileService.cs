using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace QRShot.Services;

public class FileService(Window window)
{
    private readonly Window _target = window;

    public async Task<IStorageFile?> OpenFileAsync()
    {
        var files = await _target.StorageProvider.OpenFilePickerAsync(new()
        {
            Title = "Select an image with a QR code",
            AllowMultiple = false,
            FileTypeFilter = new []
            {
                new FilePickerFileType("Images")
                {
                    Patterns = ["*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif"]
                }
            }
        });
        
        return files.Count >= 1 ? files[0] : null;
    }
}