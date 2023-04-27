using RimWorld;
using System.Linq;
using Verse;

namespace VREAndroids
{
    public class Gene_SyntheticBody : Gene
    {
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
    }
}
