using Microshaoft;
using System.Text;
using ZXing;

Console.WriteLine("======");
Console.WriteLine("======\b \b");
Console.WriteLine("======");

var s = $@"AwesomeYuer于斯人也@Microshaoft
                             AwesomeYuer于斯人也@Microshaoft";

var @value = string.Empty;
Console.WriteLine
            (
                @value = ConsoleQRCodeHelper
                                .GenerateQRCodeConsoleText
                                    (
                                        s
                                        , new Dictionary<EncodeHintType, object>()
                                            {
                                                { EncodeHintType.CHARACTER_SET              , nameof(Encoding.UTF8) }   //编码字符集
                                                , { EncodeHintType.ERROR_CORRECTION         , "L"                   }   //编码纠错级别   : L, M, Q, H
                                                , { EncodeHintType.QR_COMPACT               , true                  }   //编码是否压缩
                                                , { EncodeHintType.PURE_BARCODE             , true                  }   //编码是否纯条码
                                                //, { EncodeHintType.QR_VERSION               , 10                  }   //编码版本
                                                //, { EncodeHintType.DISABLE_ECI              , true                }   //编码是否禁用ECI编码段
                                                //, { EncodeHintType.GS1_FORMAT               , true                }   //编码是否GS1格式
                                                , { EncodeHintType.MARGIN                   , 1                     }
                                                //, { EncodeHintType.WIDTH                    , 15                  }
                                                //, { EncodeHintType.HEIGHT                   , 15                  }
                                            }
                                        , 12
                                        , 5
                                    // darkColorChar 与 lightColorChar 不一样时, 此时复制文本到使用某些字体的文本编辑器(notepad及字体不行, 由于'█'宽度为2)仍然显示为二维码外观
                                    // darkColorChar 与 lightColorChar 一样时,   此时复制文本到使用某些字体的文本编辑器(notepad及字体不行, 由于'█'宽度为2)无法显示为二维码外观, 相当于禁止复制二维码文本
                                    //, darkColorChar     : '█'          //二维码输出深颜色占位符       :   同时支持宽或窄字符, 缺省值为 : '█'
                                    //, lightColorChar    : '囍'         //二维码输出浅颜色占位符       :   同时支持宽或窄字符, 缺省值为 : ' '

                                    )
            );

System.Console.Out.PrintQRCodeLine
            (
                  s                                             //data                          :   待生成二维码原始数据

                , 100                                           //outputPostionLeft             :   二维码输出横向位置
                , 5                                             //outputPostionTop              :   二维码输出纵向位置

                , 2                                             //marginInPixel                 :   二维码图像生成边缘空白宽高度

                , 30                                            //widthInPixel                  :   二维码图像生成宽度
                , 30                                            //heightInPixel                 :   二维码图像生成高度

                , "L"                                           //errorCorrectionLevel          :   二维码纠错级别

                , ConsoleColor.White                            //darkColor                     :   二维码输出深颜色
                , ConsoleColor.Red                              //lightColor                    :   二维码输出浅颜色

                // 当 darkColorChar 与 lightColorChar 不一样时, 此时复制文本到使用某些字体的文本编辑器(notepad及默认字体不行)仍然显示为二维码外观
                // 当 darkColorChar 与 lightColorChar 一样时,   此时复制文本到使用某些字体的文本编辑器(notepad及默认字体不行)无法显示为二维码外观, 相当于禁止复制二维码文本
                , '囍'                                          //二维码输出深颜色占位符            :   同时支持宽或窄字符, 缺省值为 : '█'
                , '█'                                           //二维码输出浅颜色占位符            :   同时支持宽或窄字符, 缺省值为 : ' '

                , nameof(Encoding.UTF8)                         //二维码字符集                     :   utf-8 支持中文不乱码
                
            );

Console.Out.PrintQRCodeLine
                (
                    "https://www.cnblogs.com/stulzq/p/14282461.html?Thanks"     //待生成二维码原始数据
                    , 15                                                        //outputPostionLeft        :   二维码输出横向位置
                    , 50                                                        //outputPostionTop         :   二维码输出纵向位置
                    , darkColorChar     :   '$'                                 //二维码输出深颜色占位符      :   同时支持宽或窄字符, 窄: !@# , 宽: ㊚㊛囍♀♂♂♀☿♁⚢⚣⚤⚥⚦⚧⚨
                    , lightColorChar    :   '$'
                );

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();

s =
"""
♂㊚囍㊛♀
满屏荒唐言
一把辛酸泪
都言作者痴
谁解比特位
~ Duang ~

♂㊚囍㊛♀
满屏荒唐言
一把辛酸泪
都言作者痴
谁解比特位
~ Duang ~

♂㊚囍㊛♀
满屏荒唐言
一把辛酸泪
都言作者痴
谁解比特位
~ Duang ~
""";
Console.Out.PrintQRCodeLine
            (
                s
                , new Dictionary<EncodeHintType, object>()
                    {
                          { EncodeHintType.CHARACTER_SET            , nameof(Encoding.UTF8) }   //编码字符集
                        , { EncodeHintType.ERROR_CORRECTION         , "L"                   }   //编码纠错级别   : L, M, Q, H
                        , { EncodeHintType.QR_COMPACT               , true                  }   //编码是否压缩
                        , { EncodeHintType.PURE_BARCODE             , true                  }   //编码是否纯条码
                        //, { EncodeHintType.QR_VERSION               , 10                  }   //编码版本
                        //, { EncodeHintType.DISABLE_ECI              , true                }   //编码是否禁用ECI编码段
                        //, { EncodeHintType.GS1_FORMAT               , true                }   //编码是否GS1格式
                        , { EncodeHintType.MARGIN                   , 3                     }
                        //, { EncodeHintType.WIDTH                    , 15                  }
                        //, { EncodeHintType.HEIGHT                   , 15                  }
                    }

                , 20                                                //outputPostionLeft     :   控制台二维码输出横向位置
                , darkColor             : ConsoleColor.Yellow
                , lightColor            : ConsoleColor.DarkBlue
                , darkColorChar       : '$'                       //控制台二维码输出占位符   :   窄字符 $
            );
s = $@"AwesomeYuer 于斯人也 한국어 ことに доступны ㊚㊛囍♀♂♂♀☿♁⚢⚣⚤⚥⚦⚧⚨ 🌍💩";

Console.Out.PrintQRCodeLine
            (
                s       //待生成二维码原始数据
            );