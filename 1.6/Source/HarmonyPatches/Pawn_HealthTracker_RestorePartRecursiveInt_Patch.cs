using HarmonyLib;
using System;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), "RestorePartRecursiveInt")]
    public static class Pawn_HealthTracker_RestorePartRecursiveInt_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(Pawn_HealthTracker __instance, BodyPartRecord part, Hediff diffException = null)
        {
            if (__instance.pawn.IsAndroid())
            {
                List<Hediff> hediffs = __instance.hediffSet.hediffs;
                for (int num = hediffs.Count - 1; num >= 0; num--)
                {
                    Hediff hediff = hediffs[num];
                    if (hediff.Part == part && hediff != diffException && !hediff.def.keepOnBodyPartRestoration)
                    {
                        hediffs.RemoveAt(num);
                        hediff.PostRemoved();
                    }
                }
                return false;
            }
            return true;
        }
    }
}
