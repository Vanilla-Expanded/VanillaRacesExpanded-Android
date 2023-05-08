using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthCardUtility), "DrawPawnHealthCard")]
    public static class HealthCardUtility_DrawPawnHealthCard_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(Pawn pawn)
        {
            curPawn = pawn;
        }

        [HarmonyPriority(int.MinValue)]
        public static void Postfix()
        {
            curPawn = null;
        }
    }
}
