using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(StatWorker), "ShouldShowFor")]

    public static class StatWorker_ShouldShowFor_Patch
    {
        public static void Postfix(StatWorker __instance, StatRequest req, ref bool __result)
        {
            if (__result && req.Thing is Pawn pawn && pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                if (__instance.stat == StatDefOf.MeatAmount
                    || __instance.stat == StatDefOf.MeditationFocusGain
                    || __instance.stat == StatDefOf.ImmunityGainSpeed
                    || __instance.stat == StatDefOf.MentalBreakThreshold && pawn.HasActiveGene(VREA_DefOf.VREA_MentalBreaksDisabled)
                    || __instance.stat == StatDefOf.Fertility
                    || __instance.stat == StatDefOf.PainShockThreshold && pawn.HasActiveGene(VREA_DefOf.VREA_PainDisabled))
                {
                    __result = false;
                }
            }
        }
    }
}
