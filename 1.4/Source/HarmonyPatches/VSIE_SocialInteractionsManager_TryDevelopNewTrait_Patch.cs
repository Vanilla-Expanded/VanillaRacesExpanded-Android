using HarmonyLib;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VSIE_SocialInteractionsManager_TryDevelopNewTrait_Patch
    {
        public static MethodBase methodInfo;

        [HarmonyPrepare]
        public static bool Prepare()
        {
            methodInfo = AccessTools.Method("VanillaSocialInteractionsExpanded.SocialInteractionsManager:TryDevelopNewTrait");
            return methodInfo != null;
        }

        [HarmonyTargetMethod]
        public static MethodBase TargetMethod() => methodInfo;

        public static bool Prefix(Pawn pawn)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled))
            {
                return false;
            }
            return true;
        }
    }
}
