using HarmonyLib;
using TownOfSushi.Roles;

namespace TownOfSushi.NeutralRoles.PhantomMod
{
    [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.HandleAnimation))]
    public class HandleAnimation
    {
        public static void Prefix(PlayerPhysics __instance, [HarmonyArgument(0)] ref bool amDead)
        {
            if (__instance.myPlayer.Is(RoleEnum.Phantom)) amDead = Role.GetRole<Phantom>(__instance.myPlayer).Caught;
        }
    }
}