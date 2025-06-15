using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_RelationsTracker), "OpinionOf")]
    public static class Pawn_RelationsTracker_OpinionOf_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn_RelationsTracker __instance)
        {
            if (__instance.pawn.Emotionless())
            {
                return false;
            }
            return true;
        }
    }
}
