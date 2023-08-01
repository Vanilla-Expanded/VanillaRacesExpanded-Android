using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MentalBreakWorker_RunWild), "TryStart")]
    public static class MentalBreakWorker_RunWild_TryStart_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
