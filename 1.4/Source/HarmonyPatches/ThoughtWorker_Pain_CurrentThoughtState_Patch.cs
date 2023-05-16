using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThoughtWorker_Pain), "CurrentThoughtState")]
    public static class ThoughtWorker_Pain_CurrentThoughtState_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(ref ThoughtState __result, Pawn p)
        {
            if (p.HasActiveGene(VREA_DefOf.VREA_PainDisabled))
            {
                __result = CurrentThoughtState(p);
                return false;
            }
            return true;
        }

        private static float CalculatePain(Pawn pawn)
        {
            if (!pawn.RaceProps.IsFlesh || pawn.Dead)
            {
                return 0f;
            }
            float num = 0f;
            for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
            {
                num += pawn.health.hediffSet.hediffs[i].PainOffset;
            }
            if (pawn.genes != null)
            {
                num += pawn.genes.PainOffset;
            }
            for (int j = 0; j < pawn.health.hediffSet.hediffs.Count; j++)
            {
                num *= pawn.health.hediffSet.hediffs[j].PainFactor;
            }
            return Mathf.Clamp(num, 0f, 1f);
        }
        public static ThoughtState CurrentThoughtState(Pawn p)
        {
            float painTotal = CalculatePain(p);
            if (painTotal < 0.0001f)
            {
                return ThoughtState.Inactive;
            }
            if (painTotal < 0.15f)
            {
                return ThoughtState.ActiveAtStage(0);
            }
            if (painTotal < 0.4f)
            {
                return ThoughtState.ActiveAtStage(1);
            }
            if (painTotal < 0.8f)
            {
                return ThoughtState.ActiveAtStage(2);
            }
            return ThoughtState.ActiveAtStage(3);
        }
    }
}
