using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthCardUtility), "DrawHediffRow")]
    public static class HealthCardUtility_DrawHediffRow_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(Pawn pawn)
        {
            curPawn = pawn;
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions, ILGenerator ilg)
        {
            var codes = codeInstructions.MethodReplacer(AccessTools.PropertyGetter(typeof(BodyPartRecord), nameof(BodyPartRecord.LabelCap)),
                AccessTools.Method(typeof(HealthCardUtility_DrawHediffRow_Patch), nameof(GetAndroidCounterPart))).ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
            }
        }

        public static string GetAndroidCounterPart(BodyPartRecord bodyPartRecord)
        {
            if (curPawn.IsAndroid())
            {
                var counterPart = bodyPartRecord.def.GetAndroidCounterPart();
                if (counterPart != null)
                {
                    return bodyPartRecord.AndroidPartLabel(counterPart).CapitalizeFirst();
                }
            }
            return bodyPartRecord.LabelCap;
        }

        public static void Postfix()
        {
            curPawn = null;
        }
    }
}
