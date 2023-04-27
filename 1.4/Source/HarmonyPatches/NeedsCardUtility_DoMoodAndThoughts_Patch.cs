using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(NeedsCardUtility), "DoMoodAndThoughts")]
    public static class NeedsCardUtility_DoMoodAndThoughts_Patch
    {
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
