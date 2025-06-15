using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Bill), "IsFixedOrAllowedIngredient", new Type[] { typeof(Thing) })]
    public static class Bill_IsFixedOrAllowedIngredient_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(ref bool __result, Bill __instance, Thing thing)
        {
            if (__instance.recipe == VREA_DefOf.ButcherCorpseFlesh && thing is Corpse corpse 
                && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
                return false;
            }
            else if (__instance.recipe == VREA_DefOf.VREA_ButcherCorpseAndroid && thing is Corpse corpse2
                && corpse2.InnerPawn.IsAndroid() is false)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
