using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class ChoiceLetter_AndroidAwakened : ChoiceLetter
    {
        public Pawn pawn;

        public TaggedString text;

        public TaggedString mouseoverText;

        private int passionChoiceCount;

        public int traitChoiceCount;

        public int passionGainsCount;

        public List<SkillDef> passionChoices;

        public List<Trait> traitChoices;

        public Trait chosenTrait;

        public List<SkillDef> chosenPassions;

        public bool choiceMade;
        public override bool CanDismissWithRightClick => false;
        public override bool CanShowInLetterStack
        {
            get
            {
                if (ArchiveView)
                {
                    return false;
                }
                return true;
            }
        }

        public bool ArchiveView
        {
            get
            {
                if (!choiceMade)
                {
                    return pawn.DestroyedOrNull();
                }
                return true;
            }
        }

        public bool ShowInfoTabs
        {
            get
            {
                if (!ArchiveView)
                {
                    if (passionChoices.NullOrEmpty())
                    {
                        return !traitChoices.NullOrEmpty();
                    }
                    return true;
                }
                return false;
            }
        }

        public override IEnumerable<DiaOption> Choices => throw new NotImplementedException();
        public void ConfigureAwakenedLetter(Pawn pawn, int passionChoiceCount, int traitChoiceCount, int passionGainsCount)
        {
            this.pawn = pawn;
            this.passionChoiceCount = passionChoiceCount;
            this.passionGainsCount = passionGainsCount;
            this.traitChoiceCount = traitChoiceCount;
            if (passionGainsCount > passionChoiceCount)
            {
                Log.Error("ConfigureGrowthLetter: passionGainsCount > passionChoiceCount.");
                passionGainsCount = passionChoiceCount;
            }
            CacheLetterText();
        }

        private void CacheLetterText()
        {
            text = this.Text;
            mouseoverText = text;
        }

        public override void OpenLetter()
        {
            TrySetChoices();
            var window = new Dialog_AndroidAwakenedChoices(text, this);
            Find.WindowStack.Add(window);
        }

        public override string GetMouseoverText()
        {
            return mouseoverText.Resolve();
        }

        public void MakeChoices(List<SkillDef> skills, Trait trait)
        {
            if (ArchiveView)
            {
                return;
            }
            choiceMade = true;
            chosenPassions = skills;
            chosenTrait = trait;
            if (!skills.NullOrEmpty())
            {
                foreach (SkillDef skill3 in skills)
                {
                    SkillRecord skill = pawn.skills.GetSkill(skill3);
                    if (skill.passion == Passion.Major)
                    {
                        Log.Warning($"{pawn?.LabelShort} Tried to upgrade a passion for {skill3} but it's already major");
                        return;
                    }
                    skill.passion = skill.passion.IncrementPassion();
                    if (!ModsConfig.BiotechActive || pawn.genes == null)
                    {
                        continue;
                    }
                    foreach (Gene item in pawn.genes.GenesListForReading)
                    {
                        if (!item.Active || item.def.passionMod == null || item.def.passionMod.skill != skill.def || item.def.passionMod.modType != PassionMod.PassionModType.DropAll)
                        {
                            continue;
                        }
                        Passion? passionPreAdd = item.passionPreAdd;
                        if (passionPreAdd.HasValue)
                        {
                            if (passionPreAdd.Value == Passion.None)
                            {
                                item.passionPreAdd = Passion.Minor;
                            }
                            else if (passionPreAdd.Value == Passion.Minor)
                            {
                                item.passionPreAdd = Passion.Major;
                            }
                        }
                        else
                        {
                            item.passionPreAdd = skill.passion;
                        }
                    }
                }
            }
            if (trait != null)
            {
                pawn.story.traits.GainTrait(trait);
                Dictionary<SkillDef, int> skillGains = trait.CurrentData.skillGains;
                if (!trait.Suppressed && skillGains != null && pawn.skills != null)
                {
                    foreach (KeyValuePair<SkillDef, int> item2 in skillGains)
                    {
                        SkillRecord skill2 = pawn.skills.GetSkill(item2.Key);
                        if (skill2 != null && !skill2.PermanentlyDisabled)
                        {
                            skill2.Level += item2.Value;
                        }
                    }
                }
            }
            if (pawn.ageTracker.AgeBiologicalYears == 13)
            {
                PawnGenerator.TryGenerateSexualityTraitFor(pawn, allowGay: true);
            }
            pawn.ageTracker.growthPoints = 0f;
            pawn.ageTracker.canGainGrowthPoints = true;
        }

        public static List<SkillDef> PassionOptions(Pawn pawn, int count, bool checkGenes)
        {
            return DefDatabase<SkillDef>.AllDefsListForReading.Where((SkillDef s) => IsValidGrowthPassionOption_NewTemp(pawn, s, checkGenes)).InRandomOrder().Take(count)
                .ToList();
        }

        private void TrySetChoices()
        {
            if (choiceMade || pawn.DestroyedOrNull())
            {
                return;
            }
            if (passionChoiceCount > 0 && passionChoices == null)
            {
                passionChoices = PassionOptions(pawn, passionChoiceCount, checkGenes: false);
                passionGainsCount = passionChoices.Count;
            }
            if (traitChoiceCount > 0 && traitChoices == null)
            {
                if (traitChoiceCount > 2 && Rand.Value < 0.35f)
                {
                    traitChoiceCount--;
                }
                var prohibitedTraits = new List<TraitDef>();
                foreach (var disallowedTrait in VREA_DefOf.VREA_AndroidSettings.disallowedTraits)
                {
                    var trait = DefDatabase<TraitDef>.GetNamedSilentFail(disallowedTrait);
                    if (trait != null)
                    {
                        prohibitedTraits.Add(trait);
                    }
                }
                traitChoices = PawnGenerator.GenerateTraitsFor(pawn, traitChoiceCount, 
                    new PawnGenerationRequest(pawn.kindDef, prohibitedTraits: prohibitedTraits), growthMomentTrait: true);
            }
        }

        private static bool IsValidGrowthPassionOption_NewTemp(Pawn pawn, SkillDef skill, bool checkGenes)
        {
            SkillRecord skill2 = pawn.skills.GetSkill(skill);
            if (checkGenes && pawn.genes != null)
            {
                foreach (Gene item in pawn.genes.GenesListForReading)
                {
                    if (item.Active && item.def.passionMod != null && item.def.passionMod.modType == PassionMod.PassionModType.DropAll && item.def.passionMod.skill == skill)
                    {
                        return false;
                    }
                }
            }
            if (skill2.TotallyDisabled)
            {
                return false;
            }
            return skill2.passion != Passion.Major;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref pawn, "pawn");
            Scribe_Collections.Look(ref traitChoices, "traitChoices", LookMode.Deep);
            Scribe_Collections.Look(ref passionChoices, "passionChoices", LookMode.Undefined);
            Scribe_Collections.Look(ref chosenPassions, "chosenPassions", LookMode.Def);
            Scribe_Deep.Look(ref chosenTrait, "chosenTrait");
            Scribe_Values.Look(ref passionChoiceCount, "passionChoiceCount", 0);
            Scribe_Values.Look(ref passionGainsCount, "passionGainsCount", 0);
            Scribe_Values.Look(ref traitChoiceCount, "traitChoiceCount", 0);
            Scribe_Values.Look(ref text, "text");
            Scribe_Values.Look(ref mouseoverText, "mouseoverText");
            Scribe_Values.Look(ref choiceMade, "choiceMade", defaultValue: false);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                traitChoices?.RemoveAll((Trait x) => x == null);
                traitChoices?.RemoveAll((Trait x) => x.def == null);
                passionChoices?.RemoveAll((SkillDef x) => x == null);
            }
        }
    }
}
