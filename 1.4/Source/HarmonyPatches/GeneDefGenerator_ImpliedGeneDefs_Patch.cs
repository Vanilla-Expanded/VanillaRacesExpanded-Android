using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using VanillaGenesExpanded;
using Verse;

namespace VREAndroids
{

    [HarmonyPatch(typeof(GeneDefGenerator), "ImpliedGeneDefs")]
    public static class GeneDefGenerator_ImpliedGeneDefs_Patch
    {
        public static List<GeneDef> allGenes = DefDatabase<GeneDef>.AllDefsListForReading.ListFullCopy();

        public static List<GeneDef> androidConvertableGenes = new List<GeneDef>();
        public static List<GeneDef> androidConvertableGenesBlacklist = new List<GeneDef>();
        public static List<GeneCategoryDef> androidConvertableGeneCategories = new List<GeneCategoryDef>();


        public static HashSet<GeneCategoryDef> allCosmeticCategories = new()
        {
            VREA_DefOf.Cosmetic, VREA_DefOf.Cosmetic_Body, VREA_DefOf.Cosmetic_Hair, VREA_DefOf.Cosmetic_Skin, VREA_DefOf.Beauty
        };

        public static Dictionary<GeneDef, GeneDef> originalGenesWithAndroidCounterparts = new Dictionary<GeneDef, GeneDef>();
        public static IEnumerable<GeneDef> Postfix(IEnumerable<GeneDef> __result)
        {
            foreach (var r in __result)
            {
                yield return r;
            }

            List<AndroidConvertableGenesDef> allAndroidConvertableGenes = DefDatabase<AndroidConvertableGenesDef>.AllDefsListForReading;
            foreach (AndroidConvertableGenesDef individualList in allAndroidConvertableGenes)
            {
                if (!individualList.genes.NullOrEmpty()) { androidConvertableGenes.AddRange(individualList.genes); }
                if (!individualList.geneCategories.NullOrEmpty()) { androidConvertableGeneCategories.AddRange(individualList.geneCategories); }
                if (!individualList.genesBlacklist.NullOrEmpty()) { androidConvertableGenesBlacklist.AddRange(individualList.genesBlacklist); }
            }
           


            foreach (var geneDef in allGenes) 
            {
                if (geneDef.displayCategory == VREA_DefOf.VREA_Hardware || geneDef.displayCategory == VREA_DefOf.VREA_Subroutine)
                {
                    Utils.allAndroidGenes.Add(geneDef);
                }
                else
                {
                    if (!androidConvertableGenesBlacklist.Contains(geneDef) &&(allCosmeticCategories.Contains(geneDef.displayCategory) && geneDef.biostatArc <= 0) || 
                        androidConvertableGeneCategories.Contains(geneDef.displayCategory) || androidConvertableGenes.Contains(geneDef))
                    {
                        GeneDef clonedGene = geneDef.Clone() as GeneDef;
                        clonedGene.defName = "VREA_" + geneDef.defName;
                        
                        var existingGeneExtension = clonedGene.GetModExtension<GeneExtension>();
                        
                        if (existingGeneExtension != null)
                        {
                            clonedGene.modExtensions.Remove(existingGeneExtension);
                            var clonedGeneExtension = (GeneExtension)existingGeneExtension.Clone();
                            
                            clonedGeneExtension.backgroundPathXenogenes = "UI/Icons/Genes/GeneBackground_Hardware";
                            clonedGeneExtension.backgroundPathEndogenes = "UI/Icons/Genes/GeneBackground_Hardware";
                          
                            clonedGene.modExtensions.Add(clonedGeneExtension);
                            
                        }
                        else
                        {
                            clonedGene.modExtensions ??= new List<DefModExtension>();
                            clonedGene.modExtensions.Add(new GeneExtension
                            {
                                backgroundPathXenogenes = "UI/Icons/Genes/GeneBackground_Hardware",
                                backgroundPathEndogenes = "UI/Icons/Genes/GeneBackground_Hardware"
                            });
                        }
                        clonedGene.canGenerateInGeneSet = false;
                        Utils.allAndroidGenes.Add(clonedGene);
                        originalGenesWithAndroidCounterparts[geneDef] = clonedGene;
                        yield return clonedGene;
                    }
                }
            }

            foreach (AndroidGeneTemplateDef g in DefDatabase<AndroidGeneTemplateDef>.AllDefs)
            {
                foreach (SkillDef allDef in DefDatabase<SkillDef>.AllDefs)
                {
                    if (g == VREA_DefOf.VREA_AptitudeIncapable && (allDef == SkillDefOf.Melee || allDef == SkillDefOf.Shooting))
                    {
                        continue;
                    }
                    else
                    {
                        var def = GetFromTemplate(g, allDef, allDef.index * 1000);
                        def.modExtensions??= new List<DefModExtension>();
                        def.modExtensions.Add(new GeneExtension
                        {
                            backgroundPathXenogenes = "UI/Icons/Genes/GeneBackground_Subroutine"
                        });
                        Utils.allAndroidGenes.Add(def);
                        yield return def;
                    }
                }
            }
        }

