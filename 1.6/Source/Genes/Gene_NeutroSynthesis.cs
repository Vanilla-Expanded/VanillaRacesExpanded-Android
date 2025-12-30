using RimWorld;
using Verse;

namespace VREAndroids
{
    public class Gene_NeutroSynthesis : Gene
    {
        public override void TickInterval(int delta)
        {
            base.TickInterval(delta);
        
            if (pawn.IsHashIntervalTick(60, delta))
            {
                var neutroloss = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
                if (neutroloss != null)
                {
                    var refillRate = 0.05f / GenDate.TicksPerDay * 60f;
                    neutroloss.Severity -= refillRate;
                }
            }
        }
    }
}
