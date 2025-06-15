using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_RelationsTracker), "LovinAgeFactor")]
    public static class Pawn_RelationsTracker_LovinAgeFactor_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn_RelationsTracker __instance, Pawn otherPawn, ref float __result)
        {
            if (__instance.pawn.IsAndroid() || otherPawn.IsAndroid())
            {
                __result = 1;
                return false;
            }
            return true;
        }
    }
}