        private static GeneDef GetFromTemplate(AndroidGeneTemplateDef template, Def def, int displayOrderBase)
        {
            var geneDef = new AndroidGeneDef
            {
                defName = template.defName + "_" + def.defName,
                geneClass = template.geneClass,
                label = template.label.Formatted(def.label),
                iconPath = template.iconPath.Formatted(def.defName),
                description = ResolveDescription(),
                labelShortAdj = template.labelShortAdj.Formatted(def.label),
                selectionWeight = template.selectionWeight,
                biostatCpx = template.biostatCpx,
                biostatMet = template.biostatMet,
                displayCategory = template.displayCategory,
                displayOrderInCategory = displayOrderBase + template.displayOrderOffset,
                minAgeActive = template.minAgeActive,
                modContentPack = template.modContentPack,
                canGenerateInGeneSet = false,
            };
            if (!template.exclusionTagPrefix.NullOrEmpty())
            {
                geneDef.exclusionTags = new List<string> { template.exclusionTagPrefix + "_" + def.defName };
            }
            SkillDef skill;
            if ((skill = def as SkillDef) != null)
            {
                if (template.aptitudeOffset != 0)
                {
                    geneDef.aptitudes = new List<Aptitude>
                    {
                        new Aptitude(skill, template.aptitudeOffset)
                    };
                }
                else
                {
                    if (skill == SkillDefOf.Construction)
                    {
                        geneDef.disabledWorkTags = WorkTags.Constructing;
                    }
                    else if (skill == SkillDefOf.Artistic)
                    {
                        geneDef.disabledWorkTags = WorkTags.Artistic;
                    }
                    else if(skill == SkillDefOf.Animals)
                    {
                        geneDef.disabledWorkTags = WorkTags.Animals;
                    }
                    else if(skill == SkillDefOf.Plants)
                    {
                        geneDef.disabledWorkTags = WorkTags.PlantWork;
                    }
                    else if(skill == SkillDefOf.Intellectual)
                    {
                        geneDef.disabledWorkTags = WorkTags.Intellectual;
                    }
                    else if(skill == SkillDefOf.Cooking)
                    {
                        geneDef.disabledWorkTags = WorkTags.Cooking;
                    }
                    else if(skill == SkillDefOf.Medicine)
                    {
                        geneDef.disabledWorkTags = WorkTags.Caring;
                    }
                    else if(skill == SkillDefOf.Crafting)
                    {
                        geneDef.disabledWorkTags = WorkTags.Crafting;
                    }
                    else if(skill == SkillDefOf.Social)
                    {
                        geneDef.disabledWorkTags = WorkTags.Social;
                    }
                    else if(skill == SkillDefOf.Mining)
                    {
                        geneDef.disabledWorkTags = WorkTags.Mining;
                    }
                }
            }
            return geneDef;
            string ResolveDescription()
            {
                if (template.geneClass == typeof(Gene_ChemicalDependency))
                {
                    return template.description.Formatted(def.label, "PeriodDays".Translate(5f).Named("DEFICIENCYDURATION"), "PeriodDays".Translate(30f).Named("COMADURATION"), "PeriodDays".Translate(60f).Named("DEATHDURATION"));
                }
                return template.description.Formatted(def.label);
            }
        }
    }

    public class AndroidGeneTemplateDef : Def
    {
        public enum GeneTemplateType
        {
            Skill,
        }

        public Type geneClass = typeof(Gene);

        public int biostatCpx;

        public int biostatMet;

        public int aptitudeOffset;

        public WorkTags disabledWorkTags;

        public float addictionChanceFactor = 1f;

        public PassionMod.PassionModType passionModType;

        public GeneTemplateType geneTemplateType;

        public float minAgeActive;

        public GeneCategoryDef displayCategory;

        public int displayOrderOffset;

        public float selectionWeight = 1f;

        [MustTranslate]
        public string labelShortAdj;

        [NoTranslate]
        public string iconPath;

        [NoTranslate]
        public string exclusionTagPrefix;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }
            if (!typeof(Gene).IsAssignableFrom(geneClass))
            {
                yield return "geneClass is not Gene or child thereof.";
            }
        }
    }
}
