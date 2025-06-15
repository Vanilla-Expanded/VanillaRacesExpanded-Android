using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "Notify_TuckedIntoBed")]
    public static class Pawn_JobTracker_Notify_TuckedIntoBed_Patch
    {
        public static void Postfix(Pawn_JobTracker __instance, Building_Bed bed)
        {
            if (bed is Building_AndroidStand)
            {
                __instance.pawn.Rotation = Rot4.West;
            }
        }
    }
}
