using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompBiosculpterPod), "CannotUseNowPawnReason")]
    public static class CompBiosculpterPod_CannotUseNowPawnReason_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref string __result, Pawn p)
        {
            if (p.IsAndroid())
            {
                __result = "VREA.CannotUseAndroid".Translate();
            }
        }
    }
}
