﻿using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;

namespace TownOfRoles.ColorsMod
{
    [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString),
        new[] { typeof(StringNames), typeof(Il2CppReferenceArray<Il2CppSystem.Object>) })]
    public class PatchColours
    {
        public static bool Prefix(ref string __result, [HarmonyArgument(0)] StringNames name)
        {
            var newResult = (int)name switch
            {
                999983 => "Watermelon",
                999984 => "Chocolate",
                999985 => "Sky Blue",
                999986 => "Beige",
                999987 => "Magenta",
                999988 => "Turquoise",
                999989 => "Lilac",
                999990 => "Olive",
                999991 => "Azure",
                999992 => "Plum",
                999993 => "Jungle",
                999994 => "Mint",
                999995 => "Chartreuse",
                999996 => "Macau",
                999997 => "Tawny",
                999998 => "Gold",
                999999 => "Rainbow",
                222222 => "Ice",
                222221 => "Sunrise",
                222223 => "Northie",
                222224 => "RaLu",
                222225 => "Fizz",
                222226 => "GGamer",
                222227 => "Snax",
                222228 => "Lotty",
                222229 => "Bordeaux",
                222210 => "Peach",
                222211 => "Signal-Orange",


                _ => null
            };
            if (newResult != null)
            {
                __result = newResult;
                return false;
            }

            return true;
        }
    }
}
