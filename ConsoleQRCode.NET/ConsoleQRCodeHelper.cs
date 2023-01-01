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

    public static void PrintQRCode
                            (
                                this TextWriter @this
                                , string data

                                , IDictionary
                                        <EncodeHintType, object>
                                                qrEncodeHints

                                , int? outputPostionLeft            = null
                                , int? outputPostionTop             = null

                                , int widthInPixel                  = 10
                                , int heightInPixel                 = 10

                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White

                                , char placeholderChar              = '囍'
                            )
    {
        _ = @this;

        // Wide Char Detection
        var isWideChar = false;
        lock (_locker)
        {
            (int left, int top) = Console.GetCursorPosition();
            Console.Write(placeholderChar);
            isWideChar = (Console.CursorLeft - left) > 1;
            while (Console.CursorLeft != left)
            {
                Console.Write("\b \b");
            }
            Console.SetCursorPosition(left, top);
        }

        QRCodeWriter qrCodeWriter = new ();
        BitMatrix bitMatrix;

        if (qrEncodeHints is null)
        {
            bitMatrix = qrCodeWriter
                                .encode
                                    (
                                        data
                                        , BarcodeFormat.QR_CODE
                                        , widthInPixel
                                        , heightInPixel
                                    );
        }
        else
        {
            bitMatrix = qrCodeWriter
                                .encode
                                    (
                                        data
                                        , BarcodeFormat.QR_CODE
                                        , widthInPixel
                                        , heightInPixel
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
                Console
                    .BackgroundColor
                = Console
                    .ForegroundColor
                = bitMatrix[i, j] ? lightColor : darkColor;

                Console.Write(placeholderChar);
                if (!isWideChar)
                {
                    wideCharWidth -= charWidth;
                    Console.Write(placeholderChar);
                }
                Console.ResetColor();
            }
            //Console.ResetColor();
            Console.Write("\n");
        }
    }

    public static void PrintQRCode
                            (
                                this TextWriter @this
                                , string data

                                , int? outputPostionLeft            = null
                                , int? outputPostionTop             = null

                                , int marginInPixel                 = 1

                                , int widthInPixel                  = 10
                                , int heightInPixel                 = 10

                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White

                                , string characterSet               = nameof(Encoding.UTF8)
                                , char placeholderChar              = '囍'
                            )
    {

        Dictionary<EncodeHintType, object> qrEncodeHints = new ()
        {
              { EncodeHintType.CHARACTER_SET            , characterSet              }
            //, { EncodeHintType.ERROR_CORRECTION         , errorCorrectionLevel    }
            //, { EncodeHintType.QR_COMPACT               , qrCompact               }
            //, { EncodeHintType.PURE_BARCODE             , pureBarcode             }
            //, { EncodeHintType.QR_VERSION               , qrVersion               }
            //, { EncodeHintType.DISABLE_ECI              , disableECI              }
            //, { EncodeHintType.GS1_FORMAT               , gs1Format               }
            , { EncodeHintType.MARGIN                   , marginInPixel             }
            //, { EncodeHintType.widthInPixel                    , widthInPixel     }
            //, { EncodeHintType.heightInPixel                   , heightInPixel    }
        };

        PrintQRCode
            (
                @this

                , data

                , qrEncodeHints

                , outputPostionLeft
                , outputPostionTop

                , widthInPixel
                , heightInPixel

                , darkColor
                , lightColor

                , placeholderChar
            );
    }

    public static void PrintQRCodeLine
                            (
                                this TextWriter @this
                                , string data

                                , int? outputPostionLeft            = null
                                , int? outputPostionTop             = null

                                , int marginInPixel                 = 1

                                , int widthInPixel                  = 10
                                , int heightInPixel                 = 10

                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White

                                , string characterSet               = nameof(Encoding.UTF8)
                                , char placeholderChar              = '囍'
                            )
    {
        PrintQRCode
                (
                    @this
                    , data

                    , outputPostionLeft
                    , outputPostionTop

                    , marginInPixel

                    , widthInPixel
                    , heightInPixel

                    , darkColor
                    , lightColor

                    , characterSet
                    , placeholderChar
                );
        Console.WriteLine();
    }
   
    public static void PrintQRCodeLine
                            (
                                this TextWriter @this
                                , string data

                                , IDictionary
                                        <EncodeHintType, object>
                                                qrEncodeHints

                                , int? outputPostionLeft            = null
                                , int? outputPostionTop             = null

                                , int widthInPixel                  = 10
                                , int heightInPixel                 = 10

                                , ConsoleColor darkColor            = ConsoleColor.Black
                                , ConsoleColor lightColor           = ConsoleColor.White

                                , char placeholderChar              = '囍'
                            )
    {
        PrintQRCode
                (
                    @this

                    , data

                    , qrEncodeHints

                    , outputPostionLeft
                    , outputPostionTop

                    , widthInPixel
                    , heightInPixel

                    , darkColor
                    , lightColor
                
                    , placeholderChar
                );
        Console.WriteLine();
    }
}
