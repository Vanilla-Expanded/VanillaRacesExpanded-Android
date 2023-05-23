using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    public class Recipe_InstallAndroidPart : Recipe_Surgery
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (thing is Pawn pawn && pawn.IsAndroid() is false)
            {
                return false;
            }
            return base.AvailableOnNow(thing, part);
        }
        public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
        {
            return MedicalRecipesUtility.GetFixedPartsToApplyOn(recipe, pawn, delegate (BodyPartRecord record)
            {
                if (record.parent != null && !pawn.health.hediffSet.GetNotMissingParts().Contains(record.parent))
                {
                    return false;
                }
                return (!pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(record) 
                || pawn.health.hediffSet.HasDirectlyAddedPartFor(record)) ? true : false;
            });
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (billDoer != null)
            {
                pawn.health.RestorePart(part);
                if (ModsConfig.IdeologyActive)
                {
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InstalledProsthetic, billDoer.Named(HistoryEventArgsNames.Doer)));
                }
            }
            else
            {
                pawn.health.RestorePart(part);
            }
            pawn.health.AddHediff(recipe.addsHediff, part);
        }

        public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
        {
            if ((pawn.Faction == billDoerFaction || pawn.Faction == null) && !pawn.IsQuestLodger())
            {
                return false;
            }
            if (recipe.addsHediff.addedPartProps != null && recipe.addsHediff.addedPartProps.betterThanNatural)
            {
                return false;
            }
            return HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
        }
    }
}
