using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthAIUtility), "ShouldBeTendedNowByPlayer")]
    public static class HealthAIUtility_ShouldBeTendedNowByPlayer_Patch
    {
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
