﻿using HarmonyLib;
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
        public static HashSet<HediffDef> autogeneratedAndroidPartHediffs = new HashSet<HediffDef>();
        public static void Prefix()
        {
            var humanlikes = DefDatabase<ThingDef>.AllDefs.Where(x => x.race?.Humanlike ?? false).ToList();
            var bodyPartDefs = new HashSet<BodyPartDef>();
            GetBodyPartsRecursive(ThingDefOf.Human.race.body.corePart);
            void GetBodyPartsRecursive(BodyPartRecord partRecord)
            {
                bodyPartDefs.Add(partRecord.def);
                foreach (var child in partRecord.parts)
                {
                    GetBodyPartsRecursive(child);
                }
            }
            foreach (var bodyPartDef in bodyPartDefs)
            {
                if (DefDatabase<HediffDef>.GetNamedSilentFail("VREA_" + bodyPartDef.defName) is null)
                {
                    var androidCounterPart = bodyPartDef.GetAndroidCounterPart();
                    if (androidCounterPart is null)
                    {
                        var androidPartThingDef = bodyPartDef.HasAndroidPartThingVariant() ? GetAndroidPartThing(bodyPartDef) : null;
                        var androidPartHediffDef = GetAndroidPartHediff(bodyPartDef, androidPartThingDef);
                        if (androidPartThingDef != null)
                        {
                            var androidPartRecipeDef = GetAndroidPartRecipe(humanlikes, androidPartThingDef, androidPartHediffDef, bodyPartDef);
                            DefGenerator.AddImpliedDef(androidPartThingDef);
                            DefGenerator.AddImpliedDef(androidPartRecipeDef);
                        }

                        DefGenerator.AddImpliedDef(androidPartHediffDef);
                        Utils.cachedCounterParts[bodyPartDef] = androidPartHediffDef;
                        autogeneratedAndroidPartHediffs.Add(androidPartHediffDef);
                    }
                }
            }
        }

        public static ThingDef GetAndroidPartThing(BodyPartDef bodyPartDef)
        {
            var digitsReplacements = new Dictionary<char, string>
            { 
                {'0', "ZERO"},
                {'1',"ONE"},
                {'2', "TWO"},
                {'3', "THREE"},
                {'4', "FOUR"},
                {'5', "FIVE"},
                {'6', "SIX"},
                {'7', "SEVEN"},
                {'8',"EIGHT"},
                { '9', "NINE"},
            };
            var defname = "VREA_" + bodyPartDef.defName;
            for (int i = defname.Length - 1; i > 0; i--)
            {
                if (digitsReplacements.TryGetValue(defname[i], out var replacement))
                {
                    defname = defname.Remove(i, 1);
                    defname = defname.Insert(i, replacement);
                    i = defname.Length - 1;
                }
            }
            return new ThingDef
            {
                defName = defname,
                label = "VREA.Artificial".Translate().ToLower() + " " + bodyPartDef.label,
                description = "VREA.ArtificialDesc".Translate() + " " + bodyPartDef.label + ".",
                costList = new List<ThingDefCountClass>
                {
                    new ThingDefCountClass { thingDef = ThingDefOf.Plasteel, count = bodyPartDef.hitPoints / 4},
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
                    texPath = "Items/HealthItemAndroid",
                    graphicClass = typeof(Graphic_Single),
                    drawSize = new Vector2(0.80f, 0.80f),
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
                    researchPrerequisite = VREA_DefOf.VREA_AndroidTech,
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
            var label = "VREA.Artificial".Translate().ToLower() + " " + bodyPartDef.label;
            var hediffDef = new HediffDef
            {
                defName = "VREA_" + bodyPartDef.defName,
                label = label,
                description = "VREA.AnInstalled".Translate() + " " + label,
                hediffClass = typeof(Hediff_AndroidPart),
                defaultLabelColor = new Color(0.6f, 0.6f, 1.0f),
                isBad = false,
                countsAsAddedPartOrImplant = true,
                addedPartProps = new AddedBodyPartProps
                {
                    solid = bodyPartDef.solid,
                    partEfficiency = 1f,
                }
            };
            if (ingredient != null)
            {
                hediffDef.descriptionHyperlinks = new List<DefHyperlink>
                {
                    new DefHyperlink{ def = ingredient}
                };
                hediffDef.spawnThingOnRemoved = ingredient;
            }
            return hediffDef;
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
                uiIconThing = ingredient,
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
                workerClass = typeof(Recipe_InstallAndroidPart),
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
