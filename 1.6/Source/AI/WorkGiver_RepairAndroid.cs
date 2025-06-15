using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class WorkGiver_RepairAndroid : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Pawn);
        public override PathEndMode PathEndMode => PathEndMode.InteractionCell;
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
            return HasJobOn(pawn, t, forced);
        }
        public static bool HasJobOn(Pawn pawn, Thing t, bool forced)
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
            if (GoodLayingStatusForTend(pawn2, pawn, forced) is false)
            {
                return false;
            }
            if (pawn != pawn2)
            {
                if (pawn2.CurJobDef == VREA_DefOf.VREA_RepairAndroid)
                {
                    return false;
                }
                if (pawn2.HostileTo(pawn))
                {
                    return false;
                }
                if (t.IsForbidden(pawn))
                {
                    return false;
                }
                if (!pawn.CanReserveAndReach(t, PathEndMode.InteractionCell, Danger.Deadly))
                {
                    return false;
                }
            }
            if (!JobDriver_RepairAndroid.CanRepairAndroid(pawn2))
            {
                return false;
            }
            return true;
        }

        public static bool GoodLayingStatusForTend(Pawn patient, Pawn doctor, bool forced)
        {
            if (patient == doctor)
            {
                if (patient.playerSettings.selfTend is false)
                {
                    return false;
                }
                else if (forced is false && patient.InBed())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return patient.InBed();
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            return JobMaker.MakeJob(VREA_DefOf.VREA_RepairAndroid, t);
        }
    }
}
