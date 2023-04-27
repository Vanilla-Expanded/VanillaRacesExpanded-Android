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
            if (___pawn.HasActiveGene(VREA_DefOf.VREA_NoSkillGain))
            {
                return false;
            }
            return true;
        }
    }
}
