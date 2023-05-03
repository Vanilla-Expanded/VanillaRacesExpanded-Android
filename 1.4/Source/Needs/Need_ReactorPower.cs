using RimWorld;
using Verse;

namespace VREAndroids
{
    public class Need_ReactorPower : Need
    {
        public Need_ReactorPower(Pawn pawn)
            : base(pawn)
        {

        }

        public override float CurLevel
        {
            get
            {
                var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_Reactor) as Hediff_AndroidReactor;
                if (hediff != null)
                {
                    return hediff.Energy;
                }
                return 0f;
            }
        }

        public override void NeedInterval()
        {

        }
    }
}
