using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompBiosculpterPod), "CanAgeReverse")]
    public static class CompBiosculpterPod_CanAgeReverse_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn biosculptee)
        {
            if (biosculptee.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
