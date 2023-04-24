using HarmonyLib;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MentalBreaker), "BreakExtremeIsApproaching", MethodType.Getter)]
    public static class MentalBreaker_BreakExtremeIsApproaching_Patch
    {
        public static void Postfix(MentalBreaker __instance, ref bool __result)
        {
            if (__instance.pawn.IsAndroid(out var state) && state != AndroidState.Awakened)
            {
                __result = false;
            }
        }
    }
}
