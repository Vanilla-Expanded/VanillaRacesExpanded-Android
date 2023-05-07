using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_AgeTracker), "BirthdayBiological")]
    public static class Pawn_AgeTracker_BirthdayBiological_Patch
    {
        public static bool Prefix(Pawn_AgeTracker __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                __instance.RecalculateLifeStageIndex();
                return false;
            }
            return true;
        }
    }
}
