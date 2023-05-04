using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(AgeInjuryUtility), "GenerateRandomOldAgeInjuries")]
    public static class AgeInjuryUtility_GenerateRandomOldAgeInjuries_Patch
    {
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                return false;
            }
            return true;
        }
    }
}
