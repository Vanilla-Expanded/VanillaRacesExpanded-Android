using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_GeneTracker), "ExposeData")]
    public static class Pawn_GeneTracker_ExposeData_Patch
    {
        public static void Postfix(Pawn_GeneTracker __instance)
        {
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                Utils.RecheckGenes(__instance);
                Utils.RecheckHediffs(__instance.pawn);
            }
        }
    }
}
