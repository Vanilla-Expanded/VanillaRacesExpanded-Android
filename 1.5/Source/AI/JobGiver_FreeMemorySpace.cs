using RimWorld;
using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace VREAndroids
{
    public class JobGiver_FreeMemorySpace : ThinkNode_JobGiver
    {
        public const float FreeMemorySpaceThreshold = 0.2f;
        public override float GetPriority(Pawn pawn)
        {
            var memorySpace = pawn.needs.TryGetNeed<Need_MemorySpace>();
            if (memorySpace == null || memorySpace.CurLevelPercentage > FreeMemorySpaceThreshold)
            {
                return 0f;
            }
            return 999f;
        }

        public override Job TryGiveJob(Pawn pawn)
        {
            var memorySpace = pawn.needs.TryGetNeed<Need_MemorySpace>();
            if (memorySpace == null)
            {
                return null;
            }
            Building_AndroidStand stand = FindStandFor(pawn);
            if (stand != null)
            {
                return JobMaker.MakeJob(VREA_DefOf.VREA_FreeMemorySpace, stand);
            }
            return null;
        }

        public static Building_AndroidStand FindStandFor(Pawn pawn)
        {
            foreach (var stand in Building_AndroidStand.stands)
            {
                if (stand.CompAssignableToPawn.AssignedPawns.Contains(pawn) && stand.CannotUseNowReason(pawn) is null
                    && pawn.CanReserveAndReach(stand, PathEndMode.OnCell, Danger.Deadly))
                {
                    return stand;
                }
            }
            foreach (var stand in Building_AndroidStand.stands)
            {
                if (stand.CompAssignableToPawn.AssignedPawns.Any() is false && stand.CannotUseNowReason(pawn) is null
                    && pawn.CanReserveAndReach(stand, PathEndMode.OnCell, Danger.Deadly))
                {
                    stand.CompAssignableToPawn.TryAssignPawn(pawn);
                    return stand;
                }
            }
            return null;
        }
    }
}
