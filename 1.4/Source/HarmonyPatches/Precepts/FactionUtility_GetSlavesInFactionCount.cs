using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using System;

namespace VREAndroids
{
    [HarmonyPatch(typeof(FactionUtility), "GetSlavesInFactionCount")]
    public static class FactionUtility_GetSlavesInFactionCount_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Faction faction, ref int __result)
        {
            if (Faction.OfPlayerSilentFail?.ideos?.primaryIdeo?.HasPrecept(VREA_DefOf.VRE_Androids_Tools) == true)
            {
                int num = __result;
                foreach (Pawn item in PawnsFinder.AllMaps_SpawnedPawnsInFaction(faction))
                {
                    if (item.IsAndroid()&&item.IsSlave)
                    {
                        num--;
                    }
                }
                __result= num;
            }
           
        }
    }
}
