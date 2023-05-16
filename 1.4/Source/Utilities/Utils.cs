using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [StaticConstructorOnStartup]
    public static class Utils
    {
        public static bool DubsMintMenusActive = ModsConfig.IsActive("Dubwise.DubsMintMenus");
        public static bool HasAndroidPartThingVariant(this BodyPartDef part)
        {
            return part != BodyPartDefOf.Torso && part != BodyPartDefOf.Brain
                && part != BodyPartDefOf.Head && part != VREA_DefOf.Skull;
        }

        private static HashSet<ThingDef> androidBeds = new HashSet<ThingDef>
        {
            VREA_DefOf.VREA_NeutroCasket, VREA_DefOf.VREA_AndroidStand, VREA_DefOf.VREA_AndroidStandSpot
        };
        public static bool IsAndroidBed(this ThingDef thingDef) => androidBeds.Contains(thingDef);
        [DebugAction("Pawns", null, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
        private static void AwakenAndroid(Pawn p)
        {
            var gene = p.genes?.GetGene(VREA_DefOf.VREA_SyntheticBody) as Gene_SyntheticBody;
            if (gene != null && gene.Awakened is false) 
            {
                gene.Awaken();
            }
        }

        public static HashSet<GeneDef> allAndroidGenes = new HashSet<GeneDef>();
        private static List<GeneDef> cachedGeneDefsInOrder = null;
        static Utils()
        {
            foreach (var race in DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.race != null && x.race.Humanlike))
            {
                race.recipes ??= new List<RecipeDef>();
                race.recipes.Add(VREA_DefOf.VREA_RemoveArtificialPart);
            }
            if (DubsMintMenusActive)
            {
                LongEventHandler.ExecuteWhenFinished(delegate
                {
                    if (DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch.Prepare())
                    {
                        VREAndroidsMod.harmony.Patch(DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch.TargetMethod(),
                            prefix: new HarmonyMethod(AccessTools.Method(typeof(DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch),
                            nameof(DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch.Prefix))));
                    }
                    else
                    {
                        Log.Error("[VREAndroids] Failed to patch Dubs Mint Menus");
                    }
                });

            }
        }
        public static List<GeneDef> AndroidGenesGenesInOrder
        {
            get
            {
                if (cachedGeneDefsInOrder == null)
                {
                    cachedGeneDefsInOrder = new List<GeneDef>();
                    foreach (GeneDef allDef in allAndroidGenes)
                    {
                        if (allDef.endogeneCategory != EndogeneCategory.Melanin)
                        {
                            cachedGeneDefsInOrder.Add(allDef);
                        }
                    }
                    cachedGeneDefsInOrder.SortBy((GeneDef x) => 0f - x.displayCategory.displayPriorityInXenotype, (GeneDef x) => x.displayCategory.label, (GeneDef x) => x.displayOrderInCategory);
                }
                return cachedGeneDefsInOrder;
            }
        }

        public static bool IsAndroidGene(this GeneDef geneDef)
        {
            return allAndroidGenes.Contains(geneDef);
        }

        public static bool CanBeRemovedFromAndroid(this GeneDef geneDef)
        {
            if (geneDef is AndroidGeneDef androidGeneDef && androidGeneDef.isCoreComponent)
            {
                return false;
            }
            return true;
        }
        public static bool HasActiveGene(this Pawn pawn, GeneDef geneDef)
        {
            if (pawn.genes is null) return false;
            return pawn.genes.GetGene(geneDef)?.Active ?? false;
        }

        public static bool IsHardware(this GeneDef geneDef)
        {
            if (geneDef.IsAndroidGene() is false)
                return false;
            return geneDef.IsSubroutine() is false;
        }
        public static bool IsSubroutine(this GeneDef geneDef)
        {
            return geneDef.displayCategory == VREA_DefOf.VREA_Subroutine;
        }

        public static Dictionary<BodyPartDef, HediffDef> cachedCounterParts = new Dictionary<BodyPartDef, HediffDef>();
        public static HediffDef GetAndroidCounterPart(this BodyPartDef bodyPart)
        {
            if (!cachedCounterParts.TryGetValue(bodyPart, out HediffDef hediffDef))
            {
                cachedCounterParts[bodyPart] = hediffDef = GetAndroidCounterPartInt(bodyPart);
            }
            return hediffDef;
        }
        private static HediffDef GetAndroidCounterPartInt(BodyPartDef bodyPart)
        {
            foreach (var recipe in DefDatabase<RecipeDef>.AllDefs)
            {
                if (recipe.addsHediff != null && recipe.appliedOnFixedBodyParts != null && recipe.appliedOnFixedBodyParts.Contains(bodyPart)
                    && typeof(Hediff_AndroidPart).IsAssignableFrom(recipe.addsHediff.hediffClass))
                {
                    return recipe.addsHediff;
                }
            }
            return null;
        }

        public static bool AndroidCanCatch(HediffDef hediffDef)
        {
            var extension = hediffDef.GetModExtension<AndroidSettingsExtension>();
            if (extension != null)
            {
                return extension.androidCanCatchIt;
            }
            if (hediffDef.tags != null)
            {
                if (hediffDef.tags.Contains("Sterilized"))
                {
                    return false;
                }
            }
            if (VREA_DefOf.VREA_AndroidSettings.androidsShouldNotReceiveHediffs.Contains(hediffDef.defName))
            {
                return false;
            }
            if (typeof(Hediff_Addiction).IsAssignableFrom(hediffDef.hediffClass)
                || DefDatabase<ChemicalDef>.AllDefs.Any(x => x.toleranceHediff == hediffDef)
                || typeof(Hediff_Psylink).IsAssignableFrom(hediffDef.hediffClass)
                || typeof(Hediff_High).IsAssignableFrom(hediffDef.hediffClass)
                || typeof(Hediff_Hangover).IsAssignableFrom(hediffDef.hediffClass)
                || hediffDef.chronic || hediffDef.CompProps<HediffCompProperties_Immunizable>() != null
                || hediffDef.makesSickThought)
            {
                return false;
            }
            return true;
        }

        public static Dictionary<Pawn_GeneTracker, bool> cachedPawnTypes = new();
        public static bool IsAndroid(this Pawn pawn)
        {
            if (pawn is null)
            {
                Log.Error("Checking for null pawn. It shouldn't happen.");
                return false;
            }
            if (pawn.genes is null) return false;
            if (!cachedPawnTypes.TryGetValue(pawn.genes, out var isAndroid))
            {
                if (pawn.genes.xenogenes.Count == 0 && pawn.genes.endogenes.Count == 0) return false;
                cachedPawnTypes[pawn.genes] = isAndroid = pawn.genes.GenesListForReading.Any(x => x.def.CanBeRemovedFromAndroid() is false);
            }
            return isAndroid;
        }

        public static void RecheckGenes(Pawn_GeneTracker __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                for (var i = 0; i < __instance.endogenes.Count; i++)
                {
                    var gene = __instance.endogenes[i];
                    if (gene.def.IsAndroidGene() is false)
                    {
                        var androidVariant = DefDatabase<GeneDef>.GetNamedSilentFail("VREA_" + gene.def.defName);
                        if (androidVariant != null)
                        {
                            __instance.endogenes[i] = GeneMaker.MakeGene(androidVariant, __instance.pawn);
                        }
                    }
                }

                for (var i = 0; i < __instance.xenogenes.Count; i++)
                {
                    var gene = __instance.xenogenes[i];
                    if (gene.def.IsAndroidGene() is false)
                    {
                        var androidVariant = DefDatabase<GeneDef>.GetNamedSilentFail("VREA_" + gene.def.defName);
                        if (androidVariant != null)
                        {
                            __instance.xenogenes[i] = GeneMaker.MakeGene(androidVariant, __instance.pawn);
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < __instance.endogenes.Count; i++)
                {
                    var gene = __instance.endogenes[i];
                    if (gene.def.IsAndroidGene())
                    {
                        var humanVariant = DefDatabase<GeneDef>.GetNamedSilentFail(gene.def.defName.Replace("VREA_", ""));
                        if (humanVariant != null)
                        {
                            __instance.endogenes[i] = GeneMaker.MakeGene(humanVariant, __instance.pawn);
                        }
                    }
                }

                for (var i = 0; i < __instance.xenogenes.Count; i++)
                {
                    var gene = __instance.xenogenes[i];
                    if (gene.def.IsAndroidGene())
                    {
                        var humanVariant = DefDatabase<GeneDef>.GetNamedSilentFail(gene.def.defName.Replace("VREA_", ""));
                        if (humanVariant != null)
                        {
                            __instance.xenogenes[i] = GeneMaker.MakeGene(humanVariant, __instance.pawn);
                        }
                    }
                }
            }
        }

        public static void TryAssignBackstory(Pawn pawn, string spawnCategory)
        {
            if (pawn.story.Childhood.spawnCategories?.Contains(spawnCategory) is false)
            {
                pawn.story.Childhood = DefDatabase<BackstoryDef>.AllDefs.Where(x => x.spawnCategories?.Contains(spawnCategory) ?? false).RandomElement();
            }
        }

        private static List<GeneDef> skinColorGenes;

        public static List<GeneDef> SkinColorAndroidGenesInOrder
        {
            get
            {
                if (skinColorGenes == null)
                {
                    skinColorGenes = new List<GeneDef>();
                    foreach (GeneDef allDef in DefDatabase<GeneDef>.AllDefs)
                    {
                        if ((allDef.endogeneCategory == EndogeneCategory.Melanin || !(allDef.minMelanin >= 0f)) && allDef.skinColorBase.HasValue)
                        {
                            if (allDef.IsAndroidGene())
                            {
                                skinColorGenes.Add(allDef);
                            }
                        }
                    }
                    skinColorGenes.SortBy((GeneDef x) => x.minMelanin);
                }
                return skinColorGenes;
            }
        }

        private static List<GeneDef> cachedHairColorGenes;

        public static List<GeneDef> HairColorAndroidGenes
        {
            get
            {
                if (cachedHairColorGenes == null)
                {
                    cachedHairColorGenes = DefDatabase<GeneDef>.AllDefs.Where((GeneDef x) => x.hairColorOverride.HasValue && x.IsAndroidGene()).ToList();
                }
                return cachedHairColorGenes;
            }
        }


        public static bool IsAndroid(this Pawn pawn, out Gene_SyntheticBody gene_SyntheticBody)
        {
            if (pawn is null)
            {
                Log.Error("Checking for null pawn. It shouldn't happen.");
                gene_SyntheticBody = null;
                return false;
            }
            gene_SyntheticBody = pawn.genes?.GetGene(VREA_DefOf.VREA_SyntheticBody) as Gene_SyntheticBody;
            return gene_SyntheticBody != null;
        }

        public static void TrySpawnWaste(this Pawn pawn, IntVec3 pos, Map map)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_ZeroWaste) is false)
            {
                var wasteCount = pawn.HasActiveGene(VREA_DefOf.VREA_ExtraWaste) ? 15 : 5;
                var wastepack = ThingMaker.MakeThing(ThingDefOf.Wastepack);
                wastepack.stackCount = wasteCount;
                GenSpawn.Spawn(wastepack, pos, map);
            }
        }

        public static bool IsAndroidType(this XenotypeDef def)
        {
            return def.genes.Count > 0 && def.genes.All(x => x is AndroidGeneDef androidGeneDef && androidGeneDef.isCoreComponent);
        }
        public static bool IsAndroidType(this CustomXenotype def)
        {
            return def.genes.Count > 0 && def.genes.All(x => x is AndroidGeneDef androidGeneDef && androidGeneDef.isCoreComponent);
        }
        public static RecipeDef RecipeForAndroid(this RecipeDef originalRecipe)
        {
            if (originalRecipe.workSkill != SkillDefOf.Crafting)
            {
                var recipe = originalRecipe.Clone() as RecipeDef;
                recipe.effectWorking = VREA_DefOf.ButcherMechanoid;
                recipe.soundWorking = VREA_DefOf.Recipe_Machining;
                recipe.workSpeedStat = VREA_DefOf.ButcheryMechanoidSpeed;
                recipe.workSkill = SkillDefOf.Crafting;
                if (recipe.skillRequirements != null)
                {
                    recipe.skillRequirements = new List<SkillRequirement>();
                    foreach (var skillReq in originalRecipe.skillRequirements)
                    {
                        if (skillReq.skill == SkillDefOf.Medicine)
                        {
                            recipe.skillRequirements.Add(new SkillRequirement { minLevel = skillReq.minLevel, skill = SkillDefOf.Crafting });
                        }
                        else
                        {
                            recipe.skillRequirements.Add(new SkillRequirement { minLevel = skillReq.minLevel, skill = skillReq.skill });
                        }
                    }
                }
                recipe.ingredients = recipe.ingredients.Where(x => (x.filter?.categories != null
                    && x.filter.categories.Contains("Medicine")) is false).ToList();
                return recipe;
            }
            return originalRecipe;
        }

        public static bool Emotionless(this Pawn pawn)
        {
            return pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled) && !pawn.HasActiveGene(VREA_DefOf.VREA_EmotionSimulators);
        }
        public static object Clone(this object obj)
        {
            var cloneMethod = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            return cloneMethod.Invoke(obj, null);
        }
    }
}
