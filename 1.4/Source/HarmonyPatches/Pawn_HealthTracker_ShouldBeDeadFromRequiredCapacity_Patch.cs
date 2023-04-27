using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDeadFromRequiredCapacity")]
    public static class Pawn_HealthTracker_ShouldBeDeadFromRequiredCapacity_Patch
    {
        public static void Postfix(ref PawnCapacityDef __result, Pawn ___pawn)
        {
            if (___pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __result = null;
            }
        }
    }
}
