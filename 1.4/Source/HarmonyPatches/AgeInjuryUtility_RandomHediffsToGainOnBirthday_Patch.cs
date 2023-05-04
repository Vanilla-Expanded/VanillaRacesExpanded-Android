using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(AgeInjuryUtility), "RandomHediffsToGainOnBirthday", new Type[] { typeof(Pawn), typeof(float)})]
    public static class AgeInjuryUtility_RandomHediffsToGainOnBirthday_Patch
    {
        public static IEnumerable<HediffGiver_Birthday> Postfix(IEnumerable<HediffGiver_Birthday> __result, Pawn pawn, float age)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                yield break;
            }
            else
            {
                foreach (var r in __result)
                {
                    yield return r;
                }
            }
        }
    }
}
