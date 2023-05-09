using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InteractionUtility), "CanInitiateInteraction")]
    public static class InteractionUtility_CanInitiateInteraction_Patch
    {
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled) && !pawn.HasActiveGene(VREA_DefOf.VREA_EmotionSimulators))
            {
                __result = false;
            }
        }
    }
}
