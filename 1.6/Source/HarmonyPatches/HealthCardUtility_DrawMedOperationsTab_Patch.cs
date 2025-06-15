using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class HealthCardUtility_DrawMedOperationsTab_Patch
    {
        public static MethodBase TargetMethod()
        {
            foreach (var nestedType in typeof(HealthCardUtility).GetNestedTypes(AccessTools.all))
            {
                foreach (var method in nestedType.GetMethods(AccessTools.all))
                {
                    if (method.Name.Contains("<DrawMedOperationsTab>b") && method.ReturnType == typeof(List<FloatMenuOption>))
                    {
                        return method;
                    }
                }
            }
            return null;
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions, MethodBase method)
        {
            var filterThingsInfo = AccessTools.Method(typeof(HealthCardUtility_DrawMedOperationsTab_Patch), "FilterThings");
            var pawnField = AccessTools.Field(method.DeclaringType, "pawn");
            foreach (var instruction in codeInstructions)
            {
                yield return instruction;
                if (instruction.opcode == OpCodes.Stloc_S && instruction.operand is LocalBuilder lb && lb.LocalIndex == 5)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 5);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, pawnField);
                    yield return new CodeInstruction(OpCodes.Call, filterThingsInfo);
                    yield return new CodeInstruction(OpCodes.Stloc_S, 5);
                }
            }
        }

        public static IEnumerable<ThingDef> FilterThings(IEnumerable<ThingDef> thingDefs, Pawn patient)
        {
            if (patient.IsAndroid())
            {
                thingDefs = thingDefs.Where(x => x.IsMedicine is false);
            }
            return thingDefs;
        }
    }
}
