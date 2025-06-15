using HarmonyLib;
using System;
using Verse;
using static Verse.DamageWorker;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDead")]
    public static class Pawn_HealthTracker_ShouldBeDead_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref bool __result, Pawn ___pawn)
        {
            if (__result && ___pawn.IsAndroid())
            {
                if (___pawn.health.hediffSet.GetBrain() != null)
                {
                    __result = false;
                }
            }
        }
    }
}
