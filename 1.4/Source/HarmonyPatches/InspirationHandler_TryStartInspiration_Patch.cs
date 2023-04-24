using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InspirationHandler), "TryStartInspiration")]
    public static class InspirationHandler_TryStartInspiration_Patch
    {
        public static bool Prefix(InspirationHandler __instance)
        {
            if (__instance.pawn.IsAndroid(out var state) && state != AndroidState.Awakened)
            {
                return false;
            }
            return true;
        }
    }
}
