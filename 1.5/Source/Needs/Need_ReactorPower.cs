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
            set
            {
                var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_Reactor) as Hediff_AndroidReactor;
                if (hediff != null)
                {
                    if (hediff.pawn is null)
                    {
                        hediff.pawn = pawn;
                    }
                    hediff.Energy = value;
                    curLevelInt = hediff.Energy;
                }
            }
        }

        public override void SetInitialLevel()
        {
            var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_Reactor) as Hediff_AndroidReactor;
            if (hediff != null)
            {
                curLevelInt = hediff.Energy;
            }
        }

        public override void NeedInterval()
        {

        }
    }
}
