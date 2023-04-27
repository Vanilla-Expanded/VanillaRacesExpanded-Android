using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SkillRecord), "Interval")]
    public static class SkillRecord_Interval_Patch
    {
        public static bool Prefix(SkillRecord __instance, Pawn ___pawn)
        {
            if (___pawn.HasActiveGene(VREA_DefOf.VREA_NoSkillGain))
            {
                return false;
            }
            return true;
        }
    }

}
