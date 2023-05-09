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
                race.recipes.Add(VREA_DefOf.VREA_RemoveArtificalPart);
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
            if (extension != null && extension.androidCanCatchIt)
            {
                return true;
            }
            return VREA_DefOf.VREA_AndroidSettings.androidsShouldNotReceiveHediffs.Contains(hediffDef.defName) is false
                && (typeof(Hediff_Addiction).IsAssignableFrom(hediffDef.hediffClass)
                || DefDatabase<ChemicalDef>.AllDefs.Any(x => x.toleranceHediff == hediffDef)
                || typeof(Hediff_Psylink).IsAssignableFrom(hediffDef.hediffClass)
                || typeof(Hediff_High).IsAssignableFrom(hediffDef.hediffClass)
                || typeof(Hediff_Hangover).IsAssignableFrom(hediffDef.hediffClass)
                || hediffDef.chronic || hediffDef.CompProps<HediffCompProperties_Immunizable>() != null
                || hediffDef.makesSickThought) is false;
        }

        public static bool IsAndroid(this Pawn pawn)
        {
            if (pawn is null)
            {
                Log.Error("Checking for null pawn. It shouldn't happen.");
                return false;
            }
            return HasActiveGene(pawn, VREA_DefOf.VREA_SyntheticBody);
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
            return def.genes.Count > 0 && def.genes.All(x => x.IsAndroidGene());
        }
        public static bool IsAndroidType(this CustomXenotype def)
        {
            return def.genes.Count > 0 && def.genes.All(x => x.IsAndroidGene());
        }
        public static void ChangeRecipeForAndroid(this Bill_Medical __instance)
        {
            var recipe = __instance.recipe.Clone() as RecipeDef;
            recipe.effectWorking = VREA_DefOf.ButcherMechanoid;
            recipe.soundWorking = VREA_DefOf.Recipe_Machining;
            recipe.workSpeedStat = VREA_DefOf.ButcheryMechanoidSpeed;
            recipe.workSkill = SkillDefOf.Crafting;
            recipe.skillRequirements = new List<SkillRequirement>();
            foreach (var skillReq in __instance.recipe.skillRequirements)
            {
                if (skillReq.skill == SkillDefOf.Medicine)
                {
                    recipe.skillRequirements.Add(new SkillRequirement { minLevel= skillReq.minLevel, skill = SkillDefOf.Crafting });
                }
                else
                {
                    recipe.skillRequirements.Add(new SkillRequirement { minLevel = skillReq.minLevel, skill = skillReq.skill });
                }
            }

            recipe.ingredients = recipe.ingredients.Where(x => (x.filter?.categories != null
                && x.filter.categories.Contains("Medicine")) is false).ToList();
            __instance.recipe = recipe;
        }

        public static object Clone(this object obj)
        {
            var cloneMethod = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
            return cloneMethod.Invoke(obj, null);
        }
    }
}
