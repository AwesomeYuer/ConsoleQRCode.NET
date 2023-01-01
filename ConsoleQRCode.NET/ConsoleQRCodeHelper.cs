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

    public static string GetQRCodeConsoleText
                        (
                            string data

                            , IDictionary
                                    <EncodeHintType, object>
                                            qrEncodeHints

                            , int? outputPostionLeft                = null
                            , int? outputPostionTop                 = null

                            , int widthInPixel                      = 10
                            , int heightInPixel                     = 10

                            , char darkChar                         = '█'
                            , char lightChar                        = ' '
                        )
    {
        var sb = new StringBuilder();

        //Wide Char Detection
        var darkCharWidth = ConsoleText.CalcCharLength(darkChar);
        var lightCharWidth = ConsoleText.CalcCharLength(lightChar);

        //lock (_locker)
        //{
        //    (int left, int top) = Console.GetCursorPosition();
        //    Console.Write(darkChar);
        //    darkCharWidth = Console.CursorLeft - left;
        //    while (Console.CursorLeft != left)
        //    {
        //        Console.Write("\b \b");
        //    }
        //    Console.SetCursorPosition(left, top);

        //    (left, top) = Console.GetCursorPosition();
        //    Console.Write(lightChar);
        //    lightCharWidth = Console.CursorLeft - left;
        //    while (Console.CursorLeft != left)
        //    {
        //        Console.Write("\b \b");
        //    }
        //    Console.SetCursorPosition(left, top);
        //}

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
            for (var i = 0; i < outputPostionTop.Value; i++)
            {
                sb.AppendLine();
            }
        }

        for (var i = 0; i < bitMatrix.Width; i++)
        {
            if (outputPostionLeft is not null)
            {
                for (var ii = 0; ii < outputPostionLeft.Value; ii++)
                {
                    sb.Append(' ');
                }
            }
            for (var j = 0; j < bitMatrix.Height; j++)
            {
                var wideCharWidth = _wideCharWidth;
                if (bitMatrix[i, j])
                {
                    while (wideCharWidth > 0)
                    {
                        sb.Append(lightChar);
                        wideCharWidth -= lightCharWidth;
                    }
                }
                else
                {
                    while (wideCharWidth > 0)
                    {
                        sb.Append(darkChar);
                        wideCharWidth -= darkCharWidth;
                    }
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }



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

                                , char placeholderChar              = '█'   //控制台二维码输出占位符参数缺省值为 : '█' ,二维码输出后,此时选择控制台屏幕文本,
                                                                            //并拷贝到文本文件,再使用某些字体(如:Consolas)的文本编辑器(Windows notepad及默认字体不行)打开,仍然显示为二维码外观,
                                                                            //使用其他字符作为二维码输出占位符,文本文件中仅显示该字符,相当于禁止拷贝二维码文本
                            )
    {
        _ = @this;

        // Wide Char Detection
        var charWidth = 0;
        if (placeholderChar != '█')
        {
            //lock (_locker)
            //{
            //    (int left, int top) = Console.GetCursorPosition();
            //    @this.Write(placeholderChar);
            //    charWidth = Console.CursorLeft - left;
            //    while (Console.CursorLeft != left)
            //    {
            //        @this.Write("\b \b");
            //    }
            //    Console.SetCursorPosition(left, top);
            //}
            charWidth = ConsoleText.CalcCharLength( placeholderChar );
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

                if (placeholderChar == '█')
                {
                    @this.Write(bitMatrix[i, j] ? "  " : "██");
                }
                else
                {
                    var wideCharWidth = _wideCharWidth;
                    while (wideCharWidth > 0)
                    {
                        @this.Write(placeholderChar);
                        wideCharWidth -= charWidth;
                    }
                }
                Console.ResetColor();
            }
            //Console.ResetColor();
            @this.Write("\n");
        }
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

                                , char placeholderChar              = '█'
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
        @this.WriteLine();
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
                                , char placeholderChar              = '█'
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
                                , char placeholderChar              = '█'
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
        @this.WriteLine();
    }
  
}
