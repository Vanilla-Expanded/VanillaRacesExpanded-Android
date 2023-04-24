using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class WorkGiver_TendAndroid : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.InteractionCell;
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);
        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }

        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.SpawnedPawnsWithAnyHediff;
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn pawn2 = t as Pawn;
            if (pawn2 == null ||pawn2.IsAndroid() is false || pawn.WorkTypeIsDisabled(WorkTypeDefOf.Crafting)
                || !GoodLayingStatusForTend(pawn2, pawn) 
                || !pawn2.health.HasHediffsNeedingTendByPlayer()
                || !pawn.CanReserve(pawn2, 1, -1, null, forced) || (pawn2.InAggroMentalState))
            {
                return false;
            }
            return true;
        }

        public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor)
        {
            if (patient == doctor)
            {
                return true;
            }
            return patient.InBed();
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn pawn2 = t as Pawn;
            return JobMaker.MakeJob(VREA_DefOf.VREA_TendAndroid, pawn2);
        }
    }
}
