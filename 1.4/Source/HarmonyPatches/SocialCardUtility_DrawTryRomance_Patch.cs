using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SocialCardUtility), "DrawTryRomance")]
    public static class SocialCardUtility_DrawTryRomance_Patch
    {
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled) && !pawn.HasActiveGene(VREA_DefOf.VREA_EmotionSimulators))
            {
                return false;
            }
            return true;
        }
    }
}
