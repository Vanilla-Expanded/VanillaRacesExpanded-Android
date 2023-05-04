using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Bill), "IsFixedOrAllowedIngredient", new Type[] { typeof(Thing) })]
    public static class Bill_IsFixedOrAllowedIngredient_Patch
    {
        private static bool Prefix(ref bool __result, Bill __instance, Thing thing)
        {
            if (__instance.recipe == VREA_DefOf.ButcherCorpseFlesh && thing is Corpse corpse 
                && corpse.InnerPawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __result = false;
                return false;
            }
            else if (__instance.recipe == VREA_DefOf.VREA_ButcherCorpseAndroid && thing is Corpse corpse2
                && corpse2.InnerPawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody) is false)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
