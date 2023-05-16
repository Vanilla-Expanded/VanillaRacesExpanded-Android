using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnRelationWorker), "BaseGenerationChanceFactor")]
    public static class PawnRelationWorker_BaseGenerationChanceFactor_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref float __result, PawnRelationWorker __instance, Pawn generated, Pawn other, PawnGenerationRequest request)
        {
            if (generated.IsAndroid() || other.IsAndroid())
            {
                if (__instance.def.familyByBloodRelation)
                {
                    __result = 0;
                }
            }
        }
    }
}
