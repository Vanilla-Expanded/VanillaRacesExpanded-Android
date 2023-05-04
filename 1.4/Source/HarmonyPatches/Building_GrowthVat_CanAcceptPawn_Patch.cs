using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Building_GrowthVat), "CanAcceptPawn")]
    public static class Building_GrowthVat_CanAcceptPawn_Patch
    {
        public static void Postfix(Pawn pawn, ref AcceptanceReport __result)
        {
            if (pawn.IsAndroid())
            {
                __result = "VREA.IsAndroid".Translate(pawn.Named("PAWN"));
            }
        }
    }
}
