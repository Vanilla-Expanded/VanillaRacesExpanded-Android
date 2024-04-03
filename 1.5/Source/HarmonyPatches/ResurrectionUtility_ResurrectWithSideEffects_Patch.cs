using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ResurrectionUtility), "TryResurrectWithSideEffects")]
    public static class ResurrectionUtility_TryResurrectWithSideEffects_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
