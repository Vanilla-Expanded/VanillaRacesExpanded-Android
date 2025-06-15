using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "ShouldGenerateFloatMenuForPawn")]
    public static class FloatMenuMakerMap_ShouldGenerateFloatMenuForPawn_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn pawn, ref AcceptanceReport __result)
        {
            if (__result && pawn.HasActiveGene(VREA_DefOf.VREA_Uncontrollable))
            {
                __result = false;
            }
        }
    }
}
