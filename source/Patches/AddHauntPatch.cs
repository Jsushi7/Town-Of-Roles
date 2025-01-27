using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;
using TownOfSushi.NeutralRoles.PhantomMod;
using TownOfSushi.CrewmateRoles.AvengerMod;

namespace TownOfSushi.Patches
{
    [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
    public static class AirshipAddHauntPatch
    {
        public static void Postfix(AirshipExileController __instance) => AddHauntPatch.ExileControllerPostfix(__instance);
    }

    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    [HarmonyPriority(Priority.First)]
    class AddHauntPatch
    {
        public static Il2CppSystem.Collections.Generic.List<PlayerControl> AssassinatedPlayers = new Il2CppSystem.Collections.Generic.List<PlayerControl>();

        public static void ExileControllerPostfix(ExileController __instance)
        {
            foreach (var player in AssassinatedPlayers)
            {
                try
                {
                    if (SetPhantom.WillBePhantom != player && SetAvenger.WillBeAvenger != player
                        && !player.Data.Disconnected) player.Exiled();
                }
                catch { }
            }
            AssassinatedPlayers.Clear();
        }

        public static void Postfix(ExileController __instance) => ExileControllerPostfix(__instance);

        [HarmonyPatch(typeof(Object), nameof(Object.Destroy), new Type[] { typeof(GameObject) })]
        public static void Prefix(GameObject obj)
        {
            if (!SubmergedCompatibility.Loaded || GameOptionsManager.Instance?.currentNormalGameOptions?.MapId != 5) return;
            if (obj.name?.Contains("ExileCutscene") == true) ExileControllerPostfix(ExileControllerPatch.lastExiled);
        }
    }
}