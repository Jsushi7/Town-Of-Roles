using HarmonyLib;
using TownOfRoles.Extensions;

namespace TownOfRoles
{
    internal static class TaskPatches
    {
        [HarmonyPatch(typeof(GameData), nameof(GameData.RecomputeTaskCounts))]
        private class GameData_RecomputeTaskCounts
        {
            private static bool Prefix(GameData __instance)
            {
                __instance.TotalTasks = 0;
                __instance.CompletedTasks = 0;
                for (var i = 0; i < __instance.AllPlayers.Count; i++)
                {
                    var playerInfo = __instance.AllPlayers.ToArray()[i];
                    if (!playerInfo.Disconnected && playerInfo.Tasks != null && playerInfo.Object &&
                        (GameOptionsManager.Instance.currentNormalGameOptions.GhostsDoTasks || !playerInfo.IsDead) && !playerInfo.IsImpostor() &&
                        !(
                            playerInfo._object.Is(RoleEnum.Jester) || playerInfo._object.Is(RoleEnum.Amnesiac) ||
                         playerInfo._object.Is(RoleEnum.Guardian)  || 
                             playerInfo._object.Is(RoleEnum.Vulture) ||
                            playerInfo._object.Is(RoleEnum.Glitch) || playerInfo._object.Is(RoleEnum.Executioner) ||
                            playerInfo._object.Is(RoleEnum.Pyromaniac) ||playerInfo._object.Is(RoleEnum.SerialKiller) || playerInfo._object.Is(RoleEnum.Juggernaut) ||
                            playerInfo._object.Is(RoleEnum.Plaguebearer) || playerInfo._object.Is(RoleEnum.Pestilence) ||
                            playerInfo._object.Is(RoleEnum.Werewolf) ||
                            playerInfo._object.Is(RoleEnum.Phantom) || playerInfo._object.Is(RoleEnum.Avenger)
                        ))
                        for (var j = 0; j < playerInfo.Tasks.Count; j++)
                        {
                            __instance.TotalTasks++;
                            if (playerInfo.Tasks.ToArray()[j].Complete) __instance.CompletedTasks++;
                        }
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(Console), nameof(Console.CanUse))]
        private class Console_CanUse
        {
            private static bool Prefix(Console __instance, [HarmonyArgument(0)] GameData.PlayerInfo playerInfo, ref float __result)
            {
                var playerControl = playerInfo.Object;

                var flag = playerControl.Is(RoleEnum.Glitch)
                           || playerControl.Is(RoleEnum.Jester)
                           || playerControl.Is(RoleEnum.Executioner)
                           || playerControl.Is(RoleEnum.Juggernaut)
                           || playerControl.Is(RoleEnum.Vulture)
                           || playerControl.Is(RoleEnum.Pyromaniac)                    
                           || playerControl.Is(RoleEnum.SerialKiller)                           
                           || playerControl.Is(RoleEnum.Plaguebearer)
                           || playerControl.Is(RoleEnum.Pestilence)
                           || playerControl.Is(RoleEnum.Werewolf)
                           
                           || (
                                !CustomGameOptions.BegninNeutralHasTasks &&
                                (
                                    playerControl.Is(RoleEnum.Amnesiac) ||
                                    playerControl.Is(RoleEnum.Guardian)
                                )
                            );
                // If the console is not a sabotage repair console
                if (flag && !__instance.AllowImpostor)
                {
                    __result = float.MaxValue;
                    return false;
                }

                return true;
            }
        }
    }
}