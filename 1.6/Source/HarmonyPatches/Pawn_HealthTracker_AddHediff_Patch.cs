using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "AddHediff", new Type[]
    {
        typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult)
    })]
    public static class Pawn_HealthTracker_AddHediff_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(Pawn_HealthTracker __instance, Pawn ___pawn, ref Hediff hediff, BodyPartRecord part = null, DamageInfo? dinfo = null, DamageWorker.DamageResult result = null)
        {
            if (___pawn.IsAndroid())
            {
                return HandleHediffForAndroid(___pawn, ref hediff);
            }
            return true;
        }

        public static bool HandleHediffForAndroid(Pawn ___pawn, ref Hediff hediff)
        {
            if (ModCompatibility.MSE2Active)
            {
                foreach (var androidImplant in Utils.cachedCounterParts.Values.ToList())
                {
                    var extension = androidImplant.modExtensions?.FirstOrDefault(x => x.GetType() == ModCompatibility.ignoreSubPartsExtensionType);
                    if (extension != null)
                    {
                        androidImplant.modExtensions.Remove(extension);
                    }
                }
            }

            if (___pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticImmunity) && Utils.AndroidCanCatch(hediff.def) is false)
            {
                return false;
            }
            if (hediff is Hediff_MissingPart && hediff.Part != null && !___pawn.health.hediffSet.GetNotMissingParts().Contains(hediff.Part))
            {
                return false;
            }
            if (hediff.def == HediffDefOf.Hypothermia)
            {
                var newHediff = HediffMaker.MakeHediff(VREA_DefOf.VREA_Freezing, ___pawn, hediff.part);
                newHediff.Severity = hediff.Severity;
                hediff = newHediff;
            }
            else if (hediff.def == HediffDefOf.Heatstroke)
            {
                var newHediff = HediffMaker.MakeHediff(VREA_DefOf.VREA_Overheating, ___pawn, hediff.part);
                newHediff.Severity = hediff.Severity;
                hediff = newHediff;
            }
            return true;
        }
    }
}
