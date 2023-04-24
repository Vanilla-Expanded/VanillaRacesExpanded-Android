using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve")]
    public static class DefGenerator_GenerateImpliedDefs_PreResolve_Patch
    {
        public static void Prefix()
        {
            var bodyPartDefs = new HashSet<BodyPartDef>();
            var bodyPartsPerRace = new Dictionary<ThingDef, HashSet<BodyPartDef>>();
            foreach (var def in DefDatabase<ThingDef>.AllDefs)
            {
                if (def.race != null && def.race.Humanlike)
                {
                    var curBodyParts = new HashSet<BodyPartDef>();
                    GetBodyPartsRecursive(def.race.body.corePart);
                    void GetBodyPartsRecursive(BodyPartRecord partRecord)
                    {
                        curBodyParts.Add(partRecord.def);
                        foreach (var child in partRecord.parts)
                        {
                            GetBodyPartsRecursive(child);
                        }
                    }
                    bodyPartDefs.AddRange(curBodyParts);
                    bodyPartsPerRace[def] = curBodyParts;
                }
            }

            foreach (var bodyPartDef in bodyPartDefs)
            {
                var androidCounterPart = bodyPartDef.GetAndroidCounterPart();
                if (androidCounterPart is null)
                {
                    var androidPartThingDef = GetAndroidPartThing(bodyPartDef);
                    var androidPartHediffDef = GetAndroidPartHediff(bodyPartDef, androidPartThingDef);
                    var recipeUsers = new List<ThingDef>();
                    foreach (var kvp in bodyPartsPerRace)
                    {
                        if (kvp.Value.Contains(bodyPartDef))
                        {
                            recipeUsers.Add(kvp.Key);
                        }
                    }
                    var androidPartRecipeDef = GetAndroidPartRecipe(recipeUsers, androidPartThingDef, androidPartHediffDef, bodyPartDef);
                    DefGenerator.AddImpliedDef(androidPartThingDef);
                    DefGenerator.AddImpliedDef(androidPartHediffDef);
                    DefGenerator.AddImpliedDef(androidPartRecipeDef);
                    Utils.cachedCounterParts[bodyPartDef] = androidPartHediffDef;
                }
            }
        }

        public static ThingDef GetAndroidPartThing(BodyPartDef bodyPartDef)
        {
            return new ThingDef
            {
                defName = "VREA_" + bodyPartDef.defName,
                label = "VREA.Artifical".Translate().ToLower() + " " + bodyPartDef.label,
                description = "VREA.ArtificalDesc".Translate() + " " + bodyPartDef.label,
                costList = new List<ThingDefCountClass>
                {
                    new ThingDefCountClass { thingDef = ThingDefOf.Plasteel, count = bodyPartDef.hitPoints / 2},
                },
                thingClass = typeof(ThingWithComps),
                category = ThingCategory.Item,
                drawerType  = DrawerType.MapMeshOnly,
                useHitPoints = true,
                selectable = true,
                altitudeLayer = AltitudeLayer.Item,
                tickerType = TickerType.Never,
                alwaysHaulable = true,
                isTechHediff = true,
                pathCost = 14,
                allowedArchonexusCount = 1,
                techLevel = TechLevel.Ultra,
                thingCategories = new List<ThingCategoryDef>
                {
                    VREA_DefOf.VREA_BodyPartsAndroid,
                },
                graphicData = new GraphicData
                {
                    texPath = "Things/Item/Health/HealthItem",
                    graphicClass = typeof(Graphic_Single),
                    drawSize = new Vector2(0.80f, 0.80f),
                    color = new ColorInt(189, 169, 118).ToColor
                },
                tradeTags = new List<string>
                {
                    "TechHediff",
                    "BodyPartsAndroid"
                },
                techHediffsTags = new List<string>
                {
                    "BodyPartsAndroid"
                },
                statBases = new List<StatModifier>
                {
                    new StatModifier { stat = StatDefOf.WorkToMake, value = 26000 },
                    new StatModifier { stat = StatDefOf.MaxHitPoints, value = 50 },
                    new StatModifier { stat = StatDefOf.Flammability, value = 0.7f },
                    new StatModifier { stat = StatDefOf.Beauty, value = -4 },
                    new StatModifier { stat = StatDefOf.DeteriorationRate, value = 2.0f },
                    new StatModifier { stat = StatDefOf.Mass, value = 1 },
                },
                recipeMaker = new RecipeMakerProperties
                {
                    workSpeedStat = StatDefOf.GeneralLaborSpeed,
                    workSkill = SkillDefOf.Crafting,
                    effectWorking = VREA_DefOf.Smith,
                    soundWorking = VREA_DefOf.Recipe_Smith,
                    unfinishedThingDef = VREA_DefOf.VREA_UnfinishedHealthItemAndroid,
                    skillRequirements = new List<SkillRequirement>
                    {
                        new SkillRequirement{ skill = SkillDefOf.Crafting, minLevel = 8}
                    },
                    researchPrerequisite = VREA_DefOf.VREA_AndroidCreation,
                    recipeUsers = new List<ThingDef>
                    {
                        VREA_DefOf.VREA_AndroidPartWorkbench
                    }
                },
                comps = new List<CompProperties>
                {
                    new CompProperties_Forbiddable()
                }
            };
        }

        public static HediffDef GetAndroidPartHediff(BodyPartDef bodyPartDef, ThingDef ingredient)
        {
            return new HediffDef
            {
                defName = "VREA_" + bodyPartDef.defName,
                label = ingredient.label,
                description = "VREA.AnInstalled".Translate() + " " + ingredient.label,
                descriptionHyperlinks = new List<DefHyperlink>
                {
                    new DefHyperlink{ def = ingredient}
                },
                spawnThingOnRemoved = ingredient,
                hediffClass = typeof(Hediff_AndroidPart),
                defaultLabelColor = new Color(0.6f, 0.6f, 1.0f),
                isBad = false,
                countsAsAddedPartOrImplant = true,
                addedPartProps = new AddedBodyPartProps
                {
                    solid = true,
                    partEfficiency = 1f,
                }
            };
        }

        public static RecipeDef GetAndroidPartRecipe(List<ThingDef> recipeUsers, ThingDef ingredient, HediffDef hediffDef, BodyPartDef bodyPart)
        {
            return new RecipeDef
            {
                defName = "VREA_Install" + bodyPart.defName,
                label = "VREA.Install".Translate().ToLower() + " " + ingredient.label,
                description = "VREA.Install".Translate() + " " + ingredient.label,
                ingredients = new List<IngredientCount>
                {
                    new IngredientCount
                    {
                        filter = new ThingFilter
                        {
                            thingDefs = new List<ThingDef>
                            {
                                ingredient
                            }
                        },
                        count = 1
                    }
                },
                fixedIngredientFilter = new ThingFilter
                {
                    thingDefs = new List<ThingDef>
                    {
                        ingredient
                    }
                },
                appliedOnFixedBodyParts = new List<BodyPartDef>
                {
                    bodyPart
                },
                addsHediff = hediffDef,
                workerClass = typeof(Recipe_InstallArtificialBodyPart),
                workAmount = 2500,
                recipeUsers = recipeUsers,
                effectWorking = VREA_DefOf.ButcherMechanoid,
                soundWorking = VREA_DefOf.Recipe_ButcherCorpseMechanoid,
                workSkill = SkillDefOf.Crafting,
                skillRequirements = new List<SkillRequirement>
                {
                    new SkillRequirement { skill = SkillDefOf.Crafting, minLevel = 5 }
                },
                jobString = "VREA.Installing".Translate() + " " + ingredient.label,
                descriptionHyperlinks = new List<DefHyperlink>
                {
                    new DefHyperlink { def = ingredient}, new DefHyperlink{ def = hediffDef}
                },
            };
        }
    }
}
