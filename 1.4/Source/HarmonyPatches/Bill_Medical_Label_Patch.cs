using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Bill_Medical), "Label", MethodType.Getter)]
    public static class Bill_Medical_Label_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(Bill_Medical __instance)
        {
            curPawn = __instance.GiverPawn;
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            return codeInstructions.MethodReplacer(AccessTools.PropertyGetter(typeof(BodyPartRecord), nameof(BodyPartRecord.Label)),
                AccessTools.Method(typeof(Bill_Medical_Label_Patch), nameof(GetAndroidCounterPart)));
        }

        public static string GetAndroidCounterPart(BodyPartRecord bodyPartRecord)
        {
            if (curPawn.IsAndroid())
            {
                var counterPart = bodyPartRecord.def.GetAndroidCounterPart();
                if (counterPart != null)
                {
                    return bodyPartRecord.AndroidPartLabel(counterPart);
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
