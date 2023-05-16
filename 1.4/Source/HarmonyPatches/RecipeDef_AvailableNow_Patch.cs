using HarmonyLib;
using System.Diagnostics;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RecipeDef), "AvailableNow", MethodType.Getter)]
    public static class RecipeDef_AvailableNow_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(RecipeDef __instance, ref bool __result)
        {
            var curPawn = HealthCardUtility_DrawPawnHealthCard_Patch.curPawn;
            if (__result && curPawn != null && curPawn.IsAndroid()) 
            {
                __result = RecipeWorker_AvailableOnNow_Patch.RecipeIsAvailableOnAndroid(__instance.Worker, curPawn);
            }
        }
    }
}
