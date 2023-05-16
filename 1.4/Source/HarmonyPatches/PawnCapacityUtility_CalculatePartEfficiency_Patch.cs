using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using static Verse.PawnCapacityUtility;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnCapacityUtility), "CalculatePartEfficiency")]
    public static class PawnCapacityUtility_CalculatePartEfficiency_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(ref float __result, HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<CapacityImpactor> impactors = null)
        {
            if (diffSet.pawn.IsAndroid())
            {
                __result = CalculatePartEfficiency(diffSet, part, ignoreAddedParts, impactors);
                return false;
            }
            return true;
        }

        public static float CalculatePartEfficiency(HediffSet diffSet, BodyPartRecord part, bool ignoreAddedParts = false, List<CapacityImpactor> impactors = null)
        {
            if (part.parent != null && diffSet.PartIsMissing(part.parent))
            {
                return 0f;
            }
            float num = 1f;
            if (!ignoreAddedParts)
            {
                for (int i = 0; i < diffSet.hediffs.Count; i++)
                {
                    Hediff_AddedPart hediff_AddedPart2 = diffSet.hediffs[i] as Hediff_AddedPart;
                    if (hediff_AddedPart2 != null && hediff_AddedPart2.Part == part)
                    {
                        num *= hediff_AddedPart2.def.addedPartProps.partEfficiency;
                        if (hediff_AddedPart2.def.addedPartProps.partEfficiency != 1f)
                        {
                            impactors?.Add(new CapacityImpactorHediff
                            {
                                hediff = hediff_AddedPart2
                            });
                        }
                    }
                }
            }
            float b = -1f;
            float num2 = 0f;
            bool flag = false;
            for (int j = 0; j < diffSet.hediffs.Count; j++)
            {
                if (diffSet.hediffs[j].Part == part && diffSet.hediffs[j].CurStage != null)
                {
                    HediffStage curStage = diffSet.hediffs[j].CurStage;
                    num2 += curStage.partEfficiencyOffset;
                    flag |= curStage.partIgnoreMissingHP;
                    if (curStage.partEfficiencyOffset != 0f && curStage.becomeVisible)
                    {
                        impactors?.Add(new CapacityImpactorHediff
                        {
                            hediff = diffSet.hediffs[j]
                        });
                    }
                }
            }
            if (!flag)
            {
                float num3 = diffSet.GetPartHealth(part) / part.def.GetMaxHealth(diffSet.pawn);
                if (num3 != 1f)
                {
                    if (DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(part))
                    {
                        num3 = Mathf.InverseLerp(0.1f, 1f, num3);
                    }
                    impactors?.Add(new CapacityImpactorBodyPartHealth
                    {
                        bodyPart = part
                    });
                    num *= num3;
                }
            }
            num += num2;
            if (num > 0.0001f)
            {
                num = Mathf.Max(num, b);
            }
            return Mathf.Max(num, 0f);
        }
    }
}
