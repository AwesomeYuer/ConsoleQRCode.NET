namespace Microshaoft;

using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

public static class QRCodeConsole
{
    private static readonly object _locker = new ();

    public static void WriteQRCodeLine
                            (
                                this TextWriter @this
                                , string data

                                , string errorCorrectionLevel       = "M"
                                , string characterSet               = "utf-8"

                                , bool qrCompact                    = false
                                , bool pureBarcode                  = false
                                , int? qrVersion                    = null
                                , bool disableECI                   = false
                                , bool gs1Format                    = false

                                , int width                         = 10
                                , int height                        = 10
                                , int margin                        = 1

                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White
                                , int thresholdOfDarkLightColor     = 200

                                , char placeholderChar              = '囍'

                                , int? outputPostionLeft            = null!
                                , int? outputPostionTop             = null!
                            )
    {
        WriteQRCode
            (
                @this
                , data

                , errorCorrectionLevel
                , characterSet

                , qrCompact
                , pureBarcode
                , qrVersion
                , disableECI
                , gs1Format

                , width
                , height
                , margin

                , darkColor
                , lightColor
                , thresholdOfDarkLightColor

                , placeholderChar

                , outputPostionLeft
                , outputPostionTop
            );
        Console.WriteLine();
    }

    public static void WriteQRCode
                            (
                                this TextWriter @this
                                , string data

                                , string errorCorrectionLevel       = "M"
                                , string characterSet               = "utf-8"

                                , bool qrCompact                    = false
                                , bool pureBarcode                  = false
                                , int? qrVersion                    = null
                                , bool disableECI                   = false
                                , bool gs1Format                    = false

                                , int width                         = 10
                                , int height                        = 10
                                , int margin                        = 1

                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White
                                , int thresholdOfDarkLightColor     = 200

                                , char placeholderChar              = '囍'

                                , int? outputPostionLeft            = null!
                                , int? outputPostionTop             = null!
                            )
    {
        _ = @this;
        Output
            (
                data

                , errorCorrectionLevel
                , characterSet

                , qrCompact
                , pureBarcode
                , qrVersion
                , disableECI
                , gs1Format

                , width
                , height
                , margin

                , darkColor
                , lightColor
                , thresholdOfDarkLightColor

                , placeholderChar

                , outputPostionLeft
                , outputPostionTop
            );
    }

    public static void Output
                            (
                                string data
                        
                                , string errorCorrectionLevel       = "M"
                                , string characterSet               = "utf-8"

                                , bool qrCompact                    = false
                                , bool pureBarcode                  = false
                                , int? qrVersion                    = null
                                , bool disableECI                   = false
                                , bool gs1Format                    = false

                                , int width                         = 10
                                , int height                        = 10
                                , int margin                        = 1

                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White
                                , int thresholdOfDarkLightColor     = 200

                                , char placeholderChar              = '囍'

                                , int? outputPostionLeft            = null!
                                , int? outputPostionTop             = null!
                            )
    {
        var isWideDisplayChar = false;
        lock (_locker)
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.Write(placeholderChar);
            isWideDisplayChar = ((Console.CursorLeft - left) > 1);
            while (Console.CursorLeft != left)
            {
                Console.Write("\b \b");
            }
            Console.SetCursorPosition(left, top);
        }

        static ErrorCorrectionLevel ToErrorCorrectionLevel(string errorCorrectionLevel) =>
        errorCorrectionLevel.ToUpper()
        switch
        {
              "L"   => ErrorCorrectionLevel.L
            , "M"   => ErrorCorrectionLevel.M
            , "Q"   => ErrorCorrectionLevel.Q
            , "H"   => ErrorCorrectionLevel.H
            , _     => throw new ArgumentOutOfRangeException
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

        if (outputPostionLeft is not null)
        {
            Console.CursorTop = outputPostionTop.Value;
        }
        for (var i = 0; i < image.Width; i++)
        {
            if (outputPostionLeft is not null)
            {
                Console.CursorLeft = outputPostionLeft.Value;
            }
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
                Console.Write(placeholderChar);
                if (!isWideDisplayChar)
                {
                    Console.Write(placeholderChar);
                }
                Console.ResetColor();
            }
            //Console.ResetColor();
            Console.Write("\n");
        }
    }
}
