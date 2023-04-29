using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class JobDriver_ModifyAndroid : JobDriver
    {
        public Building_AndroidBehavioristStation Station => TargetA.Thing as Building_AndroidBehavioristStation;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (!pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            return true;
        }

        public override IEnumerable<Toil> MakeNewToils()
        {
            AddEndCondition(delegate
            {
                Thing thing = GetActor().jobs.curJob.GetTarget(TargetIndex.A).Thing;
                return thing.Spawned && thing is Building_AndroidBehavioristStation station && station.ReadyForModifying(pawn, out _) 
                ? JobCondition.Ongoing : JobCondition.Incompletable;
            });
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
            yield return new Toil
            {
                tickAction = delegate
                {
                    Station.DoWork(pawn, out bool workDone);
                    if (workDone)
                    {
                        Station.FinishAndroidProject();
                        this.EndJobWith(JobCondition.Succeeded);
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Never
            }.WithEffect(() => VREA_DefOf.VREA_ModifyingAndroidEffecter, TargetIndex.A).WithProgressBar(TargetIndex.A, () => (Station.currentWorkAmountDone / Station.totalWorkAmount));
        }
    }
}
