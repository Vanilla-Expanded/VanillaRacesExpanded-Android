using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "AddEveryMedicineToRelevantThings")]
    public static class WorkGiver_DoBill_AddEveryMedicineToRelevantThings_Patch
    {
        public static bool Prefix(Thing billGiver)
        {
            if (billGiver is Pawn pawn2 && pawn2.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
