using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RelationsUtility), "RomanceEligible")]
    public static class RelationsUtility_RomanceEligible_Patch
    {
        public static void Postfix(Pawn pawn, ref AcceptanceReport __result)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled) && !pawn.HasActiveGene(VREA_DefOf.VREA_EmotionSimulators))
            {
                __result = false;
            }
        }
    }
}
