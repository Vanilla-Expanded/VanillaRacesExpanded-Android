using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(TraitSet), "GainTrait")]
    public static class TraitSet_GainTrait_Patch
    {
        public static bool shouldAllowGaining;
        public static bool Prefix(Pawn ___pawn, Trait trait)
        {
            if (!shouldAllowGaining && ___pawn.IsAndroid(out var state) && state != AndroidState.Awakened)
            {
                return false;
            }
            return true;
        }
    }
}
