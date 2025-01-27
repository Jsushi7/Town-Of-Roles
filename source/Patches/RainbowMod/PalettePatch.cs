﻿using UnityEngine;

namespace TownOfSushi.RainbowMod
{
    public static class PalettePatch
    {
        public static void Load()
        {
            Palette.ColorNames = new[]
            {
                StringNames.ColorRed,
                StringNames.ColorBlue,
                StringNames.ColorGreen,
                StringNames.ColorPink,
                StringNames.ColorOrange,
                StringNames.ColorYellow,
                StringNames.ColorBlack,
                StringNames.ColorWhite,
                StringNames.ColorPurple,
                StringNames.ColorBrown,
                StringNames.ColorCyan,
                StringNames.ColorLime,
                StringNames.ColorMaroon,
                StringNames.ColorRose,
                StringNames.ColorBanana,
                StringNames.ColorGray,
                StringNames.ColorTan,
                StringNames.ColorCoral,
                // New colours
                (StringNames)999983,//"Watermelon",
                (StringNames)999984,//"Chocolate",
                (StringNames)999985,//"Sky Blue",
                (StringNames)999986,//"Beige",
                (StringNames)999987,//"Magenta",
                (StringNames)999988,//"Turquoise",
                (StringNames)999989,//"Lilac",
                (StringNames)999990,//"Olive",
                (StringNames)999991,//"Azure",
                (StringNames)999992,//"Plum",
                (StringNames)999993,//"Jungle",
                (StringNames)999994,//"Mint",
                (StringNames)999995,//"Chartreuse",
                (StringNames)999996,//"Macau",
                (StringNames)999997,//"Gold",
                (StringNames)999998,//"Tawny",
                (StringNames)999999,//"Rainbow",
                //TOR Colors
                (StringNames)000001,//"Sloth",              
                (StringNames)000002,//"Northie :)",   
                (StringNames)000003,//"Darkness",     
                (StringNames)000004,//"Juggernaut", 
                (StringNames)000005,//"RaLu",     
                (StringNames)000006,//"Diddly",      
                (StringNames)000007,//"Hannah",   
                (StringNames)000008,//"RufusZeno",           
                (StringNames)000009,//"Veteran",             
                (StringNames)000010,//"Lotty",  
                (StringNames)000011,//"Snax", 
                (StringNames)000012,//"GGamer",        
                (StringNames)000013,//"Blackberry",                                                                                                                                           
            };
            Palette.PlayerColors = new[]
            {
                new Color32(198, 17, 17, byte.MaxValue),
                new Color32(19, 46, 210, byte.MaxValue),
                new Color32(17, 128, 45, byte.MaxValue),
                new Color32(238, 84, 187, byte.MaxValue),
                new Color32(240, 125, 13, byte.MaxValue),
                new Color32(246, 246, 87, byte.MaxValue),
                new Color32(63, 71, 78, byte.MaxValue),
                new Color32(215, 225, 241, byte.MaxValue),
                new Color32(107, 47, 188, byte.MaxValue),
                new Color32(113, 73, 30, byte.MaxValue),
                new Color32(56, byte.MaxValue, 221, byte.MaxValue),
                new Color32(80, 240, 57, byte.MaxValue),
                Palette.FromHex(6233390),
                Palette.FromHex(15515859),
                Palette.FromHex(15787944),
                Palette.FromHex(7701907),
                Palette.FromHex(9537655),
                Palette.FromHex(14115940),
                // New colours
                new Color32(168, 50, 62, byte.MaxValue),
                new Color32(60, 48, 44, byte.MaxValue),
                new Color32(61, 129, 255, byte.MaxValue),
                new Color32(240, 211, 165, byte.MaxValue),
                new Color32(255, 0, 127, byte.MaxValue),
                new Color32(61, 255, 181, byte.MaxValue),
                new Color32(186, 161, 255, byte.MaxValue),
                new Color32(97, 114, 24, byte.MaxValue),
                new Color32(1, 166, 255, byte.MaxValue),
                new Color32(79, 0, 127, byte.MaxValue),
                new Color32(0, 47, 0, byte.MaxValue),
                new Color32(151, 255, 151, byte.MaxValue),
                new Color32(207, 255, 0, byte.MaxValue),
                new Color32(0, 97, 93, byte.MaxValue),
                new Color32(205, 63, 0, byte.MaxValue),
                new Color32(255, 207, 0, byte.MaxValue),
                new Color32(0, 0, 0, byte.MaxValue),
                //TOR Colors
                new Color32(120, 82, 169, byte.MaxValue),      
                new Color32(50, 125, 105, byte.MaxValue),    
                new Color32(40, 4, 66, byte.MaxValue),              
                new Color32(133, 30, 82, byte.MaxValue),         
                new Color32(235, 192, 192, byte.MaxValue),      
                new Color32(119, 40, 247, byte.MaxValue),       
                new Color32(255, 0, 62, byte.MaxValue),     
                new Color32(123, 92, 0, byte.MaxValue),             
                new Color32(153, 128, 64, byte.MaxValue),          
                new Color32(63, 77, 60, byte.MaxValue),  
                new Color32(221, 87, 28, byte.MaxValue),        
                new Color32(0, 49, 83, byte.MaxValue),  
                new Color32(24, 53, 158, byte.MaxValue),                                                                                                                             
            };
            Palette.ShadowColors = new[]
            {
                new Color32(122, 8, 56, byte.MaxValue),
                new Color32(9, 21, 142, byte.MaxValue),
                new Color32(10, 77, 46, byte.MaxValue),
                new Color32(172, 43, 174, byte.MaxValue),
                new Color32(180, 62, 21, byte.MaxValue),
                new Color32(195, 136, 34, byte.MaxValue),
                new Color32(30, 31, 38, byte.MaxValue),
                new Color32(132, 149, 192, byte.MaxValue),
                new Color32(59, 23, 124, byte.MaxValue),
                new Color32(94, 38, 21, byte.MaxValue),
                new Color32(36, 169, 191, byte.MaxValue),
                new Color32(21, 168, 66, byte.MaxValue),
                Palette.FromHex(4263706),
                Palette.FromHex(14586547),
                Palette.FromHex(13810825),
                Palette.FromHex(4609636),
                Palette.FromHex(5325118),
                Palette.FromHex(11813730),
                // New colours
                new Color32(101, 30, 37, byte.MaxValue),
                new Color32(30, 24, 22, byte.MaxValue),
                new Color32(31, 65, 128, byte.MaxValue),
                new Color32(120, 106, 83, byte.MaxValue),
                new Color32(191, 0, 95, byte.MaxValue),
                new Color32(31, 128, 91, byte.MaxValue),
                new Color32(93, 81, 128, byte.MaxValue),
                new Color32(66, 91, 15, byte.MaxValue),
                new Color32(17, 104, 151, byte.MaxValue),
                new Color32(55, 0, 95, byte.MaxValue),
                new Color32(0, 23, 0, byte.MaxValue),
                new Color32(109, 191, 109, byte.MaxValue),
                new Color32(143, 191, 61, byte.MaxValue),
                new Color32(0, 65, 61, byte.MaxValue),
                new Color32(141, 31, 0, byte.MaxValue),
                new Color32(191, 143, 0, byte.MaxValue),
                new Color32(0, 0, 0, byte.MaxValue),
                //TROS Colors
                new Color32(93, 64, 130, byte.MaxValue),      
                new Color32(37, 94, 79, byte.MaxValue),     
                new Color32(26, 3, 43, byte.MaxValue),                
                new Color32(99, 22, 61, byte.MaxValue),            
                new Color32(186, 149, 149, byte.MaxValue),   
                new Color32(96, 32, 199, byte.MaxValue),    
                new Color32(184, 0, 45, byte.MaxValue),             
                new Color32(92, 69, 1, byte.MaxValue),  
                new Color32(117, 98, 48, byte.MaxValue), 
                new Color32(46, 56, 44, byte.MaxValue),     
                new Color32(186, 71, 20, byte.MaxValue),   
                new Color32(1, 38, 64, byte.MaxValue),       
                new Color32(19, 41, 128, byte.MaxValue),                                                                                                                                            
            };
        }
    }
}