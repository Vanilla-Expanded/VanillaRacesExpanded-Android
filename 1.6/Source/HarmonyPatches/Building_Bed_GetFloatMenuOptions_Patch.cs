using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Building_Bed), "GetFloatMenuOptions")]
    public static class Building_Bed_GetFloatMenuOptions_Patch
    {
        public static IEnumerable<FloatMenuOption> Postfix(IEnumerable<FloatMenuOption> __result, Pawn myPawn, Building_Bed __instance)
        {
            if (myPawn.IsAndroid(out var gene))
            {
                foreach (var opt in GetFloatMenuOptions(__instance, gene, myPawn))
                {
                    yield return opt;
                }
            }
            else
            {
                foreach (var option in __result)
                {
                    yield return option;
                }
            }

        }

        public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Building_Bed __instance, Gene_SyntheticBody gene, Pawn myPawn)
        {
            if (!myPawn.RaceProps.Humanlike || __instance.ForPrisoners || !__instance.Medical || myPawn.Drafted 
                || __instance.Faction != Faction.OfPlayer || !RestUtility.CanUseBedEver(myPawn, __instance.def))
            {
                yield break;
            }
            if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && JobDriver_RepairAndroid.CanRepairAndroid(myPawn) is false)
            {
                if (myPawn.health.surgeryBills.AnyShouldDoNow && !WorkGiver_PatientGoToBedTreatment.AnyAvailableDoctorFor(myPawn))
                {
                    yield return new FloatMenuOption("VREA.UseBedAndroid".Translate() + " (" + "VREA.NoCrafter".Translate() + ")", null);
                }
                else
                {
                    yield return new FloatMenuOption("VREA.UseBedAndroid".Translate() + " (" + "NotInjured".Translate() + ")", null);
                }
                yield break;
            }
            else if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn) && JobDriver_RepairAndroid.CanRepairAndroid(myPawn) && gene.autoRepair is false)
            {
                yield return new FloatMenuOption("VREA.UseBedAndroid".Translate() + " (" + "VREA.NotSetToAutoRepair".Translate() + ")", null);
                yield break;
            }
            if (myPawn.IsSlaveOfColony && !__instance.ForSlaves)
            {
                yield return new FloatMenuOption("VREA.UseBedAndroid".Translate() + " (" + "NotForSlaves".Translate() + ")", null);
                yield break;
            }
            Action action = delegate
            {
                if (!__instance.ForPrisoners && __instance.Medical && myPawn.CanReserveAndReach(__instance, PathEndMode.ClosestTouch, 
                    Danger.Deadly, __instance.SleepingSlotsCount, -1, null, ignoreOtherReservations: true))
                {
                    if (myPawn.CurJobDef == JobDefOf.LayDown && myPawn.CurJob.GetTarget(TargetIndex.A).Thing == __instance)
                    {
                        myPawn.CurJob.restUntilHealed = true;
                    }
                    else
                    {
                        Job job = JobMaker.MakeJob(JobDefOf.LayDown, __instance);
                        job.restUntilHealed = true;
                        myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }
                    myPawn.mindState.ResetLastDisturbanceTick();
                }
            };
            yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("VREA.UseBedAndroid".Translate(), action), myPawn, 
                __instance, (__instance.AnyUnoccupiedSleepingSlot ? "ReservedBy" : "SomeoneElseSleeping").CapitalizeFirst());
        }
    }
}
