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
        public override float GetPriority(Pawn pawn)
        {
            var memorySpace = pawn.needs.TryGetNeed<Need_MemorySpace>();
            if (memorySpace == null)
            {
                return 0f;
            }
            if (memorySpace.CurLevelPercentage > 0.2f)
            {
                return 0f;
            }
            Lord lord = pawn.GetLord();
            if (lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds)
            {
                return 0f;
            }
            TimeAssignmentDef timeAssignmentDef = pawn.timetable.CurrentAssignment;
            float curLevel = memorySpace.CurLevel;
            if (timeAssignmentDef == TimeAssignmentDefOf.Anything)
            {
                if (curLevel < 0.3f)
                {
                    return 8f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Work)
            {
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Meditate)
            {
                if (curLevel < 0.16f)
                {
                    return 8f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Joy)
            {
                if (curLevel < 0.3f)
                {
                    return 8f;
                }
                return 0f;
            }
            if (timeAssignmentDef == TimeAssignmentDefOf.Sleep)
            {
                return 8f;
            }
            throw new NotImplementedException();
        }
        public override Job TryGiveJob(Pawn pawn)
        {
            var memorySpace = pawn.needs.TryGetNeed<Need_MemorySpace>();
            if (memorySpace == null)
            {
                return null;
            }
            Lord lord = pawn.GetLord();
            Building_AndroidStand stand = ((lord == null || lord.CurLordToil == null || lord.CurLordToil.AllowRestingInBed) 
                && !pawn.IsWildMan() && (!pawn.InMentalState || pawn.MentalState.AllowRestingInBed) ? FindStandFor(pawn) : null);
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
                if (stand.CompAssignableToPawn.AssignedPawns.Contains(pawn) && stand.compPower.PowerOn
                    && pawn.CanReserveAndReach(stand, PathEndMode.OnCell, Danger.Deadly))
                {
                    return stand;
                }
            }
            return null;
        }
    }
}
