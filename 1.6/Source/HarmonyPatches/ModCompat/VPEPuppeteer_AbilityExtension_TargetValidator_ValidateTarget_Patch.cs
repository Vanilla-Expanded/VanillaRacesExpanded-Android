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
            if (ModsConfig.IsActive("VanillaExpanded.VPE.Puppeteer"))
            {
                targetMethod = AccessTools.Method("VPEPuppeteer.AbilityExtension_TargetValidator:ValidateTarget");
                if (targetMethod != null)
                {
                    return true;
                }
                Log.Error("[VREAndroids] Failed to patch VPE Puppeteer mod for AbilityExtension_TargetValidator");
            }
            return false;
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
