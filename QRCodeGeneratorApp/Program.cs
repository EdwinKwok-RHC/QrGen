using Microsoft.Extensions.Configuration;
using QRCoder;
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

        string fullPath = Path.Combine(outputPath, outputFileName);
        Directory.CreateDirectory(outputPath);

        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImageBytes = qrCode.GetGraphic(20);
            File.WriteAllBytes(fullPath, qrCodeImageBytes);
            Console.WriteLine($"QR code saved to {fullPath}");
        }
    }
}

