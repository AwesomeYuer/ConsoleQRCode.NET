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

                                // darkColorChar 与 lightColorChar 不一样时, 此时复制文本到使用某些字体的文本编辑器(notepad及字体不行, 由于'█'宽度为2)仍然显示为二维码外观
                                // darkColorChar 与 lightColorChar 一样时,   此时复制文本到使用某些字体的文本编辑器(notepad及字体不行, 由于'█'宽度为2)无法显示为二维码外观, 相当于禁止复制二维码文本
                                , char darkColorChar                            = '█'
                                , char lightColorChar                           = ' '
                            )
    {
        //Wide Char Detection
        var darkCharWidth   = darkColorChar.GetCharLength();
        var lightCharWidth  = lightColorChar.GetCharLength();

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

        for (var y = 0; y < bitMatrix.Height; y++)
        {
            if (outputPostionLeft is not null)
            {
                onOutputPostionLeftProcess(outputPostionLeft.Value);
            }

            for (var x = 0; x < bitMatrix.Width; x++)
            {
                var wideCharWidth = _wideCharWidth;

                var bit = bitMatrix[x, y];
                
                while (wideCharWidth > 0)
                {
                    onBitMatrixProcess(bit);

                    wideCharWidth -= bit ? lightCharWidth : darkCharWidth;
                }
                
                onColumnProcessed(x);
            }
            onRowProcessed(y);
        }
    }

    public static string GenerateQRCodeCharsText
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
                        , (y) =>
                        {
                            for (var i = 0; i < y; i++)
                            {
                                sb.AppendLine();
                            }
                        }
                        , (x) =>
                        {
                            for (var i = 0; i < x; i++)
                            {
                                sb.Append(' ');
                            }
                        }
                        , (matrixBit) =>
                        {
                            sb.Append(matrixBit ? lightColorChar : darkColorChar);
                        }
                        , (x) =>
                        {

                        }
                        , (y) =>
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


    public static string GenerateQRCodeCharsText
                            (
                                  string data

                                , int? outputPostionLeft                        = null
                                , int? outputPostionTop                         = null

                                , int marginInPixel                             = 1    
                    
                                , int widthInPixel                              = 10
                                , int heightInPixel                             = 10

                                , string errorCorrectionLevel                   = "M"

                                , char darkColorChar                            = '█'
                                , char lightColorChar                           = ' '

                                , string characterSet                           = nameof(Encoding.UTF8)
                            )
    {
        Dictionary<EncodeHintType, object> qrEncodeHints = new ()
        {
              { EncodeHintType.CHARACTER_SET            , characterSet              }
            , { EncodeHintType.ERROR_CORRECTION         , errorCorrectionLevel      }
            //, { EncodeHintType.QR_COMPACT               , qrCompact               }
            //, { EncodeHintType.PURE_BARCODE             , pureBarcode             }
            //, { EncodeHintType.QR_VERSION               , qrVersion               }
            //, { EncodeHintType.DISABLE_ECI              , disableECI              }
            //, { EncodeHintType.GS1_FORMAT               , gs1Format               }
            , { EncodeHintType.MARGIN                   , marginInPixel             }
            //, { EncodeHintType.widthInPixel             , widthInPixel            }
            //, { EncodeHintType.heightInPixel            , heightInPixel           }
        };

        return
            GenerateQRCodeCharsText
                (
                      data

                    , qrEncodeHints

                    , outputPostionLeft
                    , outputPostionTop

                    , widthInPixel
                    , heightInPixel

                    , darkColorChar
                    , lightColorChar
                );
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
                        , (y) =>
                        {
                            Console.CursorTop = y;
                        }
                        , (x) =>
                        {
                            Console.CursorLeft = x;
                        }
                        , (matrixBit) =>
                        {
                            Console
                                .BackgroundColor
                            = Console
                                .ForegroundColor
                            = matrixBit ? lightColor : darkColor;

                            @this.Write(matrixBit ? lightColorChar : darkColorChar);
                        }
                        , (x) =>
                        {
                            Console.ResetColor();
                        }
                        , (y) =>
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

                                , string errorCorrectionLevel           = "M"

                                , ConsoleColor darkColor                = ConsoleColor.Black
                                , ConsoleColor lightColor               = ConsoleColor.White

                                , char darkColorChar                    = '█'
                                , char lightColorChar                   = ' '

                                , string characterSet                   = nameof(Encoding.UTF8)
                            )
    {

        Dictionary<EncodeHintType, object> qrEncodeHints = new ()
        {
              { EncodeHintType.CHARACTER_SET            , characterSet              }
            , { EncodeHintType.ERROR_CORRECTION         , errorCorrectionLevel      }
            //, { EncodeHintType.QR_COMPACT               , qrCompact               }
            //, { EncodeHintType.PURE_BARCODE             , pureBarcode             }
            //, { EncodeHintType.QR_VERSION               , qrVersion               }
            //, { EncodeHintType.DISABLE_ECI              , disableECI              }
            //, { EncodeHintType.GS1_FORMAT               , gs1Format               }
            , { EncodeHintType.MARGIN                   , marginInPixel             }
            //, { EncodeHintType.widthInPixel             , widthInPixel            }
            //, { EncodeHintType.heightInPixel            , heightInPixel           }
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

                                , string errorCorrectionLevel           = "M"

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

                    , errorCorrectionLevel

                    , darkColor
                    , lightColor

                    , darkColorChar
                    , lightColorChar

                    , characterSet
                );
        @this.WriteLine();
    }

}
