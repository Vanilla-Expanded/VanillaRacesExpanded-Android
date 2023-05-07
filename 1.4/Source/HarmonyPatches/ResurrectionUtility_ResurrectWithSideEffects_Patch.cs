using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ResurrectionUtility), "ResurrectWithSideEffects")]
    public static class ResurrectionUtility_ResurrectWithSideEffects_Patch
    {
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
