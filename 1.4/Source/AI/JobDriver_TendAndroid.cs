using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace VREAndroids
{
    public class JobDriver_TendAndroid : JobDriver
    {
        protected Pawn Deliveree => job.targetA.Pawn;
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            if (Deliveree != pawn && !pawn.Reserve(Deliveree, job, 1, -1, null, errorOnFailed))
            {
                return false;
            }
            return true;
        }
        public override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            AddEndCondition(delegate
            {
                return Deliveree.health.HasHediffsNeedingTend() ? JobCondition.Ongoing : JobCondition.Succeeded;
            });
            this.FailOnAggroMentalState(TargetIndex.A);
            PathEndMode interactionCell = PathEndMode.None;
            if (Deliveree == pawn)
            {
                interactionCell = PathEndMode.OnCell;
            }
            else if (Deliveree.InBed())
            {
                interactionCell = PathEndMode.InteractionCell;
            }
            else if (Deliveree != pawn)
            {
                interactionCell = PathEndMode.ClosestTouch;
            }
            Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
            yield return gotoToil;
 
        }
    }
}
