using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(JobGiver_GetHemogen), "CanFeedOnPrisoner")]
    public static class JobGiver_GetHemogen_CanFeedOnPrisoner_Patch
    {
        public static void Postfix(Pawn bloodfeeder, Pawn prisoner, ref AcceptanceReport __result)
        {
            if (prisoner.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __result = "VREA.CannotFeedOnAndroid".Translate(prisoner.Named("PAWN"));
            }
        }
    }
}
