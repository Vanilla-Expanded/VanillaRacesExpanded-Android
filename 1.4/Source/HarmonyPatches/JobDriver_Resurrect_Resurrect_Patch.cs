using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(JobDriver_Resurrect), "Resurrect")]
    public static class JobDriver_Resurrect_Resurrect_Patch
    {
        public static bool Prefix(JobDriver_Resurrect __instance)
        {
            if (__instance.Corpse.InnerPawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
