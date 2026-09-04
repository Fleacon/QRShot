using System.IO;
using Avalonia.Media;

namespace QRShot.Models;

public record QrImage(IImage? Image, string Content, bool IsLink);