using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VPEPuppeteer_AbilityExtension_TargetValidator_ValidateTarget_Patch
    {
        public static MethodInfo targetMethod;
        public static bool Prepare()
        {
            targetMethod = AccessTools.Method("VPEPuppeteer.AbilityExtension_TargetValidator:ValidateTarget");
            return targetMethod != null;
        }
        public static MethodBase TargetMethod()
        {
            return targetMethod;
        }

        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(LocalTargetInfo target, bool throwMessages)
        {
            if (target.Thing is Pawn pawn && pawn.IsAndroid())
            {
                if (throwMessages)
                {
                    Messages.Message("VREA.CannotCastOnAndroid".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                }
                return false;
            }
            return true;
        }
    }
}
