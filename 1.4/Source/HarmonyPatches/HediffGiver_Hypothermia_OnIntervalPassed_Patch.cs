using HarmonyLib;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffGiver_Hypothermia), "OnIntervalPassed")]
    public static class HediffGiver_Hypothermia_OnIntervalPassed_Patch
    {
        public static bool Prefix(Pawn pawn, Hediff cause)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_ComponentFreezing))
            {
                OnIntervalPassed(pawn, cause);
                return false;
            }
            return true;
        }

        public static void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            float ambientTemperature = pawn.AmbientTemperature;
            FloatRange floatRange = pawn.ComfortableTemperatureRange();
            FloatRange floatRange2 = pawn.SafeTemperatureRange();
            HediffSet hediffSet = pawn.health.hediffSet;
            HediffDef hediffDef = VREA_DefOf.VREA_Freezing;
            Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(hediffDef);
            if (ambientTemperature < floatRange2.min)
            {
                float a = Mathf.Abs(ambientTemperature - floatRange2.min) * 6.45E-05f;
                a = Mathf.Max(a, 0.00075f);
                a *= 2f;
                HealthUtility.AdjustSeverity(pawn, hediffDef, a);
                if (pawn.Dead)
                {
                    return;
                }
            }
            if (firstHediffOfDef == null)
            {
                return;
            }
            if (ambientTemperature > floatRange.min)
            {
                float value = firstHediffOfDef.Severity * 0.027f;
                value = Mathf.Clamp(value, 0.0015f, 0.015f);
                firstHediffOfDef.Severity -= value;
            }
            else if (ambientTemperature < 0f && firstHediffOfDef.Severity >= 0.35f)
            {
                if (pawn.RaceProps.body.AllParts.Where((BodyPartRecord x) => !hediffSet.PartIsMissing(x) && x.coverageAbs > 0)
                    .TryRandomElement(out var result))
                {
                    DamageInfo dinfo = new DamageInfo(VREA_DefOf.VREA_Freeze, 1f, 0f, -1f, null, result);
                    pawn.TakeDamage(dinfo);
                }
            }
        }
    }
}
