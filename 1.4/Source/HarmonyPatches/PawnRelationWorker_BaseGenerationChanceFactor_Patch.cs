using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnRelationWorker), "BaseGenerationChanceFactor")]
    public static class PawnRelationWorker_BaseGenerationChanceFactor_Patch
    {
        public static void Postfix(ref float __result, PawnRelationWorker __instance, Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            if (generated.HasActiveGene(VREA_DefOf.VREA_SyntheticBody) || other.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                if (__instance.def.familyByBloodRelation)
                {
                    __result = 0;
                }
            }
        }
    }
}
