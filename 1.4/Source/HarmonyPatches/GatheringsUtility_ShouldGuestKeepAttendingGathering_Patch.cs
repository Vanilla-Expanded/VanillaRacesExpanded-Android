using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(GatheringsUtility), "ShouldGuestKeepAttendingGathering")]
    public static class GatheringsUtility_ShouldGuestKeepAttendingGathering_Patch
    {
        public static void Postfix(ref bool __result, Pawn p)
        {
            if (p.Emotionless())
            {
                __result = false;
            }
        }
    }
}
