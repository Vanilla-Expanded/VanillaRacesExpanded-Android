using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(DaysWorthOfFoodCalculator), "ApproxDaysWorthOfFood", new Type[] {typeof(List<Pawn>), typeof(List<ThingDefCount>), 
        typeof(int), typeof(IgnorePawnsInventoryMode), typeof(Faction), typeof(WorldPath), typeof(float), typeof(int), typeof(bool) })]
    public static class DaysWorthOfFoodCalculator_ApproxDaysWorthOfFood_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var eatsFood = AccessTools.PropertyGetter(typeof(RaceProperties), nameof(RaceProperties.EatsFood));
            for (var i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if ((codes[i].opcode == OpCodes.Brfalse_S || codes[i].opcode == OpCodes.Brfalse) && codes[i - 1].Calls(eatsFood))
                {
                    Log.Message("Found: " + codes[i - 3]);
                    if (codes[i - 3].opcode == OpCodes.Ldloc_S)
                    {
                        yield return new CodeInstruction(OpCodes.Ldloc_S, (codes[i - 3].operand as LocalBuilder).LocalIndex);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Utils), nameof(Utils.IsAndroid)));
                        yield return new CodeInstruction(OpCodes.Brtrue_S, codes[i].operand);
                    }
                    else if (codes[i - 3].opcode == OpCodes.Callvirt)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_0);
                        yield return new CodeInstruction(OpCodes.Ldloc_S, (codes[i - 4].operand as LocalBuilder).LocalIndex);
                        yield return new CodeInstruction(OpCodes.Callvirt, codes[i - 3].operand);
                        yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Utils), nameof(Utils.IsAndroid)));
                        yield return new CodeInstruction(OpCodes.Brtrue_S, codes[i].operand);
                    }
                }
            }
        }
    }
}
