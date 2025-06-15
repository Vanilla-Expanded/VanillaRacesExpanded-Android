using RimWorld;
using Verse;

namespace VREAndroids
{
    public class Gene_NeutroSynthesis : Gene
    {
        public override void Tick()
        {
            base.Tick();
            var neutroloss = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
            if (neutroloss != null)
            {
                var refillRate = 0.05f / (float)GenDate.TicksPerDay;
                neutroloss.Severity -= refillRate;
            }
        }
    }
}
