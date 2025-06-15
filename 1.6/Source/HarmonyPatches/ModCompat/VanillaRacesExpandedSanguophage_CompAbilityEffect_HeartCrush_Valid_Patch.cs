using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VanillaRacesExpandedSanguophage_CompAbilityEffect_HeartCrush_Valid_Patch
    {
        public static MethodInfo targetMethod;
        public static bool Prepare()
        {
            targetMethod = AccessTools.Method("VanillaRacesExpandedSanguophage.CompAbilityEffect_HeartCrush:Valid", new Type[] { typeof(LocalTargetInfo), typeof(bool)});
            return targetMethod != null;
        }
        public static MethodBase TargetMethod()
        {
            return targetMethod;
        }

        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref bool __result, LocalTargetInfo target, bool throwMessages)
        {
            if (__result)
            {
                Pawn pawn = target.Pawn;
                if (pawn != null && pawn.IsAndroid())
                {
                    if (throwMessages)
                    {
                        Messages.Message("VREA.CannotCastOnAndroid".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    __result = false;
                }
            }
        }
    }
}
