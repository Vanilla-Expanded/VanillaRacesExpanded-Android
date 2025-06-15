using HarmonyLib;
using RimWorld;
using System.Reflection;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public static class DubsMintMenus_Patch_HealthCardUtility_GenerateListing_Patch
    {
        public static MethodBase targetMethod;

        public static bool Prepare()
        {
            targetMethod = AccessTools.Method("DubsMintMenus.Patch_HealthCardUtility:GenerateListing");
            return targetMethod != null;
        }
        public static MethodBase TargetMethod()
        {
            return targetMethod;
        }
        public static void Prefix(Pawn pawn2, ref RecipeDef recipe, BodyPartRecord part)
        {
            if (pawn2.IsAndroid())
            {
                recipe = recipe.RecipeForAndroid();
            }
        }
    }
}
