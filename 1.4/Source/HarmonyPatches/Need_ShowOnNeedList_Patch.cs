using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Need), "ShowOnNeedList", MethodType.Getter)]
    public static class Need_ShowOnNeedList_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Need __instance, Pawn ___pawn)
        {
            if (__instance is Need_Mood && ___pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled))
            {
                return false;
            }
            return true;
        }
    }
}
