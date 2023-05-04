using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class RecipeWorker_AvailableOnNow_Patch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return typeof(RecipeWorker).GetMethod("AvailableOnNow", AccessTools.all);
            foreach (var type in typeof(RecipeWorker).AllSubclasses())
            {
                var method = type.GetMethod("AvailableOnNow", AccessTools.all);
                if (method != null)
                {
                    yield return method;
                }
            }
        }
        public static void Postfix(ref bool __result, RecipeWorker __instance, Thing __0)
        {
            if (__0 is Pawn pawn && pawn.IsAndroid())
            {
                if (__instance is Recipe_AdministerIngestible)
                {
                    __result = false;
                }
                if (__instance is Recipe_RemoveBodyPart && __instance is not Recipe_RemoveArtificalBodyPart)
                {
                    __result = false;
                }
                if (pawn.def.recipes.Contains(__instance.recipe) && __instance.recipe != VREA_DefOf.VREA_RemoveArtificalPart)
                {
                    __result = false;
                }
                if (__instance.recipe.addsHediff == HediffDefOf.Sterilized)
                {
                    __result = false;
                }
            }
        }
    }
}
