using RimWorld;
using Verse;

namespace VREAndroids
{
    public class WorkGiver_CarryToAndroidBehavioristStation : WorkGiver_CarryToBuilding
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(VREA_DefOf.VREA_AndroidBehavioristStation);

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
