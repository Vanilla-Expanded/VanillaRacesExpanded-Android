using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffSet), "GetPartHealth")]
    public static class HediffSet_GetPartHealth_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var destroyableByDamageInfo = AccessTools.Field(typeof(BodyPartDef), "destroyableByDamage");
            var pawnInfo = AccessTools.Field(typeof(HediffSet), "pawn");
            for (var i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (codes[i].opcode == OpCodes.Brtrue_S && codes[i - 1].LoadsField(destroyableByDamageInfo))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, pawnInfo);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Utils), nameof(Utils.IsAndroid), new Type[] { typeof(Pawn) }));
                    yield return new CodeInstruction(OpCodes.Brtrue_S, codes[i].operand);
                }
            }
        }
    }
}
