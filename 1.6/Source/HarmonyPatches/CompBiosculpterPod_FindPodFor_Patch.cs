using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompBiosculpterPod), nameof(CompBiosculpterPod.FindPodFor))]
    public static class CompBiosculpterPod_FindPodFor_Patch
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