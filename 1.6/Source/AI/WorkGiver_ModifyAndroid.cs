using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class WorkGiver_ModifyAndroid : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.InteractionCell;
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(VREA_DefOf.VREA_AndroidBehavioristStation);
        public override Danger MaxPathDanger(Pawn pawn) => Danger.Some;
        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if (pawn.CurJob?.def != VREA_DefOf.VREA_ModifyAndroid &&  thing is Building_AndroidBehavioristStation station 
                && pawn.CanReserveAndReach(thing, PathEndMode.Touch, MaxPathDanger(pawn)))
            {
                if (station.curAndroidProject != null && station.Occupant != null)
                {
                    var job = JobMaker.MakeJob(VREA_DefOf.VREA_ModifyAndroid);
                    job.targetA = station;
                    return job;
                }
            }
            return null;
        }
    }
}
