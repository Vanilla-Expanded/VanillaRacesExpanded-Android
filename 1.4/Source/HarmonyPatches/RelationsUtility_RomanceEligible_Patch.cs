using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RelationsUtility), "RomanceEligible")]
    public static class RelationsUtility_RomanceEligible_Patch
    {
        public static void Postfix(Pawn pawn, ref AcceptanceReport __result)
        {
            if (pawn.Emotionless())
            {
                __result = false;
            }
        }
    }
}
