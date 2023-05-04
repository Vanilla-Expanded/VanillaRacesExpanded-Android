using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(FoodUtility), "WillEat_NewTemp", new Type[] { typeof(Pawn), typeof(Thing), typeof(Pawn), typeof(bool), typeof(bool) })]
    public static class FoodUtility_WillEat_Patch
    {
        public static void Postfix(ref bool __result, Thing food)
        {
            if (food is Corpse corpse && corpse.InnerPawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __result = false;
            }
        }
    }
}
