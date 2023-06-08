using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    public class Gene_SyntheticBody : Gene
    {
        public NameTriple storedTripleName;

        public bool autoRepair = true;
        public override void PostAdd()
        {
            base.PostAdd();
            foreach (var bodyPart in this.pawn.def.race.body.AllParts.OrderByDescending(x => x.Index))
            {
                var hediffDef = bodyPart.def.GetAndroidCounterPart();
                if (hediffDef != null && this.pawn.health.hediffSet.GetNotMissingParts().Contains(bodyPart))
                {
                    var hediff = HediffMaker.MakeHediff(hediffDef, pawn, bodyPart);
                    pawn.health.AddHediff(hediff, bodyPart);
                }
            }
            MeditationFocusTypeAvailabilityCache.ClearFor(pawn);
        }

        public override void Tick()
        {
            base.Tick();
            pawn.needs.AddOrRemoveNeedsAsAppropriate();
            if (pawn.IsHashIntervalTick(GenDate.TicksPerHour) && pawn.IsAwakened() is false 
                && pawn.HasActiveGene(VREA_DefOf.VREA_AntiAwakeningProtocols) is false)
            {
                if (pawn.needs.mood.CurLevel <= 0.05f && Rand.Chance(0.01f))
                {
                    Awaken("VREA.AndroidAwakening".Translate(pawn.Named("PAWN")), "VREA.AndroidAwakeningLowMood".Translate(pawn.Named("PAWN")));
                    var gene = pawn.genes.GetGene(VREA_DefOf.VREA_CombatIncapability);
                    if (gene != null)
                    {
                        pawn.genes.RemoveGene(gene);
                    }
                    pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk);
                }
                if (pawn.needs.mood.CurLevel >= 0.8f && Rand.Chance(0.01f))
                {
                    Awaken("VREA.AndroidAwakening".Translate(pawn.Named("PAWN")), "VREA.AndroidAwakeningHighMood".Translate(pawn.Named("PAWN")));
                    InspirationDef randomAvailableInspirationDef = pawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef();
                    if (randomAvailableInspirationDef != null)
                    {
                        pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, "LetterInspirationBeginThanksToHighMoodPart".Translate());
                    }
                }
            }
        }

        public void Awaken(TaggedString title, TaggedString description)
        {
            var letter = (ChoiceLetter_AndroidAwakened)LetterMaker.MakeLetter(VREA_DefOf.VREA_AndroidAwakenedLetter);
            letter.Label = title;
            letter.Text = description;
            letter.ConfigureAwakenedLetter(pawn, 8, 6, 4);
            Find.LetterStack.ReceiveLetter(letter);

            foreach (var gene in pawn.genes.GenesListForReading.ToList())
            {
                if (gene.def is AndroidGeneDef geneDef && geneDef.removeWhenAwakened)
                {
                    pawn.genes.RemoveGene(gene);
                }
            }
            if (storedTripleName != null)
            {
                pawn.Name = storedTripleName;
            }
            MoteMaker.MakeColonistActionOverlay(pawn, VREA_DefOf.VREA_AndroidAwakenedMote);
            VREA_DefOf.VREA_AndroidAwakenedEffect.SpawnAttached(pawn, pawn.Map);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref storedTripleName, "storedTripleName");
            Scribe_Values.Look(ref autoRepair, "autoRepair", true);
        }
    }
}
