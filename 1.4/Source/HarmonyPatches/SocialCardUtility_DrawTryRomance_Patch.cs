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
            if (selPawnForSocialInfo.Emotionless())
            {
                return false;
            }
            return true;
        }
    }
}
