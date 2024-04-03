using Verse;

namespace VREAndroids
{
    public class Hediff_Neutroloss : HediffWithComps
    {
        public override float Severity 
        { 
            get => base.Severity;
            set
            {
                pawn.health.hediffSet.DirtyCache();
                base.Severity = value;
            }
        }

        public override void PostRemoved()
        {
            pawn.health.hediffSet.DirtyCache();
            base.PostRemoved();
        }
    }
}
