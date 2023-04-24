using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthCardUtility), "CreateSurgeryBill")]
    public static class HealthCardUtility_CreateSurgeryBill_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(Pawn medPawn)
        {
            curPawn = medPawn;
        }

        public static void Postfix()
        {
            curPawn = null;
        }
    }
}
