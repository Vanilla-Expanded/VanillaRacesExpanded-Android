using HarmonyLib;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VSIE_SocialInteractionsManager_TryDevelopNewTrait_Patch
    {
        public static MethodBase targetMethod;

        [HarmonyPrepare]
        public static bool Prepare()
        {
            if (ModLister.AnyModActiveNoSuffix(["VanillaExpanded.VanillaSocialInteractionsExpanded"]))
            {
                targetMethod = AccessTools.Method("VanillaSocialInteractionsExpanded.SocialInteractionsManager:TryDevelopNewTrait");
                if (targetMethod != null)
                {
                    return true;
                }
                Log.Error("[VREAndroids] Failed to patch VanillaSocialInteractionsExpanded mod for TryDevelopNewTrait");
            }
            return false;
        }

        [HarmonyTargetMethod]
        public static MethodBase TargetMethod() => targetMethod;

        [HarmonyPriority(int.MaxValue)]
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
