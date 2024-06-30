using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SteadyEnvironmentEffects), "FinalDeteriorationRate", 
        new[] { typeof(Thing), typeof(bool), typeof(bool), typeof(TerrainDef), typeof(List<string>) })]
    public static class SteadyEnvironmentEffects_FinalDeteriorationRate_Patch
    {
        public static void Postfix(Thing t, ref float __result)
        {
            if (t.def == VREA_DefOf.VREA_SpentReactor)
            {
                __result = 50;
            }
        }
    }
}
