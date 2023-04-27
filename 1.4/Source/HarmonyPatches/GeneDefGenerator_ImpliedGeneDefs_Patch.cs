using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(GeneDefGenerator), "ImpliedGeneDefs")]
    public static class GeneDefGenerator_ImpliedGeneDefs_Patch
    {
        public static List<GeneDef> allGenes = DefDatabase<GeneDef>.AllDefsListForReading.ListFullCopy();
        public static MethodInfo cloneMethod = typeof(GeneDef).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
        public static HashSet<GeneCategoryDef> allCosmeticCategories = new()
        {
            VREA_DefOf.Cosmetic, VREA_DefOf.Cosmetic_Body, VREA_DefOf.Cosmetic_Hair, VREA_DefOf.Cosmetic_Skin, VREA_DefOf.Beauty
        };

        public static IEnumerable<GeneDef> Postfix(IEnumerable<GeneDef> __result)
        {
            foreach (var r in __result)
            {
                yield return r;
            }
            foreach (var geneDef in allGenes) 
            {
                if (geneDef.displayCategory == VREA_DefOf.VREA_Hardware || geneDef.displayCategory == VREA_DefOf.VREA_Subroutine)
                {
                    Utils.allAndroidGenes.Add(geneDef);
                }
                else
                {
                    if (allCosmeticCategories.Contains(geneDef.displayCategory) && geneDef.biostatArc <= 0)
                    {
                        var clonedGene = cloneMethod.Invoke(geneDef, null) as GeneDef;
                        clonedGene.defName = "VREA_" + geneDef.defName;
                        clonedGene.modExtensions ??= new List<DefModExtension>();
                        clonedGene.canGenerateInGeneSet = false;
                        Utils.allAndroidGenes.Add(clonedGene);
                        yield return clonedGene;
                    }
                }
            }

        }
    }
}
