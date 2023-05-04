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
            if (pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
