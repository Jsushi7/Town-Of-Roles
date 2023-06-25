using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HarmonyLib;
using Reactor.Utilities.Extensions;
using TownOfRoles.CustomOption;
using AmongUs.GameOptions;
using System.Linq;
using UnityEngine;
using TownOfRoles.Classes;

namespace TownOfRoles
{
    [HarmonyPatch]
    public static class GameSettings
    {
        public static int SettingsPage = -1;

        [HarmonyPatch(typeof(IGameOptionsExtensions), nameof(IGameOptionsExtensions.ToHudString))]
        private static class GameOptionsDataPatch
        {
            public static IEnumerable<MethodBase> TargetMethods()
            {
                return typeof(GameOptionsData).GetMethods(typeof(string), typeof(int));
            }

            private static void Postfix(ref string __result)
            {
                if (GameOptionsManager.Instance.CurrentGameOptions.GameMode == GameModes.HideNSeek) return;

                var builder = new StringBuilder();
                builder.AppendLine($"<color=#FF0000FF>Use Scroll Wheel If Necessary</color>");                
                builder.AppendLine("Press Tab To Change Page");
                if (SettingsPage == 0) builder.AppendLine("Page 2: General Mod Settings");
                else if (SettingsPage == 1) builder.AppendLine("Page 3: <color=#26ffff>Crewmate</color> Settings");
                else if (SettingsPage == 2) builder.AppendLine("Page 4: <color=#80797c>Neutral</color> Settings");
                else if (SettingsPage == 3) builder.AppendLine("Page 5: <color=#FF0000FF>Impostor</color> Settings");
                else if (SettingsPage == 4) builder.AppendLine("Page 6: <color=#9cbee4>Modifier</color> Settings");

                if (SettingsPage == -1) builder.Append(new StringBuilder(__result));

                else
                {
                    foreach (var option in CustomOption.CustomOption.AllOptions.Where(x => x.Menu == (MultiMenu)SettingsPage))
                    {
                        if (option.Type == CustomOptionType.Button)
                            continue;
                        if (option == Generate.MaxPlayers) continue;

                        if (option.Type == CustomOptionType.Header)
                            builder.AppendLine($"\n{option.Name}");
                        else
                            builder.AppendLine($"    {option.Name}: {option}");
                    }
                }

                __result = builder.ToString();
                __result = $"<size=1.30>{__result}</size>";
            }
        }

        [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Update))]
        public static class Update
        {
            public static void Postfix(ref GameOptionsMenu __instance)
            {
                __instance.GetComponentInParent<Scroller>().ContentYBounds.max = (__instance.Children.Length - 6.5f) / 2;
            }
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public class LobbyPatch
        {
            public static void Postfix(HudManager __instance)
            {   
                if (!GameStates.IsLobby)
                    return;

                __instance.ReportButton.gameObject.SetActive(false);

                if (!GameStates.IsLobby)
                    return;

                __instance.ImpostorVentButton.gameObject.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (SettingsPage > 3)
                        SettingsPage = -1;
                    else
                        SettingsPage++;
                }
            }
        }
    }
}