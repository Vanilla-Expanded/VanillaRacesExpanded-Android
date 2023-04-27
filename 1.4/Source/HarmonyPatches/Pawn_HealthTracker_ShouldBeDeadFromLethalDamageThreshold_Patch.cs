using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDead")]
    public static class Pawn_HealthTracker_ShouldBeDead_Patch
    {
        public static void Postfix(ref bool __result, Pawn ___pawn)
        {
            if (__result && ___pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                if (___pawn.health.hediffSet.GetBrain() != null)
                {
                    __result = false;
                }
            }
        }
    }
}
