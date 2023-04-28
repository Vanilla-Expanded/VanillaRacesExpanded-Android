using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{

    [HarmonyPatch(typeof(HealthCardUtility), "DrawHediffListing")]
    public static class HealthCardUtility_DrawHediffListing_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            foreach (var code in codes)
            {
                yield return code;
                if (code.opcode == OpCodes.Stloc_S && code.operand is LocalBuilder lb && lb.LocalIndex == 9)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 9);
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(HealthCardUtility_DrawHediffListing_Patch), nameof(ReplaceBleedingTextIfAndroid)));
                }
            }
        }
        public static void ReplaceBleedingTextIfAndroid(Pawn pawn, ref string text)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_NeutroCirculation))
            {
                float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
                var neutroloss = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
                if (neutroloss.Severity >= 1)
                {
                    text = "VREA.NeutroamineLeakedOutCompletely".Translate();
                }
                else
                {
                    text = "VREA.NeutrolossRate".Translate() + ": " + bleedRateTotal.ToStringPercent() + "/" + "LetterDay".Translate();
                    int num = TicksUntilTotalNeutroloss(pawn);
                    text += " (" + "VREA.TotalNeutroLoss".Translate(num.ToStringTicksToPeriod()) + ")";
                }
            }
        }

        public static int TicksUntilTotalNeutroloss(Pawn pawn)
        {
            float bleedRateTotal = pawn.health.hediffSet.BleedRateTotal;
            if (bleedRateTotal < 0.0001f)
            {
                return int.MaxValue;
            }
            float num = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss)?.Severity ?? 0f;
            return (int)((1f - num) / bleedRateTotal * 60000f);
        }
    }
}
