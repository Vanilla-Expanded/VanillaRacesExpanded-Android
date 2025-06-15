using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class HarmonyPatches_ForRomanceReasonsChangeAge
    {
        public static List<MethodBase> currentMethods = new List<MethodBase>();
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(SocialCardUtility), "CanDrawTryRomance");
            yield return AccessTools.Method(typeof(RelationsUtility), "RomanceEligiblePair");
            yield return AccessTools.Method(typeof(RelationsUtility), "RomanceEligible");
            yield return AccessTools.Method(typeof(Pawn_RelationsTracker), "SecondaryLovinChanceFactor");
            yield return AccessTools.Method(typeof(CompAbilityEffect_WordOfLove), "ValidateTarget");
        }

        public static void Prefix(MethodBase __originalMethod)
        {
            currentMethods.Add(__originalMethod);
        }

        public static void Postfix(MethodBase __originalMethod)
        {
            currentMethods.Remove(__originalMethod);
        }
    }

    [HarmonyPatch(typeof(Pawn_AgeTracker), "AgeBiologicalYearsFloat", MethodType.Getter)]
    public static class Pawn_AgeTracker_AgeBiologicalYearsFloat_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn_AgeTracker __instance, ref float __result)
        {
            if (HarmonyPatches_ForRomanceReasonsChangeAge.currentMethods.Any() 
                && __instance.pawn.IsAndroid() && !__instance.pawn.Emotionless())
            {
                __result = 18f;
            }
        }
    }
}
