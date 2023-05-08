using HarmonyLib;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(DaysWorthOfFoodCalculator), "AnyFoodEatingPawn")]
    public static class DaysWorthOfFoodCalculator_AnyFoodEatingPawn_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var eatsFood = AccessTools.PropertyGetter(typeof(RaceProperties), nameof(RaceProperties.EatsFood));
            for (var i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (codes[i].opcode == OpCodes.Brfalse_S && codes[i - 1].Calls(eatsFood))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 0);
                    yield return new CodeInstruction(OpCodes.Callvirt, codes[i - 3].operand);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Utils), nameof(Utils.IsAndroid)));
                    yield return new CodeInstruction(OpCodes.Brtrue_S, codes[i].operand);
                }
            }
        }
    }
}
