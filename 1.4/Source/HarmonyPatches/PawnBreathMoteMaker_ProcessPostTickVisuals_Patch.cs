using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnBreathMoteMaker), "ProcessPostTickVisuals")]
    public static class PawnBreathMoteMaker_ProcessPostTickVisuals_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(PawnBreathMoteMaker __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
