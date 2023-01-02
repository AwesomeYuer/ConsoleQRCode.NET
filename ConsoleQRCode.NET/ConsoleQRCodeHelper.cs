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
    private const int _wideCharWidth = 2;


    private static void QRCodeBitMatrixProcess
                    (
                        string data

                        , IDictionary
                                <EncodeHintType, object>
                                        qrEncodeHints
                        
                        , Action<int>   onOutputPostionTopProcess
                        , Action<int>   onOutputPostionLeftProcess
                        , Action<bool>  onBitMatrixProcess
                        , Action<int>   onColumnProcessed
                        , Action<int>   onRowProcessed

                        , int? outputPostionLeft                        = null
                        , int? outputPostionTop                         = null

                        , int widthInPixel                              = 10
                        , int heightInPixel                             = 10

                        , char darkColorChar                            = '█'
                        , char lightColorChar                           = ' '
                    )
    {
        //Wide Char Detection
        var darkCharWidth   = ConsoleText.CalcCharLength(darkColorChar);
        var lightCharWidth  = ConsoleText.CalcCharLength(lightColorChar);

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
            onOutputPostionTopProcess(outputPostionTop.Value);
        }

        for (var i = 0; i < bitMatrix.Height; i++)
        {
            if (outputPostionLeft is not null)
            {
                onOutputPostionLeftProcess(outputPostionLeft.Value);
            }
            for (var j = 0; j < bitMatrix.Width; j++)
            {
                var wideCharWidth = _wideCharWidth;
                var b = bitMatrix[j, i];
                if (b)
                {
                    while (wideCharWidth > 0)
                    {
                        onBitMatrixProcess(b);
                        wideCharWidth -= lightCharWidth;
                    }
                }
                else
                {
                    while (wideCharWidth > 0)
                    {
                        onBitMatrixProcess(b);
                        wideCharWidth -= darkCharWidth;
                    }
                }
                onColumnProcessed(j);
            }
            onRowProcessed(i);
        }
    }

    public static string GetQRCodeConsoleText
                        (
                            string data

                            , IDictionary
                                    <EncodeHintType, object>
                                            qrEncodeHints

                            , int? outputPostionLeft                    = null
                            , int? outputPostionTop                     = null

                            , int widthInPixel                          = 10
                            , int heightInPixel                         = 10

                            , char darkColorChar                        = '█'
                            , char lightColorChar                       = ' '
                        )
    {
        var sb = new StringBuilder();

        QRCodeBitMatrixProcess
                    (
                        data
                        , qrEncodeHints
                        , (left) =>
                        {
                            for (var i = 0; i < left; i++)
                            {
                                sb.AppendLine();
                            }
                        }
                        , (top) =>
                        {
                            for (var i = 0; i < top; i++)
                            {
                                sb.Append(' ');
                            }
                        }
                        , (bitMatrix) =>
                        {
                            sb.Append(bitMatrix ? lightColorChar : darkColorChar);
                        }
                        , (column) =>
                        {

                        }
                        , (row) =>
                        {
                            sb.AppendLine();
                        }
                        , outputPostionLeft
                        , outputPostionTop

                        , widthInPixel
                        , heightInPixel

                        , darkColorChar
                        , lightColorChar

                    );
        return sb.ToString();
    }

    public static void PrintQRCode
                            (
                                this TextWriter @this
                                , string data

                                , IDictionary
                                        <EncodeHintType, object>
                                                qrEncodeHints

                                , int? outputPostionLeft                = null
                                , int? outputPostionTop                 = null

                                , int widthInPixel                      = 10
                                , int heightInPixel                     = 10

                                , ConsoleColor darkColor                = ConsoleColor.Black
                                , ConsoleColor lightColor               = ConsoleColor.White

                                , char darkColorChar                    = '█'
                                , char lightColorChar                   = ' '

                            )
    {
        QRCodeBitMatrixProcess
                    (
                        data
                        , qrEncodeHints
                        , (left) =>
                        {
                            Console.CursorTop = outputPostionTop!.Value;
                        }
                        , (top) =>
                        {
                            Console.CursorLeft = outputPostionLeft!.Value;
                        }
                        , (bitMatrix) =>
                        {
                            Console
                                .BackgroundColor
                            = Console
                                .ForegroundColor
                            = bitMatrix ? lightColor : darkColor;
                            @this.Write(bitMatrix ? lightColorChar : darkColorChar);
                        }
                        , (column) =>
                        {
                            Console.ResetColor();
                        }
                        , (row) =>
                        {
                            Console.Write('\n');
                        }
                        , outputPostionLeft
                        , outputPostionTop

                        , widthInPixel
                        , heightInPixel

                        , darkColorChar
                        , lightColorChar

                    );
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

                                , char darkColorChar                = '█'
                                , char lightColorChar               = ' '

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

                    , darkColorChar
                    , lightColorChar

                );
        @this.WriteLine();
    }

    public static void PrintQRCode
                            (
                                this TextWriter @this
                                , string data

                                , int? outputPostionLeft                = null
                                , int? outputPostionTop                 = null

                                , int marginInPixel                     = 1

                                , int widthInPixel                      = 10
                                , int heightInPixel                     = 10

                                , ConsoleColor darkColor                = ConsoleColor.Black
                                , ConsoleColor lightColor               = ConsoleColor.White

                                , char darkColorChar                    = '█'
                                , char lightColorChar                   = ' '

                                , string characterSet                   = nameof(Encoding.UTF8)
                            )
    {

        Dictionary<EncodeHintType, object> qrEncodeHints = new()
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

                , darkColorChar
                , lightColorChar
            );
    }

    public static void PrintQRCodeLine
                            (
                                this TextWriter @this
                                , string data

                                , int? outputPostionLeft                = null
                                , int? outputPostionTop                 = null

                                , int marginInPixel                     = 1

                                , int widthInPixel                      = 10
                                , int heightInPixel                     = 10

                                , ConsoleColor darkColor                = ConsoleColor.Black
                                , ConsoleColor lightColor               = ConsoleColor.White

                                , char darkColorChar                    = '█'
                                , char lightColorChar                   = ' '

                                , string characterSet                   = nameof(Encoding.UTF8)
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

                    , darkColorChar
                    , lightColorChar

                    , characterSet
                );
        @this.WriteLine();
    }

}
