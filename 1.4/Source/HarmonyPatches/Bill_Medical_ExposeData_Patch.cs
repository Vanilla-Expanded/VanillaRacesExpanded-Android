using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Bill_Medical), "ExposeData")]
    public static class Bill_Medical_ExposeData_Patch
    {
        public static void Postfix(Bill_Medical __instance)
        {
            if (__instance.billStack != null && __instance.GiverPawn.IsAndroid())
            {
                __instance.ChangeRecipeForAndroid();
            }
        }
    }
}
