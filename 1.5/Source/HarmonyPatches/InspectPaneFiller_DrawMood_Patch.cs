using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InspectPaneFiller), "DrawMood")]
    public static class InspectPaneFiller_DrawMood_Patch
    {
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
