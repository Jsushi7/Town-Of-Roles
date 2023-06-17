﻿using System.Linq;
using Hazel;
using TownOfRoles.Roles;
using UnityEngine;
using UnityEngine.UI;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using TownOfRoles.CrewmateRoles.MedicMod;
using TownOfRoles.CrewmateRoles.SwapperMod;
using TownOfRoles.ImpostorRoles.SilencerMod;
using TownOfRoles.Roles.Modifiers;
using TownOfRoles.Extensions;
using TownOfRoles.CrewmateRoles.ImitatorMod;
using RewiredConsts;

namespace TownOfRoles.Modifiers.AssassinMod
{
    public class AssassinKill
    {
        public static void RpcMurderPlayer(PlayerControl player, PlayerControl assassin)
        {
            PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(
                x => x.TargetPlayerId == player.PlayerId
            );
            RpcMurderPlayer(voteArea, player, assassin);
        }
        public static void RpcMurderPlayer(PlayerVoteArea voteArea, PlayerControl player, PlayerControl assassin)
        {
            MurderPlayer(voteArea, player);
            AssassinKillCount(player, assassin);
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                (byte)CustomRPC.AssassinKill, SendOption.Reliable, -1);
            writer.Write(player.PlayerId);
            writer.Write(assassin.PlayerId);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }

