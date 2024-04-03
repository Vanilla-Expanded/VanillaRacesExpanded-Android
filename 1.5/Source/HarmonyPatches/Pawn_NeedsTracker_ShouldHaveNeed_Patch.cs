using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_NeedsTracker), "ShouldHaveNeed")]
    public static class Pawn_NeedsTracker_ShouldHaveNeed_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn ___pawn, NeedDef nd, ref bool __result)
        {
            if (___pawn.IsAndroid())
            {
                if (VREA_DefOf.VREA_AndroidSettings.excludedNeedsForAndroids.Contains(nd.defName))
                {
                    __result = false;
                }
                else if (VREA_DefOf.VREA_AndroidSettings.androidExclusiveNeeds.Contains(nd.defName))
                {
                    __result = true;
                }
            }
            else
            {
                if (VREA_DefOf.VREA_AndroidSettings.androidExclusiveNeeds.Contains(nd.defName))
                {
                    __result = false;
                }
                if (nd == VREA_DefOf.VREA_MemorySpace || nd == VREA_DefOf.VREA_ReactorPower)
                {
                    __result = false;
                }
            }
        }
    }
}
