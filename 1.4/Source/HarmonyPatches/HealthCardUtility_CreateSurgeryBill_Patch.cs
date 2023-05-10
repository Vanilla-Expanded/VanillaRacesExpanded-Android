using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthCardUtility), "CreateSurgeryBill")]
    public static class HealthCardUtility_CreateSurgeryBill_Patch
    {
        public static void Postfix(Bill_Medical __result)
        {
            if (__result.GiverPawn.IsAndroid())
            {
                __result.recipe = __result.recipe.RecipeForAndroid();
            }
        }
    }
}
