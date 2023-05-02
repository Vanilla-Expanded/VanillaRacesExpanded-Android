using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "RandomSelectionWeight")]
    public static class InteractionWorker_RomanceAttempt_RandomSelectionWeight_Patch
    {
        public static void Postfix(ref float __result, Pawn initiator, Pawn recipient)
        {
            if (initiator.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled) && !initiator.HasActiveGene(VREA_DefOf.VREA_EmotionSimulators))
            {
                __result = 0f;
            }
            else if (recipient.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled) && !recipient.HasActiveGene(VREA_DefOf.VREA_EmotionSimulators))
            {
                __result = 0f;
            }
        }
    }
}
