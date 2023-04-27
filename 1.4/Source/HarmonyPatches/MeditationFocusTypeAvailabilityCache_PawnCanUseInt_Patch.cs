using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MeditationFocusTypeAvailabilityCache), nameof(MeditationFocusTypeAvailabilityCache.PawnCanUseInt))]
    public static class MeditationFocusTypeAvailabilityCache_PawnCanUseInt_Patch
    {
        public static void Postfix(ref bool __result, Pawn p)
        {
            if (p.HasActiveGene(VREA_DefOf.VREA_JoyDisabled))
            {
                __result = false;
            }
        }
    }
}
