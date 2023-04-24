using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RelationsUtility), "TryDevelopBondRelation")]
    public static class RelationsUtility_TryDevelopBondRelation_Patch
    {
        public static bool Prefix(Pawn humanlike, Pawn animal, ref float baseChance)
        {
            if (humanlike.IsAndroid(out var state) && state != AndroidState.Awakened)
            {
                return false;
            }
            return true;
        }
    }
}
