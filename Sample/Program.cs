using Microshaoft;
using System;
using System.Collections.Generic;
using System.Text;
using ZXing;

Console.WriteLine("======");
Console.WriteLine("======\b \b");
Console.WriteLine("======");

System.Console.Out.WriteQRCodeLine
            (
                  data  : $@"AwesomeYuer于斯人也@Microshaoft
                             AwesomeYuer于斯人也@Microshaoft"        //待生成二维码原始数据
                    
                , width                 : 10                        //二维码图像生成宽度
                , height                : 10                        //二维码图像生成高度
                , margin                : 1                         //二维码图像生成边缘空白宽高度

                , characterSet          : nameof(Encoding.UTF8)     //二维码字符集

                , darkColor             : ConsoleColor.White        //控制台二维码输出深颜色
                , lightColor            : ConsoleColor.Red          //控制台二维码输出浅颜色

                , placeholderChar       : '囍'                      //控制台二维码输出占位符 同时支持窄宽字符: !@# ㊚㊛囍♀♂♂♀☿♁⚢⚣⚤⚥⚦⚧⚨

                , outputPostionLeft     : 10                        //控制台二维码输出横向位置
                , outputPostionTop      : null!                     //控制台二维码输出纵向位置
            );

Console.Out.WriteQRCodeLine
                (
                    data                    : "https://www.cnblogs.com/stulzq/p/14282461.html?Thanks"
                    , placeholderChar       : '♂' //㊚㊛囍♀♂♂♀☿♁⚢⚣⚤⚥⚦⚧⚨
                    , outputPostionLeft     : 100
                    , outputPostionTop      : 10
                );

Console.WriteLine();
Console.WriteLine();
Console.WriteLine("㊚㊛囍♀♂♂♀☿♁⚢⚣⚤⚥⚦⚧⚨");
Console.WriteLine();
Console.WriteLine();

var s =
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
Console.Out.WriteQRCodeLine
            (
                s
                , new Dictionary<EncodeHintType, object> ()
                    {
                          { EncodeHintType.CHARACTER_SET            , nameof(Encoding.UTF8)                 }
                        , { EncodeHintType.ERROR_CORRECTION         , "L"                                   }
                        , { EncodeHintType.QR_COMPACT               , true                                  }
                        , { EncodeHintType.PURE_BARCODE             , true                                  }
                        //, { EncodeHintType.QR_VERSION               , 10                                    }
                        //, { EncodeHintType.DISABLE_ECI              , true                                  }
                        //, { EncodeHintType.GS1_FORMAT               , true                                  }
                        , { EncodeHintType.MARGIN                   , 1                                     }
                        //, { EncodeHintType.WIDTH                    , width                                 }
                        //, { EncodeHintType.HEIGHT                   , height                                }
                    }
                , placeholderChar           : '$' //窄字符
                , outputPostionLeft         : 20
                , darkColor                 : ConsoleColor.Yellow
                , lightColor                : ConsoleColor.DarkBlue
            );


Console.Out.WriteQRCodeLine
            (
                $@"AwesomeYuer于斯人也" //待生成二维码原始数据
            );