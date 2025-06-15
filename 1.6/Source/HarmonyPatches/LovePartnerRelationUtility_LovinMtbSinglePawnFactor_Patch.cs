using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(LovePartnerRelationUtility), "LovinMtbSinglePawnFactor")]
    public static class LovePartnerRelationUtility_LovinMtbSinglePawnFactor_Patch
    {
        public static bool Prefix(Pawn pawn, ref float __result)
        {
            if (pawn.IsAndroid())
            {
                float num = 1f;
                num /= 1f - pawn.health.hediffSet.PainTotal;
                float level = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness);
                if (level < 0.5f)
                {
                    num /= level * 2f;
                }
                __result = num;
                return false;
            }
            return true;
        }
    }
}
