using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Hediff_AddedPart), "PostAdd")]
    public static class Hediff_AddedPart_PostAdd_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions, ILGenerator il)
        {
            var codes = codeInstructions.ToList();
            var restorePart = AccessTools.Method(typeof(Pawn_HealthTracker), "RestorePart");
            var pawnField = AccessTools.Field(typeof(Hediff_AddedPart), "pawn");
            var shouldReturnIfAndroidInfo = AccessTools.Method(typeof(Hediff_AddedPart_PostAdd_Patch), "ShouldReturnIfAndroid");
            var label = il.DefineLabel();
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (code.Calls(restorePart))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    codes[i + 1].labels.Add(label);
                    yield return new CodeInstruction(OpCodes.Ldfld, pawnField);
                    yield return new CodeInstruction(OpCodes.Call, shouldReturnIfAndroidInfo);
                    yield return new CodeInstruction(OpCodes.Brfalse_S, label);
                    yield return new CodeInstruction(OpCodes.Ret);
                }
            }
        }

        public static bool ShouldReturnIfAndroid(Pawn pawn) => pawn.IsAndroid();
    }
}
