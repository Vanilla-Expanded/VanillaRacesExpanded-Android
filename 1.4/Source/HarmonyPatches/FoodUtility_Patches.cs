using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class FoodUtility_PostfixPatches
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> GetMethods()
        {
            yield return AccessTools.Method(typeof(FoodUtility), "WillEat", new Type[] { typeof(Pawn), typeof(Thing), typeof(Pawn), typeof(bool) });
            yield return AccessTools.Method(typeof(FoodUtility), "WillEat", new Type[] { typeof(Pawn), typeof(ThingDef), typeof(Pawn), typeof(bool) });
        }
    
        [HarmonyPriority(int.MaxValue)]
        public static void Postfix(Pawn __0, ref bool __result)
        {
            if (__0.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __result = false;
            }
        }
    }
}
