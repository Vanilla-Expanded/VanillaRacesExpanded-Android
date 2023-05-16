using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class WorkGiver_RepairAndroid : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);
        public override PathEndMode PathEndMode => PathEndMode.Touch;
        public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
        {
            return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
        }
        public override Danger MaxPathDanger(Pawn pawn)
        {
            return Danger.Deadly;
        }
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn pawn2 = (Pawn)t;
            if (pawn2 is null || pawn2.IsAndroid(out var gene) is false)
            {
                return false;
            }
            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Crafting))
            {
                return false;
            }
            if (gene.autoRepair is false)
            {
                return false;
            }
            if (GoodLayingStatusForTend(pawn2, pawn) is false)
            {
                return false;
            }
            if (pawn2.InAggroMentalState || pawn2.HostileTo(pawn))
            {
                return false;
            }
            if (t.IsForbidden(pawn))
            {
                return false;
            }
            if (!pawn.CanReserve(t, 1, -1, null, forced))
            {
                return false;
            }
            if (pawn2.IsBurning())
            {
                return false;
            }
            if (pawn2.IsAttacking())
            {
                return false;
            }
            if (!JobDriver_RepairAndroid.CanRepairAndroid(pawn2))
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
            return JobMaker.MakeJob(VREA_DefOf.VREA_RepairAndroid, t);
        }
    }
}
