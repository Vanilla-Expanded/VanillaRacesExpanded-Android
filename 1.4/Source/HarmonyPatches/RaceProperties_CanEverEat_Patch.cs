using HarmonyLib;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RaceProperties), "CanEverEat", new Type[] { typeof(Thing) })]
    public static class RaceProperties_CanEverEat_Patch
    {
        public static void Postfix(ref bool __result, Thing t)
        {
            if (t is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