        public static void MurderPlayer(PlayerControl player, bool checkLover = true)
        {
            PlayerVoteArea voteArea = MeetingHud.Instance.playerStates.First(
                x => x.TargetPlayerId == player.PlayerId
            );
            MurderPlayer(voteArea, player, checkLover);
        }
        public static void AssassinKillCount(PlayerControl player, PlayerControl assassin)
        {
            var assassinPlayer = Role.GetRole(assassin);
            if (player == assassin) assassinPlayer.IncorrectAssassinKills += 1;
            else assassinPlayer.CorrectAssassinKills += 1;
        }    
        public static void MurderPlayer(
            PlayerVoteArea voteArea,
            PlayerControl player,
            bool checkLover = true
        )
        {
            var hudManager = DestroyableSingleton<HudManager>.Instance;
            if (checkLover)
            {
            //DestroyableSingleton<HudManager>.Instance.Chat.AddChat(PlayerControl.LocalPlayer, "The Assassin shot" + ((Object) player).name + "!");                
                SoundManager.Instance.PlaySound(player.KillSfx, false, 0.8f);
                //hudManager.KillOverlay.ShowKillAnimation(player.Data, player.Data);
            }
            var amOwner = player.AmOwner;
            if (amOwner)
            {
                Utils.ShowDeadBodies = true;
                hudManager.ShadowQuad.gameObject.SetActive(false);
                player.nameText().GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                player.RpcSetScanner(false);
                ImportantTextTask importantTextTask = new GameObject("_Player").AddComponent<ImportantTextTask>();
                importantTextTask.transform.SetParent(AmongUsClient.Instance.transform, false);
                if (!GameOptionsManager.Instance.currentNormalGameOptions.GhostsDoTasks)
                {
                    for (int i = 0;i < player.myTasks.Count;i++)
                    {
                        PlayerTask playerTask = player.myTasks.ToArray()[i];
                        playerTask.OnRemove();
                        Object.Destroy(playerTask.gameObject);
                    }

                    player.myTasks.Clear();
                    importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                        StringNames.GhostIgnoreTasks,
                        new Il2CppReferenceArray<Il2CppSystem.Object>(0)
                    );
                }
                else
                {
                    importantTextTask.Text = DestroyableSingleton<TranslationController>.Instance.GetString(
                        StringNames.GhostDoTasks,
                        new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                }

                player.myTasks.Insert(0, importantTextTask);

                if (player.Is(RoleEnum.Swapper))
                {
                    var swapper = Role.GetRole<Swapper>(PlayerControl.LocalPlayer);
                    swapper.ListOfActives.Clear();
                    swapper.Buttons.Clear();
                    SwapVotes.Swap1 = null;
                    SwapVotes.Swap2 = null;
                    var buttons = Role.GetRole<Swapper>(player).Buttons;
                    foreach (var button in buttons)
                    {
                        button.SetActive(false);
                        button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                    }
                }

                if (player.Is(RoleEnum.Imitator))
                {
                    var imitator = Role.GetRole<Imitator>(PlayerControl.LocalPlayer);
                    imitator.ListOfActives.Clear();
                    imitator.Buttons.Clear();
                    SetImitate.Imitate = null;
                    var buttons = Role.GetRole<Imitator>(player).Buttons;
                    foreach (var button in buttons)
                    {
                        button.SetActive(false);
                        button.GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();
                    }
                }

                var player2 = PlayerControl.LocalPlayer;

                var guess = Role.GetRole(player);

            if ((player2.GetFaction() == PlayerControl.LocalPlayer.GetFaction() && (player2.GetFaction() is Faction.Impostors or Faction.Neutral)) ||
                CustomGameOptions.DeadSnitcholes)
            {
                if (player2 != player)
                    hudManager.Chat.AddChat(PlayerControl.LocalPlayer, $"{player2.name} Guessed the Role {guess.Name} for {player.name}!");
            }
            
                if (player.Is(AbilityEnum.Assassin))
                {
                    var assassin = Ability.GetAbility<Assassin>(PlayerControl.LocalPlayer);
                    ShowHideButtons.HideButtons(assassin);
                }
            }
            player.Die(DeathReason.Kill, false);

            var meetingHud = MeetingHud.Instance;
            if (amOwner)
            {
                meetingHud.SetForegroundForDead();
            }
            var deadPlayer = new DeadPlayer
            {
                PlayerId = player.PlayerId,
                KillerId = player.PlayerId,
                KillTime = System.DateTime.UtcNow,
            };

            Murder.KilledPlayers.Add(deadPlayer);
            if (voteArea == null) return;
            if (voteArea.DidVote) voteArea.UnsetVote();
            voteArea.AmDead = true;
            voteArea.Overlay.gameObject.SetActive(true);
            voteArea.Overlay.color = Color.white;
            voteArea.XMark.gameObject.SetActive(true);
            voteArea.XMark.transform.localScale = Vector3.one;

            var Silencers = Role.AllRoles.Where(x => x.RoleType == RoleEnum.Silencer && x.Player != null).Cast<Silencer>();
            foreach (var role in Silencers)
            {
                if (role.Silenced != null && voteArea.TargetPlayerId == role.Silenced.PlayerId)
                {
                    if (SilenceMeetingUpdate.PrevXMark != null && SilenceMeetingUpdate.PrevOverlay != null)
                    {
                        voteArea.XMark.sprite = SilenceMeetingUpdate.PrevXMark;
                        voteArea.Overlay.sprite = SilenceMeetingUpdate.PrevOverlay;
                        voteArea.XMark.transform.localPosition = new Vector3(
                            voteArea.XMark.transform.localPosition.x - SilenceMeetingUpdate.LetterXOffset,
                            voteArea.XMark.transform.localPosition.y - SilenceMeetingUpdate.LetterYOffset,
                            voteArea.XMark.transform.localPosition.z);
                    }
                }
            }

            if (PlayerControl.LocalPlayer.Is(RoleEnum.Swapper) && !player.AmOwner && !PlayerControl.LocalPlayer.Data.IsDead)
            {
                
                SwapVotes.Swap1 = voteArea == SwapVotes.Swap1 ? null : SwapVotes.Swap1;
                SwapVotes.Swap2 = voteArea == SwapVotes.Swap2 ? null : SwapVotes.Swap2;
                if (SwapVotes.Swap1 == null || SwapVotes.Swap2 == null)
                {
                    var writer2 = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                        (byte)CustomRPC.SetSwaps, SendOption.Reliable, -1);
                    writer2.Write(sbyte.MaxValue);
                    writer2.Write(sbyte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(writer2);

                }
                var swapperrole = Role.GetRole<Swapper>(PlayerControl.LocalPlayer);

                int index;
                for (index = 0; index < MeetingHud.Instance.playerStates.Length; index++) if (MeetingHud.Instance.playerStates[index].TargetPlayerId == voteArea.TargetPlayerId) break;

                swapperrole.Buttons[index].GetComponent<SpriteRenderer>().sprite = CrewmateRoles.SwapperMod.AddButton.DisabledSprite;
                swapperrole.ListOfActives[index] = false;
                swapperrole.Buttons[index].SetActive(false);
                swapperrole.Buttons[index].GetComponent<PassiveButton>().OnClick = new Button.ButtonClickedEvent();

            }

            foreach (var playerVoteArea in meetingHud.playerStates)
            {
                if (playerVoteArea.VotedFor != player.PlayerId) continue;
                playerVoteArea.UnsetVote();
                var voteAreaPlayer = Utils.PlayerById(playerVoteArea.TargetPlayerId);
                if (!voteAreaPlayer.AmOwner) continue;
                meetingHud.ClearVote();
            }

            player.Exiled();

            if (AmongUsClient.Instance.AmHost)
            {
                foreach (var role in Role.GetRoles(RoleEnum.Mayor))
                {
                    if (role is Mayor mayor)
                    {
                        if (role.Player == player)
                        {
                            mayor.ExtraVotes.Clear();
                        }
                        else
                        {
                            var votesRegained = mayor.ExtraVotes.RemoveAll(x => x == player.PlayerId);

                            if (mayor.Player == PlayerControl.LocalPlayer)
                                mayor.VoteBank += votesRegained;

                            var writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId,
                                (byte) CustomRPC.AddMayorVoteBank, SendOption.Reliable, -1);
                            writer.Write(mayor.Player.PlayerId);
                            writer.Write(votesRegained);
                            AmongUsClient.Instance.FinishRpcImmediately(writer);
                        }
                    }
                }
                meetingHud.CheckForEndVoting();
            }
        }
    }
}
