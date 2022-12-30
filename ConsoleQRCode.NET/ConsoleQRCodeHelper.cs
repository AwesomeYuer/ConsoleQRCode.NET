namespace Microshaoft;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

public static class ConsoleQRCodeHelper
{
    private static readonly object _locker = new ();

    public static void WriteQRCode
                    (
                        this TextWriter @this
                        , string data

                        , IDictionary
                                <EncodeHintType, object>
                                        qrEncodeHints

                        , int width = 10
                        , int height = 10

                        , ConsoleColor darkColor = ConsoleColor.Black
                        , ConsoleColor lightColor = ConsoleColor.White

                        , char placeholderChar = '囍'

                        , int? outputPostionLeft = null!
                        , int? outputPostionTop = null!
                    )
    {
        _ = @this;

        // Wide Char Detection
        var isWideChar = false;
        lock (_locker)
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.Write(placeholderChar);
            isWideChar = ((Console.CursorLeft - left) > 1);
            while (Console.CursorLeft != left)
            {
                Console.Write("\b \b");
            }
            Console.SetCursorPosition(left, top);
        }

        QRCodeWriter qrCodeWriter = new();
        BitMatrix bitMatrix;

        if (qrEncodeHints is null)
        {
            bitMatrix = qrCodeWriter
                                .encode
                                    (
                                        data
                                        , BarcodeFormat.QR_CODE
                                        , width
                                        , height
                                    );
        }
        else
        {
            bitMatrix = qrCodeWriter
                                .encode
                                    (
                                        data
                                        , BarcodeFormat.QR_CODE
                                        , width
                                        , height
                                        , qrEncodeHints
                                    );
        }

        if (outputPostionTop is not null)
        {
            Console.CursorTop = outputPostionTop.Value;
        }
        for (var i = 0; i < bitMatrix.Width; i++)
        {
            if (outputPostionLeft is not null)
            {
                Console.CursorLeft = outputPostionLeft.Value;
            }
            for (var j = 0; j < bitMatrix.Height; j++)
            {
                if (!bitMatrix[i, j])
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
                if (!isWideChar)
                {
                    Console.Write(placeholderChar);
                }
                Console.ResetColor();
            }
            //Console.ResetColor();
            Console.Write("\n");
        }
    }

    public static void WriteQRCode
                        (
                            this TextWriter @this
                            , string data

                            , int width = 10
                            , int height = 10
                            , int margin = 1

                            , string characterSet = nameof(Encoding.UTF8)

                            , ConsoleColor darkColor = ConsoleColor.Black
                            , ConsoleColor lightColor = ConsoleColor.White

                            , char placeholderChar = '囍'

                            , int? outputPostionLeft = null!
                            , int? outputPostionTop = null!
                        )
    {

        Dictionary<EncodeHintType, object> qrEncodeHints = new()
        {
              { EncodeHintType.CHARACTER_SET            , characterSet                  }
            //, { EncodeHintType.ERROR_CORRECTION         , errorCorrectionLevel          }
            //, { EncodeHintType.QR_COMPACT               , qrCompact                     }
            //, { EncodeHintType.PURE_BARCODE             , pureBarcode                   }
            //, { EncodeHintType.QR_VERSION               , qrVersion                     }
            //, { EncodeHintType.DISABLE_ECI              , disableECI                    }
            //, { EncodeHintType.GS1_FORMAT               , gs1Format                     }
            , { EncodeHintType.MARGIN                   , margin                        }
            //, { EncodeHintType.WIDTH                    , width                         }
            //, { EncodeHintType.HEIGHT                   , height                        }
        };

        WriteQRCode
            (
                @this

                , data

                , qrEncodeHints

                , width
                , height

                , darkColor
                , lightColor

                , placeholderChar

                , outputPostionLeft
                , outputPostionTop
            );
    }

    public static void WriteQRCodeLine
                            (
                                this TextWriter @this
                                , string data

                                , int width                         = 10
                                , int height                        = 10
                                , int margin                        = 1

                                , string characterSet               = nameof(Encoding.UTF8)
                                
                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White

                                , char placeholderChar              = '囍'

                                , int? outputPostionLeft            = null!
                                , int? outputPostionTop             = null!
                            )
    {
        WriteQRCode
            (
                @this
                , data

                , width
                , height
                , margin

                , characterSet

                , darkColor
                , lightColor

                , placeholderChar

                , outputPostionLeft
                , outputPostionTop
            );
        Console.WriteLine();
    }
   
    public static void WriteQRCodeLine
                        (
                            this TextWriter @this
                            , string data

                            , IDictionary
                                    <EncodeHintType, object>
                                            qrEncodeHints

                            , int width                         = 10
                            , int height                        = 10

                            , ConsoleColor darkColor            = ConsoleColor.Black
                            , ConsoleColor lightColor           = ConsoleColor.White

                            , char placeholderChar              = '囍'

                            , int? outputPostionLeft            = null!
                            , int? outputPostionTop             = null!
                        )
    {
        WriteQRCode
            (
                @this

                , data

                , qrEncodeHints
                
                , width
                , height

                , darkColor
                , lightColor
                
                , placeholderChar
                
                , outputPostionLeft
                , outputPostionTop
            );
        Console.WriteLine();
    }
}
