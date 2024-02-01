using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            //pawn.health.AddHediff(recipe.addsHediff, part);
            foreach (BodyPartRecord child in GetAllChildren(part))
            {
                pawn.health.RestorePart(child);
                pawn.health.AddHediff(Utils.GetAndroidCounterPart(child.def), child);
            }
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

        public static IEnumerable<BodyPartRecord> GetAllChildren(BodyPartRecord part)
        {
            yield return part;
            foreach (BodyPartRecord part2 in part.parts)
            {
                foreach (BodyPartRecord part3 in GetAllChildren(part2))
                {
                    yield return part3;
                }
            }
            yield break;
        }
        public static bool IsChildrenClean(Pawn pawn, BodyPartRecord part)
        {
            foreach (BodyPartRecord child in GetAllChildren(part))
            {
                if (pawn.health.hediffSet.hediffs.Where((Hediff x) => x.Part == part && x.def != Utils.GetAndroidCounterPart(part.def)).Any() || pawn.health.hediffSet.PartIsMissing(child))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
