using System.IO;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.Common;

namespace QRShot.Services;

public static class ScannerService
{
    public static async Task<Result?> ReadQrCode(Stream imageStream)
    {
        if (imageStream.CanSeek)
            imageStream.Position = 0;
        
        using var image = await Image.LoadAsync<Rgba32>(imageStream);
        
        var reader = new BarcodeReaderGeneric
        {
            AutoRotate = true,
            Options = new DecodingOptions
            {
                PossibleFormats = [BarcodeFormat.QR_CODE],
                TryHarder = true,
                TryInverted = true
            }
        };
        
        return reader.Decode(image);
    }
    
}