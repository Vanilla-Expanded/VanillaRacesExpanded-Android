using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_RoyaltyTracker), "CanRequireBedroom")]
    public static class Pawn_RoyaltyTracker_CanRequireBedroom_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn_RoyaltyTracker __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
