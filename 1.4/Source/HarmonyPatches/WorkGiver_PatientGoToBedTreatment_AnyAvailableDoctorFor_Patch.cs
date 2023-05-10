using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(WorkGiver_PatientGoToBedTreatment), "AnyAvailableDoctorFor")]
    public static class WorkGiver_PatientGoToBedTreatment_AnyAvailableDoctorFor_Patch
    {
        public static bool Prefix(Pawn pawn, ref bool __result)
        {
            if (pawn.IsAndroid())
            {
                __result = AnyAvailableCrafterFor(pawn);
                return false;
            }
            return true;
        }

        public static bool AnyAvailableCrafterFor(Pawn pawn)
        {
            Map mapHeld = pawn.MapHeld;
            if (mapHeld == null)
            {
                return false;
            }
            List<Pawn> list = mapHeld.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
            for (int i = 0; i < list.Count; i++)
            {
                Pawn pawn2 = list[i];
                if (pawn2 != pawn && (pawn2.RaceProps.Humanlike || pawn2.IsColonyMechPlayerControlled) 
                    && !pawn2.Downed && pawn2.Awake() && !pawn2.InBed() 
                    && !pawn2.InMentalState && !pawn2.IsPrisoner 
                    && pawn2.workSettings != null && pawn2.workSettings.EverWork 
                    && pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Crafting) 
                    && pawn2.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) 
                    && pawn2.CanReach(pawn, PathEndMode.Touch, Danger.Deadly))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
