using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    public static class Pawn_NeedsTracker_ShouldHaveNeed_Patch
    {
        public static bool Prefix(Pawn ___pawn, NeedDef nd, ref bool __result)
        {
            if (___pawn.IsAndroid())
            {
                if (VREA_DefOf.VREA_AndroidSettings.excludedNeedsForAndroids.Contains(nd.defName))
                {
                    return false;
                }
                else if (VREA_DefOf.VREA_AndroidSettings.androidExclusiveNeeds.Contains(nd.defName))
                {
                    __result = true;
                    return false;
                }
            }
            else
            {
                if (VREA_DefOf.VREA_AndroidSettings.androidExclusiveNeeds.Contains(nd.defName))
                {
                    return false;
                }
                if (nd == VREA_DefOf.VREA_MemorySpace || nd == VREA_DefOf.VREA_ReactorPower)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
