using RimWorld;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    public class JobGiver_MakeLovin : ThinkNode_JobGiver
    {
        public override float GetPriority(Pawn pawn)
        {
            return 8;
        }
        public override Job TryGiveJob(Pawn pawn)
        {
            if (pawn.IsAndroid() is false || pawn.Emotionless())
            {
                return null;
            }
            if (Find.TickManager.TicksGame < pawn.mindState.canLovinTick)
            {
                return null;
            }
            var building_Bed = pawn.CurrentBed() ?? pawn.ownership.OwnedBed;
            if (building_Bed is null || !pawn.health.capacities.CanBeAwake)
            {
                return null;
            }
            Pawn partnerInMyBed = GetPartnerInMyBed(building_Bed, pawn);
            if (partnerInMyBed == null || !partnerInMyBed.health.capacities.CanBeAwake 
                || Find.TickManager.TicksGame < partnerInMyBed.mindState.canLovinTick)
            {
                return null;
            }
            if (!pawn.CanReserve(partnerInMyBed) || !partnerInMyBed.CanReserve(pawn))
            {
                return null;
            }
            return JobMaker.MakeJob(JobDefOf.Lovin, partnerInMyBed, building_Bed);
        }

        public static Pawn GetPartnerInMyBed(Building_Bed building_Bed, Pawn pawn)
        {
            if (building_Bed == null)
            {
                return null;
            }
            if (building_Bed.SleepingSlotsCount <= 1)
            {
                return null;
            }
            if (!LovePartnerRelationUtility.HasAnyLovePartner(pawn))
            {
                return null;
            }
            foreach (Pawn curOccupant in building_Bed.CurOccupants)
            {
                if (curOccupant != pawn && LovePartnerRelationUtility.LovePartnerRelationExists(pawn, curOccupant))
                {
                    return curOccupant;
                }
            }
            return null;
        }

    }
}
