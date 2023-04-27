using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthUtility), "TryAnesthetize")]
    public static class HealthUtility_TryAnesthetize_Patch
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
