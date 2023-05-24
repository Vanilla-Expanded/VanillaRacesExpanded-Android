using HarmonyLib;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffSet), "AddDirect")]
    public static class HediffSet_AddDirect_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(HediffSet __instance, Pawn ___pawn, Hediff hediff)
        {
            if (___pawn.IsAndroid())
            {
                return Pawn_HealthTracker_AddHediff_Patch.HandleHediffForAndroid(___pawn, ref hediff);
            }
            return true;
        }
    }
}
