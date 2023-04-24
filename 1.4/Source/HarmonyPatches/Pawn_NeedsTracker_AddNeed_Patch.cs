using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "AddNeed")]
    public static class Pawn_NeedsTracker_AddNeed_Patch
    {
        public static bool Prefix(Pawn ___pawn, NeedDef nd)
        {
            if (___pawn.IsAndroid())
            {
                if (VREA_DefOf.VREA_AndroidSettings.excludedNeedsForAndroids.Contains(nd.defName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
