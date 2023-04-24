using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffGiver_Bleeding), "OnIntervalPassed")]
    public static class HediffGiver_Bleeding_OnIntervalPassed_Patch
    {
        public static bool Prefix(Pawn pawn, Hediff cause)
        {
            if (pawn.IsAndroid())
            {
                HediffSet hediffSet = pawn.health.hediffSet;
                if (hediffSet.BleedRateTotal >= 0)
                {
                    HealthUtility.AdjustSeverity(pawn, VREA_DefOf.VREA_NeutroLoss, hediffSet.BleedRateTotal * 0.001f);
                }
                return false;
            }
            return true;
        }
    }
}
