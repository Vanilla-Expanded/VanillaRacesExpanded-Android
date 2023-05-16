using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InspirationHandler), "TryStartInspiration")]
    public static class InspirationHandler_TryStartInspiration_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(InspirationHandler __instance)
        {
            if (__instance.pawn.HasActiveGene(VREA_DefOf.VREA_Uninspired))
            {
                return false;
            }
            return true;
        }
    }
}
