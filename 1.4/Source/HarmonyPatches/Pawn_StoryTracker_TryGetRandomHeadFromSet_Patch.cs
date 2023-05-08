using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_StoryTracker), "TryGetRandomHeadFromSet")]
    public static class Pawn_StoryTracker_TryGetRandomHeadFromSet_Patch
    {
        public static bool Prefix(Pawn_StoryTracker __instance, ref bool __result, IEnumerable<HeadTypeDef> options)
        {
            if (__instance.pawn.IsAndroid())
            {
                __result = TryGetRandomHeadFromSet(__instance, options);
                return false;
            }
            return true;
        }

        public static bool TryGetRandomHeadFromSet(Pawn_StoryTracker __instance, IEnumerable<HeadTypeDef> options)
        {
            Rand.PushState(__instance.pawn.thingIDNumber);
            bool result = options.Where((HeadTypeDef h) => CanUseHeadType(h)).TryRandomElementByWeight((HeadTypeDef x) => x.selectionWeight, 
                out __instance.headType);
            Rand.PopState();
            return result;
            bool CanUseHeadType(HeadTypeDef head)
            {
                if (ModsConfig.BiotechActive && !head.requiredGenes.NullOrEmpty())
                {
                    if (__instance.pawn.genes == null)
                    {
                        return false;
                    }
                    foreach (GeneDef requiredGene in head.requiredGenes)
                    {
                        GeneDef androidRequiredGene = null;
                        if (requiredGene.IsAndroidGene())
                        {
                            androidRequiredGene = requiredGene;
                        }
                        else if (GeneDefGenerator_ImpliedGeneDefs_Patch.originalGenesWithAndroidCounterparts.TryGetValue(requiredGene, out var counterpartGene))
                        {
                            androidRequiredGene = counterpartGene;
                        }
                        if (androidRequiredGene is null || !__instance.pawn.genes.HasGene(androidRequiredGene))
                        {
                            return false;
                        }
                    }
                }
                if (head.gender != 0)
                {
                    return head.gender == __instance.pawn.gender;
                }
                return true;
            }
        }
    }
}
