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
            if (ModsConfig.IsActive("Dubwise.DubsMintMenus"))
            {
                targetMethod = AccessTools.Method("DubsMintMenus.Patch_HealthCardUtility:GenerateListing");
                if (targetMethod != null)
                {
                    return true;
                }
                Log.Error("[VREAndroids] Failed to patch DubsMintMenus mod for Patch_HealthCardUtility:GenerateListing");
            }
            return false;
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
