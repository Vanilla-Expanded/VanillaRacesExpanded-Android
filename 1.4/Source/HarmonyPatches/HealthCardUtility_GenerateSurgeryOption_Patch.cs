using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthCardUtility), "GenerateSurgeryOption")]
    public static class HealthCardUtility_GenerateSurgeryOption_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(Pawn pawn)
        {
            curPawn = pawn;
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            return codeInstructions.MethodReplacer(AccessTools.PropertyGetter(typeof(BodyPartRecord), nameof(BodyPartRecord.Label)),
                AccessTools.Method(typeof(HealthCardUtility_GenerateSurgeryOption_Patch), nameof(GetAndroidCounterPart)));
        }

        public static string GetAndroidCounterPart(BodyPartRecord bodyPartRecord)
        {
            if (curPawn.IsAndroid())
            {
                var counterPart = bodyPartRecord.def.GetAndroidCounterPart();
                if (counterPart != null)
                {
                    return counterPart.label;
                }
            }
            return bodyPartRecord.Label;
        }
        public static void Postfix()
        {
            curPawn = null;
        }
    }
}
