using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Building_SubcoreScanner), "CanAcceptPawn")]
    public static class Building_SubcoreScanner_CanAcceptPawn_Patch
    {
        public static void Postfix(Pawn selPawn, ref AcceptanceReport __result)
        {
            if (selPawn.IsAndroid())
            {
                __result = "VREA.AndroidAreNotAllowed".Translate();
            }
        }
    }
}
