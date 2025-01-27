using System.Linq;
using HarmonyLib;
using TownOfSushi.Roles;

namespace TownOfSushi.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CompleteTask))]
    public class CompleteTask
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (!__instance.Is(RoleEnum.Phantom)) return;
            var role = Role.GetRole<Phantom>(__instance);

            var taskinfos = __instance.Data.Tasks.ToArray();

            var tasksLeft = taskinfos.Count(x => !x.Complete);

            if (tasksLeft == 0 && !role.Caught)
            {
                role.CompletedTasks = true;
                if (AmongUsClient.Instance.AmHost)
                {
                    Utils.Rpc(CustomRPC.PhantomWin, role.Player.PlayerId);
                    Utils.EndGame();
                }
            }
        }
    }
}