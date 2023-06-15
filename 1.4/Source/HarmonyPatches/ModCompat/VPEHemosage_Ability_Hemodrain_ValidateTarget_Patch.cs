using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VPEHemosage_Ability_Hemodrain_ValidateTarget_Patch
    {
        public static MethodInfo targetMethod;
        public static bool Prepare()
        {
            targetMethod = AccessTools.Method("VPEHemosage.Ability_Hemodrain:ValidateTarget");
            return targetMethod != null;
        }
        public static MethodBase TargetMethod()
        {
            return targetMethod;
        }

        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(LocalTargetInfo target, bool showMessages = true)
        {
            if (target.Thing is Pawn pawn && pawn.IsAndroid())
            {
                if (showMessages)
                {
                    Messages.Message("VREA.CannotCastOnAndroid".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, historical: false);

                }
                return false;
            }
            return true;
        }
    }
}
