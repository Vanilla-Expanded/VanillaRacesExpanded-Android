using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThoughtUtility), "CanGetThought")]
    public static class ThoughtUtility_CanGetThought_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.IsAndroid() && pawn.IsAwakened() is false)
            {
                return false;
            }
            return true;
        }
    }
}
