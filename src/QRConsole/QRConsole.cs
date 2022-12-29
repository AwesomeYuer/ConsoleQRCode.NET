namespace Microshaoft;

using SixLabors.ImageSharp.PixelFormats;
using System;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

public class QRConsole
{
    public static void Output
                            (
                                string data
                                , string errorCorrectionLevel   = "M"
                                , string characterSet           = "utf-8"
                                , bool disableECI               = false
                                , bool qrCompact                = false
                                , bool gs1Format                = false
                                , bool pureBarcode              = false
                                , int? qrVersion                = null
                                , int width                     = 10
                                , int height                    = 10
                                , int margin                    = 1
                                , ConsoleColor darkColor        = ConsoleColor.Black
                                , ConsoleColor lightColor       = ConsoleColor.White
                                , int thresholdOfDarkLightColor   = 200
                                , string outputChars            = "囍"
                            )
    {
        static ErrorCorrectionLevel ToErrorCorrectionLevel(string errorCorrectionLevel) =>
        errorCorrectionLevel.ToUpper()
        switch
        {
            "L" => ErrorCorrectionLevel.L
            , "M" => ErrorCorrectionLevel.M
            , "Q" => ErrorCorrectionLevel.Q
            , "H" => ErrorCorrectionLevel.H
            , _ => throw new ArgumentOutOfRangeException
                                            (
                                                nameof(errorCorrectionLevel)
                                                , $"Not expected {nameof(ErrorCorrectionLevel)} value: {errorCorrectionLevel}"
                                            )
        };

        var writer = new BarcodeWriter<Rgba32>
        {
            Format = BarcodeFormat.QR_CODE
            , Options = new QrCodeEncodingOptions
                                {
                                    Width               = width
                                    , Height            = height
                                    , ErrorCorrection   = ToErrorCorrectionLevel(errorCorrectionLevel)
                                    , Margin            = 1
                                    , CharacterSet      = characterSet
                                    , DisableECI        = disableECI
                                    , QrCompact         = qrCompact
                                    , GS1Format         = gs1Format
                                    , PureBarcode       = pureBarcode
                                    , QrVersion         = qrVersion
                                }
        };

        using var image = writer.WriteAsImageSharp<Rgba32>(data);
        
        for (var i = 0; i < image.Width; i++)
        {
            for (var j = 0; j < image.Height; j++)
            {
                //获取该像素点的RGB的颜色
                var color = image[i, j];
                if (color.B > thresholdOfDarkLightColor)
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
