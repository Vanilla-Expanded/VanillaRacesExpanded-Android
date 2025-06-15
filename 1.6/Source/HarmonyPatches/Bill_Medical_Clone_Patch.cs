using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Bill_Medical), "Clone")]
    public static class Bill_Medical_Clone_Patch
    {
        public static void Postfix(Bill __result)
        {
            if (__result is Bill_Medical bill && bill.billStack != null && bill.GiverPawn.IsAndroid())
            {
                bill.recipe = bill.recipe.RecipeForAndroid();
            }
        }
    }
}
