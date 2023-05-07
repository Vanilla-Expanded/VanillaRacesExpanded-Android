using RimWorld;
using Verse;

namespace VREAndroids
{
    public class WorkGiver_CarryToSubcorePolyanalyzer : WorkGiver_CarryToBuilding
    {
        public override ThingRequest ThingRequest => ThingRequest.ForDef(VREA_DefOf.VREA_SubcorePolyanalyzer);
        public override bool ShouldSkip(Pawn pawn, bool forced = false)
        {
            if (!base.ShouldSkip(pawn, forced))
            {
                return !ModsConfig.BiotechActive;
            }
            return true;
        }
    }
}
