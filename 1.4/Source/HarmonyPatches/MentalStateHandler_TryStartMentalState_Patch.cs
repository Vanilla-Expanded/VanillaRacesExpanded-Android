using HarmonyLib;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MentalStateHandler), "TryStartMentalState")]
    public static class MentalStateHandler_TryStartMentalState_Patch
    {
        public static bool Prefix(MentalStateHandler __instance, MentalStateDef stateDef, Pawn ___pawn)
        {
            if (___pawn.IsAndroid(out var state) && state != AndroidState.Awakened 
                && VREA_DefOf.VREA_AndroidSettings.androidSpecificMentalBreaks.Contains(stateDef.defName) is false)
            {
                return false;
            }
            return true;
        }
    }
}
