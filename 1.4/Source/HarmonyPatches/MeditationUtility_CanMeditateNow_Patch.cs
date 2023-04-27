using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MeditationUtility), "CanMeditateNow")]
    public static class MeditationUtility_CanMeditateNow_Patch
    {
        public static void Postfix(ref bool __result, Pawn pawn)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_JoyDisabled))
            {
                __result = false;
            }
        }
    }
}
