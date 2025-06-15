using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThoughtWorker_AgeReversalDemanded), "ShouldHaveThought")]
    public static class ThoughtWorker_AgeReversalDemanded_ShouldHaveThought_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn p, ref ThoughtState __result)
        {
            if (p.IsAndroid())
            {
                __result = ThoughtState.Inactive;
                return false;
            }
            return true;
        }
    }
}
