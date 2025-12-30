using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VPEHemosage_AbilityExtension_OnlyNonHemogenicHumanlikes_ValidateTarget_Patch
    {
        public static MethodInfo targetMethod;
        public static bool Prepare()
        {
            if (ModLister.AnyModActiveNoSuffix(["VanillaExpanded.VPE.Hemosage"]))
            {
                targetMethod = AccessTools.Method("VPEHemosage.AbilityExtension_OnlyNonHemogenicHumanlikes:ValidateTarget");
                if (targetMethod != null)
                {
                    return true;
                }
                Log.Error("[VREAndroids] Failed to patch VPEHemosage mod for AbilityExtension_OnlyNonHemogenicHumanlikes");
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
