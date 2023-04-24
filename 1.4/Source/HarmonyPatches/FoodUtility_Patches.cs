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
            if (__0.IsAndroid())
            {
                __result = false;
            }
        }
    }

    //[HarmonyPatch]
    //public static class FoodUtility_PrefixPatches
    //{
    //    [HarmonyTargetMethods]
    //    public static IEnumerable<MethodBase> GetMethods()
    //    {
    //        yield return AccessTools.Method(typeof(FoodUtility), "WillIngestStackCountOf");
    //    }
    //
    //    [HarmonyPriority(int.MaxValue)]
    //    public static bool Prefix(Pawn __0)
    //    {
    //        if (__0.IsAndroid())
    //        {
    //            return false;
    //        }
    //        return true;
    //    }
    //}
    //
    //[HarmonyPatch(typeof(FoodUtility), "InappropriateForTitle")]
    //public static class FoodUtility_InappropriateForTitle_Patch
    //{
    //    public static bool Prefix(ThingDef food, Pawn p, bool allowIfStarving)
    //    {
    //        if (p.IsAndroid())
    //        {
    //            return false;
    //        }
    //        return true;
    //    }
    //}
}
