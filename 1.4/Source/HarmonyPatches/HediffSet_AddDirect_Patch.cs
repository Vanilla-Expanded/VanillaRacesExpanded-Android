using HarmonyLib;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffSet), "AddDirect")]
    public static class HediffSet_AddDirect_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(HediffSet __instance, Pawn ___pawn, Hediff hediff)
        {
            if (___pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticImmunity) && Utils.AndroidCanCatch(hediff.def) is false)
            {
                return false;
            }
            if (hediff is Hediff_MissingPart && hediff.Part != null && !__instance.GetNotMissingParts().Contains(hediff.Part))
            {
                return false;
            }
            return true;
        }
    }
}
