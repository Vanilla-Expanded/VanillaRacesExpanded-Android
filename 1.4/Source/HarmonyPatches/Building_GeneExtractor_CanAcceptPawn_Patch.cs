using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Building_GeneExtractor), "CanAcceptPawn")]
    public static class Building_GeneExtractor_CanAcceptPawn_Patch
    {
        public static void Postfix(Pawn pawn, ref AcceptanceReport __result)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __result = false;
            }
        }
    }
}
