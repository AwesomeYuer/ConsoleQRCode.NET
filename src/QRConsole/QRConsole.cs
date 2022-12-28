namespace Microshaoft;

using System;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;
using ZXing.QrCode;

public class QRConsole
{
    public static void Output
                            (
                                string data
                                , ConsoleColor darkColor = ConsoleColor.Black
                                , ConsoleColor lightColor = ConsoleColor.White
                                , int thresholdDarkLightColor = 200
                                , string outputChars = "囍"
                            )
    {
        var writer = new BarcodeWriter<Rgba32>
        {
            Format = BarcodeFormat.QR_CODE
            , Options = new QrCodeEncodingOptions
                                {
                                    Width = 10
                                    , Height = 10
                                    , Margin = 1
                                   // , QrCompact = true
                                    , CharacterSet = "utf-8"
            }
        };

        using var image = writer.WriteAsImageSharp<Rgba32>(data);
        
        for (var i = 0; i < image.Width; i++)
        {
            for (var j = 0; j < image.Height; j++)
            {
                //获取该像素点的RGB的颜色
                var color = image[i,j];
                if (color.B > thresholdDarkLightColor)
                {
                    Console.BackgroundColor = darkColor;
                    Console.ForegroundColor = darkColor;
                }
                else
                {
                    Console.BackgroundColor = lightColor;
                    Console.ForegroundColor = lightColor;
                }
                Console.Write(outputChars);
                Console.ResetColor();
            }
            Console.Write("\n");
        }
    }
}
