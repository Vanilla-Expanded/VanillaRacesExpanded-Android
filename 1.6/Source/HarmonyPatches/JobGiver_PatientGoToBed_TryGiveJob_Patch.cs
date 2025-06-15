using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(JobGiver_PatientGoToBed), "TryGiveJob")]
    public static class JobGiver_PatientGoToBed_TryGiveJob_Patch
    {
        public static void Postfix(ref Job __result, Pawn pawn)
        {
            if (__result != null && pawn.IsAndroid()) 
            {
                bool canRepairAndroid = JobDriver_RepairAndroid.CanRepairAndroid(pawn);
                if (canRepairAndroid is false && pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss) != null)
                {
                    if (__result.targetA.Thing is not Building_NeutroCasket neutroCasket || (neutroCasket.compPower.PowerOn is false 
                        || neutroCasket.compRefuelable.HasFuel is false))
                    {
                        __result = null;
                    }
                }
                if (__result != null && canRepairAndroid && pawn.playerSettings.selfTend)
                {
                    if (WorkGiver_RepairAndroid.HasJobOn(pawn, pawn, false))
                    {
                        __result = JobMaker.MakeJob(VREA_DefOf.VREA_RepairAndroid, pawn);
                    }
                }
            }
        }
    }
}
