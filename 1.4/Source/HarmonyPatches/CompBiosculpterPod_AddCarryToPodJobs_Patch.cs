using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompBiosculpterPod), "AddCarryToPodJobs")]
    public static class CompBiosculpterPod_AddCarryToPodJobs_Patch
    {
        public static bool Prefix(Pawn traveller)
        {
            if (traveller.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
