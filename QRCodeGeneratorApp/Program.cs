using Microsoft.Extensions.Configuration;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Load configuration
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string? url = config["QRCodeSettings:TargetUrl"];
        if (string.IsNullOrEmpty(url))
        {
            Console.WriteLine("Error: TargetUrl not found or is empty in appsettings.json");
            return;
        }

        string? outputPath = config["QRCodeSettings:OutputPath"];
        string? outputFileName = config["QRCodeSettings:OutputFileName"];

        if (string.IsNullOrEmpty(outputPath) || string.IsNullOrEmpty(outputFileName))
        {
            Console.WriteLine("Error: OutputPath or OutputFileName not found or is empty in appsettings.json");
            return;
        }

        string? logoPath = config["QRCodeSettings:LogoPath"];
        string? logoFileName = config["QRCodeSettings:LogoFileName"];

        if (string.IsNullOrEmpty(logoPath) || string.IsNullOrEmpty(logoFileName))
        {
            Console.WriteLine("Error: LogoPath or LogoFileName not found or is empty in appsettings.json");
            return;
        }

        string fullLogoPath = Path.Combine(logoPath, logoFileName);

        if (!File.Exists(fullLogoPath))
        {
            Console.WriteLine($"Error: Logo file not found at {fullLogoPath}");
            return;
        }

        string fullPath = Path.Combine(outputPath, outputFileName);
        Directory.CreateDirectory(outputPath);

        // Generate QR code as PNG bytes using QRCoder
        using var qrGenerator = new QRCodeGenerator();
        var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var pngQrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeBytes = pngQrCode.GetGraphic(20);

        // Load QR code and logo images using ImageSharp
        using var logoImage = Image.Load<Rgba32>(fullLogoPath);
        using var qrImage = Image.Load<Rgba32>(qrCodeBytes);
        

        // Resize logo (e.g., 1/5th of QR code width)
        // decrease the denominator to increase the image size
        int logoSize = qrImage.Width / 4;
        logoImage.Mutate(x => x.Resize(logoSize, logoSize));

        // Calculate centered position
        int left = (qrImage.Width - logoSize) / 2;
        int top = (qrImage.Height - logoSize) / 2;

        // Overlay the logo onto the QR code
        qrImage.Mutate(x => x.DrawImage(logoImage, new Point(left, top), 1f));

        // Save the result as PNG, forcing full color
        var encoder = new SixLabors.ImageSharp.Formats.Png.PngEncoder
        {
            ColorType = SixLabors.ImageSharp.Formats.Png.PngColorType.Rgb
        };
        qrImage.Save(fullPath, encoder);
        //logoImage.Save("output/logo.png");

        Console.WriteLine($"QR code with logo saved to {fullPath}");
    }
}