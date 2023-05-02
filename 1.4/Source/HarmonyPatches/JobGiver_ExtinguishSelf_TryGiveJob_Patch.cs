using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThinkNode_ConditionalBurning), "Satisfied")]
    public static class ThinkNode_ConditionalBurning_Satisfied_Patch
    {
        private static bool Prefix(Pawn pawn)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_FireVulnerability) is false && pawn.Drafted)
                return false;
            return true;
        }
    }
}
