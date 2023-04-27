using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_RelationsTracker), "OpinionOf")]
    public static class Pawn_RelationsTracker_OpinionOf_Patch
    {
        public static bool Prefix(Pawn_RelationsTracker __instance)
        {
            if (__instance.pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled))
            {
                return false;
            }
            return true;
        }
    }
}
