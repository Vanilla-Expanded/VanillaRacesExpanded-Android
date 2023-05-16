using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
    public static class PawnGenerator_TryGenerateNewPawnInternal_Patch
    {
        public static PawnGenerationRequest? curRequest;
        public static void Prefix(PawnGenerationRequest request)
        {
            curRequest = request;
        }
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref Pawn __result)
        {
            curRequest = null;
            if (__result?.genes != null)
            {
                if (__result.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled))
                {
                    __result.story.traits = new TraitSet(__result);
                    __result.story.favoriteColor = null;
                }
                var gene = __result.genes.GetGene(VREA_DefOf.VREA_SyntheticBody) as Gene_SyntheticBody;
                if (gene != null)
                {
                    if (gene.Awakened is false)
                    {
                        if (__result.Name is NameTriple nameTriple)
                        {
                            gene.storedTripleName = nameTriple;
                            __result.Name = new NameSingle(nameTriple.First);
                        }
                    }
                }
            }
        }
    }
}
