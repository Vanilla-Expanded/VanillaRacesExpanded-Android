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
                var method = type.GetMethod("AvailableOnNow", AccessTools.allDeclared);
                if (method != null)
                {
                    yield return method;
                }
            }
        }
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref bool __result, RecipeWorker __instance, Thing __0)
        {
            if (__0 is Pawn pawn && pawn.IsAndroid())
            {
                __result = RecipeIsAvailableOnAndroid(__instance, pawn);
            }
        }

        public static bool RecipeIsAvailableOnAndroid(RecipeWorker recipeWorker, Pawn pawn)
        {
            if (recipeWorker is Recipe_AdministerIngestible)
            {
                return false;
            }
            if (recipeWorker is Recipe_RemoveBodyPart && recipeWorker is not Recipe_RemoveArtificialBodyPart)
            {
                return false;
            }
            if (recipeWorker is Recipe_InstallNaturalBodyPart)
            {
                return false;
            }
            if (pawn.def.recipes.Contains(recipeWorker.recipe) && recipeWorker.recipe != VREA_DefOf.VREA_RemoveArtificialPart)
            {
                return false;
            }
            if (recipeWorker.recipe.addsHediff == HediffDefOf.Sterilized)
            {
                return false;
            }

            return true;
        }
    }
}
