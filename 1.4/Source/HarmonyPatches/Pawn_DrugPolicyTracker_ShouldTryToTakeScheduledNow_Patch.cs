using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_DrugPolicyTracker), "ShouldTryToTakeScheduledNow")]
    public static class Pawn_DrugPolicyTracker_ShouldTryToTakeScheduledNow_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn_DrugPolicyTracker __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }

}
