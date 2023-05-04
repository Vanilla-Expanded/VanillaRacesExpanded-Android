using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
    public static class PawnGenerator_TryGenerateNewPawnInternal_Patch
    {
        public static void Postfix(ref Pawn __result)
        {
            if (__result?.genes != null)
            {
                if (__result.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled))
                {
                    __result.story.traits = new TraitSet(__result);
                }
                var gene = __result.genes.GetGene(VREA_DefOf.VREA_SyntheticBody) as Gene_SyntheticBody;
                if (gene != null)
                {
                    __result.story.Adulthood = null;
                    if (__result.Name is NameTriple nameTriple)
                    {
                        __result.Name = new NameSingle(nameTriple.First);
                    }

                    if (gene.Awakened is false)
                    {
                        __result.story.favoriteColor = null;
                    }
                    else
                    {
                        if (__result.story.Childhood.spawnCategories?.Contains("AwakenedAndroid") is false)
                        {
                            __result.story.Childhood = DefDatabase<BackstoryDef>.AllDefs.Where(x => x.spawnCategories?.Contains("AwakenedAndroid") ?? false).RandomElement();
                        }
                    }
                }
            }
        }
    }
}
