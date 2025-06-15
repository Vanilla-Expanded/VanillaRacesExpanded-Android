using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Recipe_AdministerNeutroamineForAndroid : Recipe_AdministerIngestible
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            if (thing is Pawn pawn && pawn.IsAndroid() is false)
            {
                return false;
            }
            return base.AvailableOnNow(thing, part);
        }

        public override float GetIngredientCount(IngredientCount ing, Bill bill)
        {
            Pawn pawn = bill.billStack?.billGiver as Pawn;
            if (pawn == null)
            {
                return base.GetIngredientCount(ing, bill);
            }
            Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
            if (firstHediffOfDef == null)
            {
                return base.GetIngredientCount(ing, bill);
            }
            return Mathf.Min(bill.Map.listerThings.ThingsOfDef(VREA_DefOf.Neutroamine).Sum((Thing x) => x.stackCount), firstHediffOfDef.Severity / 0.01f);
        }

        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            var neutroamine = ingredients.Where(x => x.def == VREA_DefOf.Neutroamine).Sum(x => x.stackCount);
            var neutroloss = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
            if (neutroloss != null)
            {
                neutroloss.Severity -= neutroamine / 100f;
                if (neutroloss.Severity <= 0.01f)
                {
                    neutroloss.Severity = 0;
                }
            }
            ingredients.ForEach(x => x.Destroy());
        }
    }
}
