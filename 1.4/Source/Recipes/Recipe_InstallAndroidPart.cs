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
            var pawn = thing as Pawn;
            if (pawn is null || pawn.IsAndroid() is false)
            {
                return false;
            }
            if (this is not Recipe_InstallReactor)
            {
                var existingHediffs = pawn.health.hediffSet.hediffs.Where(x => x.part == part && x.def == recipe.addsHediff).ToList();
                if (existingHediffs.Any() && (part is null
                    || pawn.health.hediffSet.hediffs.OfType<Hediff_Injury>().Any(x => x.part == part) is false))
                {
                    return false;
                }
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
            for (int i = 0; i < part.parts.Count; i++)
            {
                RestorePartRecursiveInt(pawn.health, part.parts[i]);
            }
            Log.Message("Restoring " + part + " - recipe.addsHediff: " + recipe.addsHediff);
            pawn.health.AddHediff(recipe.addsHediff, part);
        }

        private static void RestorePartRecursiveInt(Pawn_HealthTracker __instance, BodyPartRecord part)
        {
            List<Hediff> hediffs = __instance.hediffSet.hediffs;
            for (int num = hediffs.Count - 1; num >= 0; num--)
            {
                Hediff hediff = hediffs[num];
                if (hediff.Part == part && hediff is Hediff_MissingPart && !hediff.def.keepOnBodyPartRestoration)
                {
                    hediffs.RemoveAt(num);
                    hediff.PostRemoved();
                    var androidPart = part.def.GetAndroidCounterPart();
                    if (androidPart != null)
                    {
                        var androidHediff = HediffMaker.MakeHediff(androidPart, __instance.pawn, part);
                        __instance.pawn.health.AddHediff(androidHediff, part);
                        Log.Message("Restoring " + part + " - androidHediff: " + androidHediff);
                    }
                }
            }
            for (int i = 0; i < part.parts.Count; i++)
            {
                RestorePartRecursiveInt(__instance, part.parts[i]);
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
    }
}
