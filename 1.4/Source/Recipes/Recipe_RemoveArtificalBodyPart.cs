﻿using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Verse;

namespace VREAndroids
{
    public class Recipe_RemoveArtificialBodyPart : Recipe_RemoveBodyPart
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
            IEnumerable<BodyPartRecord> notMissingParts = pawn.health.hediffSet.GetNotMissingParts();
            foreach (BodyPartRecord part in notMissingParts)
            {
                if (part.def.HasAndroidPartThingVariant() is false)
                    continue;
                
                if (pawn.health.hediffSet.hediffs.Any((Hediff d) => (d is Hediff_Injury || d.IsPermanent() || d.def.spawnThingOnRemoved is null) && d.Part == part) is false)
                {
                    yield return part;
                }
                else if (part.def.forceAlwaysRemovable)
                {
                    yield return part;
                }
            }
        }
        
        public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
        {
            if ((pawn.Faction == billDoerFaction || pawn.Faction == null) && !pawn.IsQuestLodger())
            {
                return false;
            }
            if (HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest)
            {
                return true;
            }
            return false;
        }
        
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            bool flag = MedicalRecipesUtility.IsClean(pawn, part);
            bool flag2 = IsViolationOnPawn(pawn, part, Faction.OfPlayer);
            if (billDoer != null)
            {
                TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
                if (SpawnPartsWhenRemoved && Recipe_InstallAndroidPart.IsChildrenClean(pawn, part))
                {
                    MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
                    MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
                }
            }
            DamagePart(pawn, part);
            if (flag)
            {
                ApplyThoughts(pawn, billDoer);
            }
            if (flag2)
            {
                ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
            }
        }

        public override void ApplyThoughts(Pawn pawn, Pawn billDoer)
        {

        }

        public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
        {
            return recipe.label;
        }
    }
}
