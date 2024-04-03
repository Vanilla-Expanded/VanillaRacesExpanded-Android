using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SocialCardUtility), "DrawRelationsAndOpinions")]
    public static class SocialCardUtility_DrawRelationsAndOpinions_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn selPawnForSocialInfo)
        {
            if (selPawnForSocialInfo.Emotionless())
            {
                return false;
            }
            return true;
        }
    }
}
