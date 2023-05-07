using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class WorkGiver_CreateAndroid : WorkGiver_Scanner
    {
        public override PathEndMode PathEndMode => PathEndMode.InteractionCell;
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(VREA_DefOf.VREA_AndroidCreationStation);
        public override Danger MaxPathDanger(Pawn pawn) => Danger.Some;
        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if (pawn.CurJob?.def != VREA_DefOf.VREA_CreateAndroid && thing is Building_AndroidCreationStation station 
                && pawn.CanReserveAndReach(thing, PathEndMode.Touch, MaxPathDanger(pawn)) && (station.unfinishedAndroid is null 
                || pawn.CanReserveAndReach(station.unfinishedAndroid, PathEndMode.Touch, MaxPathDanger(pawn))))
            {
                if (!station.ReadyForAssembling(pawn, out var failReason))
                    JobFailReason.Is(failReason);
                else
                {
                    if (station.curAndroidProject != null && station.currentWorkAmountDone > 0)
                    {
                        var job = JobMaker.MakeJob(VREA_DefOf.VREA_CreateAndroid);
                        job.targetA = station;
                        return job;
                    }
                    else
                    {
                        var chosen = new List<ThingCount>();
                        var requiredIngredients = station.RequiredIngredients().ToList();
                        if (!AndroidCreationUtility.TryFindBestFixedIngredients(requiredIngredients, pawn, station, chosen))
                            JobFailReason.Is("VREA.MissingMaterials".Translate(string.Join(", ", requiredIngredients.Select(x => x.ToString()))));
                        else if (chosen.Any(x => !pawn.CanReserveAndReach(x.Thing, PathEndMode.ClosestTouch, MaxPathDanger(pawn))))
                            JobFailReason.Is("VREA.MissingMaterials".Translate(string.Join(", ", requiredIngredients.Select(x => x.ToString()))));
                        else
                        {
                            var job = JobMaker.MakeJob(VREA_DefOf.VREA_CreateAndroid);
                            job.targetA = station;
                            job.targetQueueB = new List<LocalTargetInfo>(chosen.Count);
                            job.countQueue = new List<int>(chosen.Count);
                            for (var i = 0; i < chosen.Count; i++)
                            {
                                job.targetQueueB.Add(chosen[i].Thing);
                                job.countQueue.Add(chosen[i].Count);
                            }
                            return job;
                        }
                    }
                }
            }
            return null;
        }
    }
}
