using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class JobDriver_FreeMemorySpace : JobDriver
    {
        public Building_AndroidStand AndroidStand => job.targetA.Thing as Building_AndroidStand;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            this.FailOn(() => AndroidStand.compPower.PowerOn is false
                || AndroidStand.CompAssignableToPawn.AssignedPawns.Contains(pawn) is false);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
            Toil toil = new Toil();
            toil.initAction = delegate
            {
                toil.actor.pather.StopDead();
            };
            toil.defaultCompleteMode = ToilCompleteMode.Never;
            toil.handlingFacing = true;
            toil.tickAction = delegate
            {
                toil.actor.Rotation = Rot4.South;
                var memorySpace = this.pawn.needs.TryGetNeed<Need_MemorySpace>();
                memorySpace.curLevelInt = Mathf.Min(1f, memorySpace.curLevelInt + (1f / 
                    (float)MentalState_Reformatting.TicksToRecoverFromReformatting(pawn)));
                if (memorySpace.curLevelInt == 1f)
                {
                    this.EndJobWith(JobCondition.Succeeded);
                }
            };
            yield return toil;
        }
    }
}
