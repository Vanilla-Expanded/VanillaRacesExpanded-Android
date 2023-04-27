using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "BindDirectNeedFields")]
    public static class Pawn_NeedsTracker_BindDirectNeedFields_Patch
    {
        public static void Postfix(Pawn_NeedsTracker __instance)
        {
            if (__instance.pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __instance.food = __instance.TryGetNeed<Need_FoodSuppressed>();
            }
        }
    }
}
