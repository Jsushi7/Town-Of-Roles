using HarmonyLib;
using Hazel;
using TownOfSushi.Roles;
using System.Linq;
using TownOfSushi.CrewmateRoles.InformantMod;
using TownOfSushi.Extensions;
using UnityEngine;
using Reactor.Utilities;
using TownOfSushi.Patches;
using AmongUs.GameOptions;
using TownOfSushi.CrewmateRoles.ImitatorMod;
using TownOfSushi.Roles.Modifiers;
using TownOfSushi.CrewmateRoles.AurialMod;
using TownOfSushi.Patches.ScreenEffects;

namespace TownOfSushi.ImpostorRoles.TraitorMod
{
    [HarmonyPatch(typeof(AirshipExileController), nameof(AirshipExileController.WrapUpAndSpawn))]
    public static class AirshipExileController_WrapUpAndSpawn
    {
        public static void Postfix(AirshipExileController __instance) => SetTraitor.ExileControllerPostfix(__instance);
    }

    [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
    public class SetTraitor
    {
        public static PlayerControl WillBeTraitor;
        public static Sprite Sprite => TownOfSushi.Arrow;

        public static void ExileControllerPostfix(ExileController __instance)
        {
            var exiled = __instance.exiled?.Object;
            var alives = PlayerControl.AllPlayerControls.ToArray()
                    .Where(x => !x.Data.IsDead && !x.Data.Disconnected).ToList();
            foreach (var player in alives)
            {
                if (player.Data.IsImpostor() || (player.Is(Faction.NeutralKilling) && CustomGameOptions.NeutralKillingStopsTraitor))
                {
                    return;
                }
            }
            if (PlayerControl.LocalPlayer.Data.IsDead || exiled == PlayerControl.LocalPlayer) return;
            if (alives.Count < CustomGameOptions.LatestSpawn) return;
            if (PlayerControl.LocalPlayer != WillBeTraitor) return;

            if (!PlayerControl.LocalPlayer.Is(RoleEnum.Traitor))
            {
                if (PlayerControl.LocalPlayer.Is(RoleEnum.Informant))
                {
                    var snitchRole = Role.GetRole<Informant>(PlayerControl.LocalPlayer);
                    snitchRole.ImpArrows.DestroyAll();
                    snitchRole.InformantArrows.Values.DestroyAll();
                    snitchRole.InformantArrows.Clear();
                    CompleteTask.Postfix(PlayerControl.LocalPlayer);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Engineer))
                {
                    var engineerRole = Role.GetRole<Engineer>(PlayerControl.LocalPlayer);
                    Object.Destroy(engineerRole.UsesText);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Tracker))
                {
                    var trackerRole = Role.GetRole<Tracker>(PlayerControl.LocalPlayer);
                    trackerRole.TrackerArrows.Values.DestroyAll();
                    trackerRole.TrackerArrows.Clear();
                    Object.Destroy(trackerRole.UsesText);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Transporter))
                {
                    var transporterRole = Role.GetRole<Transporter>(PlayerControl.LocalPlayer);
                    Object.Destroy(transporterRole.UsesText);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Veteran))
                {
                    var veteranRole = Role.GetRole<Veteran>(PlayerControl.LocalPlayer);
                    Object.Destroy(veteranRole.UsesText);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Medium))
                {
                    var medRole = Role.GetRole<Medium>(PlayerControl.LocalPlayer);
                    medRole.MediatedPlayers.Values.DestroyAll();
                    medRole.MediatedPlayers.Clear();
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Trapper))
                {
                    var trapperRole = Role.GetRole<Trapper>(PlayerControl.LocalPlayer);
                    Object.Destroy(trapperRole.UsesText);
                }

                if (PlayerControl.LocalPlayer.Is(RoleEnum.Aurial))
                {
                    var aurial = Role.GetRole<Aurial>(PlayerControl.LocalPlayer);
                    aurial.NormalVision = true;
                    SeeAll.AllToNormal();
                    CameraEffect.singleton.materials.Clear();
                }

                if (PlayerControl.LocalPlayer == StartImitate.ImitatingPlayer) StartImitate.ImitatingPlayer = null;

                var oldRole = Role.GetRole(PlayerControl.LocalPlayer);
                var killsList = (oldRole.CorrectKills, oldRole.IncorrectKills, oldRole.CorrectAssassinKills, oldRole.IncorrectAssassinKills);
                Role.RoleDictionary.Remove(PlayerControl.LocalPlayer.PlayerId);
                var role = new Traitor(PlayerControl.LocalPlayer);
                role.formerRole = oldRole.RoleType;
                role.CorrectKills = killsList.CorrectKills;
                role.IncorrectKills = killsList.IncorrectKills;
                role.CorrectAssassinKills = killsList.CorrectAssassinKills;
                role.IncorrectAssassinKills = killsList.IncorrectAssassinKills;
                role.RegenTask();

                Utils.Rpc(CustomRPC.TraitorSpawn);

                TurnImp(PlayerControl.LocalPlayer);
            }
        }

        public static void TurnImp(PlayerControl player)
        {
            player.Data.Role.TeamType = RoleTeamTypes.Impostor;
            RoleManager.Instance.SetRole(player, RoleTypes.Impostor);
            player.SetKillTimer(GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown);

            System.Console.WriteLine("PROOF I AM IMP VANILLA ROLE: " + player.Data.Role.IsImpostor);

            foreach (var player2 in PlayerControl.AllPlayerControls)
            {
                if (player2.Data.IsImpostor() && PlayerControl.LocalPlayer.Data.IsImpostor())
                {
                    player2.nameText().color = Patches.Colors.Impostor;
                }
            }

            if (CustomGameOptions.TraitorCanAssassin) new Assassin(player);

            if (PlayerControl.LocalPlayer.PlayerId == player.PlayerId)
            {
                DestroyableSingleton<HudManager>.Instance.KillButton.gameObject.SetActive(true);
                Coroutines.Start(Utils.FlashCoroutine(Color.red, 3f));
            }

            foreach (var snitch in Role.GetRoles(RoleEnum.Informant))
            {
                var snitchRole = (Informant)snitch;
                if (snitchRole.TasksDone && PlayerControl.LocalPlayer.Is(RoleEnum.Informant) && CustomGameOptions.InformantSeesTraitor)
                {
                    var gameObj = new GameObject();
                    var arrow = gameObj.AddComponent<ArrowBehaviour>();
                    gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                    var renderer = gameObj.AddComponent<SpriteRenderer>();
                    renderer.sprite = Sprite;
                    arrow.image = renderer;
                    gameObj.layer = 5;
                    snitchRole.InformantArrows.Add(player.PlayerId, arrow);
                }
                else if (snitchRole.Revealed && PlayerControl.LocalPlayer.Is(RoleEnum.Traitor) && CustomGameOptions.InformantSeesTraitor)
                {
                    var gameObj = new GameObject();
                    var arrow = gameObj.AddComponent<ArrowBehaviour>();
                    gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                    var renderer = gameObj.AddComponent<SpriteRenderer>();
                    renderer.sprite = Sprite;
                    arrow.image = renderer;
                    gameObj.layer = 5;
                    snitchRole.ImpArrows.Add(arrow);
                }
            }

            foreach (var haunter in Role.GetRoles(RoleEnum.Avenger))
            {
                var haunterRole = (Avenger)haunter;
                if (haunterRole.Revealed && PlayerControl.LocalPlayer.Is(RoleEnum.Traitor))
                {
                    var gameObj = new GameObject();
                    var arrow = gameObj.AddComponent<ArrowBehaviour>();
                    gameObj.transform.parent = PlayerControl.LocalPlayer.gameObject.transform;
                    var renderer = gameObj.AddComponent<SpriteRenderer>();
                    renderer.sprite = Sprite;
                    arrow.image = renderer;
                    gameObj.layer = 5;
                    haunterRole.ImpArrows.Add(arrow);
                }
            }
        }

        public static void Postfix(ExileController __instance) => ExileControllerPostfix(__instance);

        [HarmonyPatch(typeof(Object), nameof(Object.Destroy), new System.Type[] { typeof(GameObject) })]
        public static void Prefix(GameObject obj)
        {
            if (!SubmergedCompatibility.Loaded || GameOptionsManager.Instance?.currentNormalGameOptions?.MapId != 5) return;
            if (obj.name?.Contains("ExileCutscene") == true) ExileControllerPostfix(ExileControllerPatch.lastExiled);
        }
    }
}