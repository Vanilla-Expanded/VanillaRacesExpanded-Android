using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThoughtWorker_AgeReversalDemanded), "CanHaveThought")]
    public static class ThoughtWorker_AgeReversalDemanded_CanHaveThought_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
