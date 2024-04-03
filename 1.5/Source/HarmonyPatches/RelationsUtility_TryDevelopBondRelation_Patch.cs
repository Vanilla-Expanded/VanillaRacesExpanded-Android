using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RelationsUtility), "TryDevelopBondRelation")]
    public static class RelationsUtility_TryDevelopBondRelation_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn humanlike, Pawn animal, ref float baseChance)
        {
            if (humanlike.Emotionless())
            {
                return false;
            }
            return true;
        }
    }
}
