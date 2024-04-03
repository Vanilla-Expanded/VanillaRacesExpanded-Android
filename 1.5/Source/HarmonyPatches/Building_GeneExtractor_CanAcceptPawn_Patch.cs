using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Building_GeneExtractor), "CanAcceptPawn")]
    public static class Building_GeneExtractor_CanAcceptPawn_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn pawn, ref AcceptanceReport __result)
        {
            if (pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
