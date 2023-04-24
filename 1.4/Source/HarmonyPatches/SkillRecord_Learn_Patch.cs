using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SkillRecord), "Learn")]
    public static class SkillRecord_Learn_Patch
    {
        public static bool Prefix(SkillRecord __instance, Pawn ___pawn, float xp, bool direct = false)
        {
            if (___pawn.IsAndroid(out var state) && state != AndroidState.Awakened)
            {
                return false;
            }
            return true;
        }
    }
}
