using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(FoodUtility), "WillEat_NewTemp", new Type[] { typeof(Pawn), typeof(Thing), typeof(Pawn), typeof(bool), typeof(bool) })]
    public static class FoodUtility_WillEat_Thing_Patch
    {
        public static void Postfix(ref bool __result, Pawn p, Thing food)
        {
            if (p.IsAndroid())
            {
                __result = false;
            }
            else if (food is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
