using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffGiver_Heat), "OnIntervalPassed")]
    public static class HediffGiver_Heat_OnIntervalPassed_Patch
    {
        public static bool Prefix(Pawn pawn, Hediff cause)
        {
            if (pawn.IsAndroid())
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
            Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_Overheating);
            if (ambientTemperature > floatRange2.max)
            {
                float x = ambientTemperature - floatRange2.max;
                x = HediffGiver_Heat.TemperatureOverageAdjustmentCurve.Evaluate(x);
                float a = x * 6.45E-05f;
                a = Mathf.Max(a, 0.000375f);
                HealthUtility.AdjustSeverity(pawn, VREA_DefOf.VREA_Overheating, a);
            }
            else if (firstHediffOfDef != null && ambientTemperature < floatRange.max)
            {
                float value = firstHediffOfDef.Severity * 0.027f;
                value = Mathf.Clamp(value, 0.0015f, 0.015f);
                firstHediffOfDef.Severity -= value;
            }
        }
    }
}
