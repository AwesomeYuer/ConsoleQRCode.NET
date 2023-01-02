﻿namespace Microshaoft;

using System;
using System.Collections;
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
        var darkCharWidth   = ConsoleText.CalculateCharLengthB(darkColorChar);
        var lightCharWidth  = ConsoleText.CalculateCharLengthB(lightColorChar);

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

                var b = bitMatrix[x, y];
                
                while (wideCharWidth > 0)
                {
                    onBitMatrixProcess(b);

                    wideCharWidth -= b ? lightCharWidth : darkCharWidth;
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
            //, { EncodeHintType.widthInPixel                    , widthInPixel     }
            //, { EncodeHintType.heightInPixel                   , heightInPixel    }
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

//file
public static class ConsoleText
{
    private struct CharLengthSegment : IComparable<CharLengthSegment>
    {
        public int Start;
        public int End;
        public int Length;

        public CharLengthSegment(int start, int end, int length)
        {
            Start       = start;
            End         = end;
            Length      = length;
        }

        public int InRange(int c)
        {
            if (c < Start)
            {
                return c - Start;
            }
            else if (c >= End)
            {
                return End - c + 1;
            }
            else
            {
                return 0;
            }
        }

        public int CompareTo(CharLengthSegment other)
        {
            return Start.CompareTo(other.Start);
        }
    }
    private class CharLengthSegments : IReadOnlyList<CharLengthSegment>
    {
        private readonly CharLengthSegment[] _charLengthSegments;
        public CharLengthSegments(CharLengthSegment[] charLengthSegments)
        {
            _charLengthSegments = charLengthSegments;
            Array.Sort(charLengthSegments);
        }

        public CharLengthSegment this[int index] => _charLengthSegments[index];

        public int Count => _charLengthSegments.Length;

        public CharLengthSegment BinarySearch(char c)
        {
            int cIndex = c;
            int
                low = 0,
                high = Count,
                middle;

            while (low <= high)
            {
                middle = (low + high) / 2;

                var segment = this[middle];

                if (cIndex < segment.Start)
                {
                    high = middle - 1;
                }
                else if (cIndex < segment.End)
                {
                    return segment;
                }
                else
                {
                    low = middle + 1;
                }
            }

            return default;
        }

        public IEnumerator<CharLengthSegment> GetEnumerator()
        {
            foreach (var segment in _charLengthSegments)
            {
                yield
                    return
                        segment;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _charLengthSegments.GetEnumerator();
        }
    }

    private static readonly CharLengthSegments AllCharsLengthSegments = new
    (
        new CharLengthSegment[]
        {
            new CharLengthSegment(0, 7, 1),
            new CharLengthSegment(7, 9, 0),
            new CharLengthSegment(9, 10, 8),
            new CharLengthSegment(10, 11, 0),
            new CharLengthSegment(11, 13, 1),
            new CharLengthSegment(13, 14, 0),
            new CharLengthSegment(14, 162, 1),
            new CharLengthSegment(162, 166, 2),
            new CharLengthSegment(166, 167, 1),
            new CharLengthSegment(167, 169, 2),
            new CharLengthSegment(169, 175, 1),
            new CharLengthSegment(175, 178, 2),
            new CharLengthSegment(178, 180, 1),
            new CharLengthSegment(180, 182, 2),
            new CharLengthSegment(182, 183, 1),
            new CharLengthSegment(183, 184, 2),
            new CharLengthSegment(184, 215, 1),
            new CharLengthSegment(215, 216, 2),
            new CharLengthSegment(216, 247, 1),
            new CharLengthSegment(247, 248, 2),
            new CharLengthSegment(248, 449, 1),
            new CharLengthSegment(449, 450, 2),
            new CharLengthSegment(450, 711, 1),
            new CharLengthSegment(711, 712, 2),
            new CharLengthSegment(712, 713, 1),
            new CharLengthSegment(713, 716, 2),
            new CharLengthSegment(716, 729, 1),
            new CharLengthSegment(729, 730, 2),
            new CharLengthSegment(730, 913, 1),
            new CharLengthSegment(913, 930, 2),
            new CharLengthSegment(930, 931, 1),
            new CharLengthSegment(931, 938, 2),
            new CharLengthSegment(938, 945, 1),
            new CharLengthSegment(945, 962, 2),
            new CharLengthSegment(962, 963, 1),
            new CharLengthSegment(963, 970, 2),
            new CharLengthSegment(970, 1025, 1),
            new CharLengthSegment(1025, 1026, 2),
            new CharLengthSegment(1026, 1040, 1),
            new CharLengthSegment(1040, 1104, 2),
            new CharLengthSegment(1104, 1105, 1),
            new CharLengthSegment(1105, 1106, 2),
            new CharLengthSegment(1106, 8208, 1),
            new CharLengthSegment(8208, 8209, 2),
            new CharLengthSegment(8209, 8211, 1),
            new CharLengthSegment(8211, 8215, 2),
            new CharLengthSegment(8215, 8216, 1),
            new CharLengthSegment(8216, 8218, 2),
            new CharLengthSegment(8218, 8220, 1),
            new CharLengthSegment(8220, 8222, 2),
            new CharLengthSegment(8222, 8229, 1),
            new CharLengthSegment(8229, 8231, 2),
            new CharLengthSegment(8231, 8240, 1),
            new CharLengthSegment(8240, 8241, 2),
            new CharLengthSegment(8241, 8242, 1),
            new CharLengthSegment(8242, 8244, 2),
            new CharLengthSegment(8244, 8245, 1),
            new CharLengthSegment(8245, 8246, 2),
            new CharLengthSegment(8246, 8251, 1),
            new CharLengthSegment(8251, 8252, 2),
            new CharLengthSegment(8252, 8254, 1),
            new CharLengthSegment(8254, 8255, 2),
            new CharLengthSegment(8255, 8364, 1),
            new CharLengthSegment(8364, 8365, 2),
            new CharLengthSegment(8365, 8451, 1),
            new CharLengthSegment(8451, 8452, 2),
            new CharLengthSegment(8452, 8453, 1),
            new CharLengthSegment(8453, 8454, 2),
            new CharLengthSegment(8454, 8457, 1),
            new CharLengthSegment(8457, 8458, 2),
            new CharLengthSegment(8458, 8470, 1),
            new CharLengthSegment(8470, 8471, 2),
            new CharLengthSegment(8471, 8481, 1),
            new CharLengthSegment(8481, 8482, 2),
            new CharLengthSegment(8482, 8544, 1),
            new CharLengthSegment(8544, 8556, 2),
            new CharLengthSegment(8556, 8560, 1),
            new CharLengthSegment(8560, 8570, 2),
            new CharLengthSegment(8570, 8592, 1),
            new CharLengthSegment(8592, 8596, 2),
            new CharLengthSegment(8596, 8598, 1),
            new CharLengthSegment(8598, 8602, 2),
            new CharLengthSegment(8602, 8712, 1),
            new CharLengthSegment(8712, 8713, 2),
            new CharLengthSegment(8713, 8719, 1),
            new CharLengthSegment(8719, 8720, 2),
            new CharLengthSegment(8720, 8721, 1),
            new CharLengthSegment(8721, 8722, 2),
            new CharLengthSegment(8722, 8725, 1),
            new CharLengthSegment(8725, 8726, 2),
            new CharLengthSegment(8726, 8728, 1),
            new CharLengthSegment(8728, 8729, 2),
            new CharLengthSegment(8729, 8730, 1),
            new CharLengthSegment(8730, 8731, 2),
            new CharLengthSegment(8731, 8733, 1),
            new CharLengthSegment(8733, 8737, 2),
            new CharLengthSegment(8737, 8739, 1),
            new CharLengthSegment(8739, 8740, 2),
            new CharLengthSegment(8740, 8741, 1),
            new CharLengthSegment(8741, 8742, 2),
            new CharLengthSegment(8742, 8743, 1),
            new CharLengthSegment(8743, 8748, 2),
            new CharLengthSegment(8748, 8750, 1),
            new CharLengthSegment(8750, 8751, 2),
            new CharLengthSegment(8751, 8756, 1),
            new CharLengthSegment(8756, 8760, 2),
            new CharLengthSegment(8760, 8764, 1),
            new CharLengthSegment(8764, 8766, 2),
            new CharLengthSegment(8766, 8776, 1),
            new CharLengthSegment(8776, 8777, 2),
            new CharLengthSegment(8777, 8780, 1),
            new CharLengthSegment(8780, 8781, 2),
            new CharLengthSegment(8781, 8786, 1),
            new CharLengthSegment(8786, 8787, 2),
            new CharLengthSegment(8787, 8800, 1),
            new CharLengthSegment(8800, 8802, 2),
            new CharLengthSegment(8802, 8804, 1),
            new CharLengthSegment(8804, 8808, 2),
            new CharLengthSegment(8808, 8814, 1),
            new CharLengthSegment(8814, 8816, 2),
            new CharLengthSegment(8816, 8853, 1),
            new CharLengthSegment(8853, 8854, 2),
            new CharLengthSegment(8854, 8857, 1),
            new CharLengthSegment(8857, 8858, 2),
            new CharLengthSegment(8858, 8869, 1),
            new CharLengthSegment(8869, 8870, 2),
            new CharLengthSegment(8870, 8895, 1),
            new CharLengthSegment(8895, 8896, 2),
            new CharLengthSegment(8896, 8978, 1),
            new CharLengthSegment(8978, 8979, 2),
            new CharLengthSegment(8979, 9312, 1),
            new CharLengthSegment(9312, 9322, 2),
            new CharLengthSegment(9322, 9332, 1),
            new CharLengthSegment(9332, 9372, 2),
            new CharLengthSegment(9372, 9632, 1),
            new CharLengthSegment(9632, 9634, 2),
            new CharLengthSegment(9634, 9650, 1),
            new CharLengthSegment(9650, 9652, 2),
            new CharLengthSegment(9652, 9660, 1),
            new CharLengthSegment(9660, 9662, 2),
            new CharLengthSegment(9662, 9670, 1),
            new CharLengthSegment(9670, 9672, 2),
            new CharLengthSegment(9672, 9675, 1),
            new CharLengthSegment(9675, 9676, 2),
            new CharLengthSegment(9676, 9678, 1),
            new CharLengthSegment(9678, 9680, 2),
            new CharLengthSegment(9680, 9698, 1),
            new CharLengthSegment(9698, 9702, 2),
            new CharLengthSegment(9702, 9733, 1),
            new CharLengthSegment(9733, 9735, 2),
            new CharLengthSegment(9735, 9737, 1),
            new CharLengthSegment(9737, 9738, 2),
            new CharLengthSegment(9738, 9792, 1),
            new CharLengthSegment(9792, 9793, 2),
            new CharLengthSegment(9793, 9794, 1),
            new CharLengthSegment(9794, 9795, 2),
            new CharLengthSegment(9795, 12288, 1),
            new CharLengthSegment(12288, 12292, 2),
            new CharLengthSegment(12292, 12293, 1),
            new CharLengthSegment(12293, 12312, 2),
            new CharLengthSegment(12312, 12317, 1),
            new CharLengthSegment(12317, 12319, 2),
            new CharLengthSegment(12319, 12321, 1),
            new CharLengthSegment(12321, 12330, 2),
            new CharLengthSegment(12330, 12353, 1),
            new CharLengthSegment(12353, 12436, 2),
            new CharLengthSegment(12436, 12443, 1),
            new CharLengthSegment(12443, 12447, 2),
            new CharLengthSegment(12447, 12449, 1),
            new CharLengthSegment(12449, 12535, 2),
            new CharLengthSegment(12535, 12540, 1),
            new CharLengthSegment(12540, 12543, 2),
            new CharLengthSegment(12543, 12549, 1),
            new CharLengthSegment(12549, 12586, 2),
            new CharLengthSegment(12586, 12690, 1),
            new CharLengthSegment(12690, 12704, 2),
            new CharLengthSegment(12704, 12832, 1),
            new CharLengthSegment(12832, 12868, 2),
            new CharLengthSegment(12868, 12928, 1),
            new CharLengthSegment(12928, 12958, 2),
            new CharLengthSegment(12958, 12959, 1),
            new CharLengthSegment(12959, 12964, 2),
            new CharLengthSegment(12964, 12969, 1),
            new CharLengthSegment(12969, 12977, 2),
            new CharLengthSegment(12977, 13198, 1),
            new CharLengthSegment(13198, 13200, 2),
            new CharLengthSegment(13200, 13212, 1),
            new CharLengthSegment(13212, 13215, 2),
            new CharLengthSegment(13215, 13217, 1),
            new CharLengthSegment(13217, 13218, 2),
            new CharLengthSegment(13218, 13252, 1),
            new CharLengthSegment(13252, 13253, 2),
            new CharLengthSegment(13253, 13262, 1),
            new CharLengthSegment(13262, 13263, 2),
            new CharLengthSegment(13263, 13265, 1),
            new CharLengthSegment(13265, 13267, 2),
            new CharLengthSegment(13267, 13269, 1),
            new CharLengthSegment(13269, 13270, 2),
            new CharLengthSegment(13270, 19968, 1),
            new CharLengthSegment(19968, 40870, 2),
            new CharLengthSegment(40870, 55296, 1),
            new CharLengthSegment(55296, 55297, 0),
            new CharLengthSegment(55297, 56320, 1),
            new CharLengthSegment(56320, 56321, 2),
            new CharLengthSegment(56321, 57344, 1),
            new CharLengthSegment(57344, 59335, 2),
            new CharLengthSegment(59335, 59337, 1),
            new CharLengthSegment(59337, 59493, 2),
            new CharLengthSegment(59493, 63733, 1),
            new CharLengthSegment(63733, 63734, 2),
            new CharLengthSegment(63734, 63744, 1),
            new CharLengthSegment(63744, 64046, 2),
            new CharLengthSegment(64046, 65072, 1),
            new CharLengthSegment(65072, 65074, 2),
            new CharLengthSegment(65074, 65075, 1),
            new CharLengthSegment(65075, 65093, 2),
            new CharLengthSegment(65093, 65097, 1),
            new CharLengthSegment(65097, 65107, 2),
            new CharLengthSegment(65107, 65108, 1),
            new CharLengthSegment(65108, 65112, 2),
            new CharLengthSegment(65112, 65113, 1),
            new CharLengthSegment(65113, 65127, 2),
            new CharLengthSegment(65127, 65128, 1),
            new CharLengthSegment(65128, 65132, 2),
            new CharLengthSegment(65132, 65281, 1),
            new CharLengthSegment(65281, 65375, 2),
            new CharLengthSegment(65375, 65504, 1),
            new CharLengthSegment(65504, 65510, 2),
            new CharLengthSegment(65510, 65536, 1),
        }
    );

    public static bool IsWideChar(char c)
    {
        return
            CalculateCharLengthB(c) > 1;
    }
    public static int CalculateCharLengthB(char c)
    {
        return
            AllCharsLengthSegments
                            .BinarySearch(c)
                            .Length;
    }
    public static int CalculateStringLengthB(string s)
    {
        return
            s
                .Select
                    (
                        CalculateCharLengthB
                    )
                .Sum();
    }
}
