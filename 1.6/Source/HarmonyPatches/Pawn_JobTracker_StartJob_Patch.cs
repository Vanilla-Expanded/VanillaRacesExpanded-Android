using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "StartJob")]
    public static class Pawn_JobTracker_StartJob_Patch
    {
        public static bool Prefix(Pawn ___pawn, Job newJob, JobCondition lastJobEndCondition = JobCondition.None, ThinkNode jobGiver = null, bool resumeCurJobAfterwards = false, bool cancelBusyStances = true, ThinkTreeDef thinkTree = null, JobTag? tag = null, bool fromQueue = false, bool canReturnCurJobToPool = false)
        {
            if (___pawn.IsAndroid() && newJob?.def == JobDefOf.Vomit)
            {
                return false;
            }
            return true;
        }
    }
}

