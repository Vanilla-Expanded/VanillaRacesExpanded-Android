using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
    public static class PawnGenerator_TryGenerateNewPawnInternal_Patch
    {
        public static void Postfix(ref Pawn __result)
        {
            if (__result != null)
            {
                if (__result.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled))
                {
                    __result.story.traits = new TraitSet(__result);
                }
                if (__result.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
                {
                    __result.story.Adulthood = null;
                }
            }
        }
    }
}
