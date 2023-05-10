using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(JobDriver_LayDown), "GetReport")]
    public static class JobDriver_LayDown_GetReport_Patch
    {
        public static bool Prefix(JobDriver_LayDown __instance, ref string __result)
        {
            if (__instance.pawn.IsAndroid())
            {
                __result = "VREA.Laying".Translate();
                return false;
            }
            return true;
        }
    }
}
