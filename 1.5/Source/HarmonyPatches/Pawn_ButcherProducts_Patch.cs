using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn), "ButcherProducts")]
    public static class Pawn_ButcherProducts_Patch
    {
        public static IEnumerable<Thing> Postfix(IEnumerable<Thing> __result, Pawn __instance)
        {
            if (__instance.IsAndroid())
            {
                var thing = ThingMaker.MakeThing(ThingDefOf.Plasteel);
                thing.stackCount = 15;
                yield return thing;

                thing = ThingMaker.MakeThing(ThingDefOf.Steel);
                thing.stackCount = 20;
                yield return thing;

                thing = ThingMaker.MakeThing(VREA_DefOf.Neutroamine);
                thing.stackCount = 5;
                yield return thing;
            }
            else
            {
                foreach (var r in __result)
                {
                    yield return r;
                }
            }
        }
    }
}
