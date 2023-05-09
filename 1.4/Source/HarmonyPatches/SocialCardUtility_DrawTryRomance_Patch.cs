using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SocialCardUtility), "DrawRelationsAndOpinions")]
    public static class SocialCardUtility_DrawRelationsAndOpinions_Patch
    {
        public static bool Prefix(Pawn selPawnForSocialInfo)
        {
            if (selPawnForSocialInfo.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled) && !selPawnForSocialInfo.HasActiveGene(VREA_DefOf.VREA_EmotionSimulators))
            {
                return false;
            }
            return true;
        }
    }
}
