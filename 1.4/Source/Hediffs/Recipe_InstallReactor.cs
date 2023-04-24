using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    public class Recipe_InstallReactor : Recipe_InstallArtificialBodyPart
    {
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            var reactor = ingredients.OfType<Reactor>().FirstOrDefault();
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_Reactor) as Hediff_AndroidReactor;
            hediff.curEnergy = reactor.curEnergy;
        }
    }
}
