using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(FeedPatientUtility), "ShouldBeFed")]
    public static class FeedPatientUtility_ShouldBeFed_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn p, ref bool __result)
        {
            if (p.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
