using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using System;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_ForHumanlike")]
    public static class PawnDiedOrDownedThoughtsUtility_AppendThoughts_ForHumanlike_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn victim, DamageInfo? dinfo)
        {
            
            if (Faction.OfPlayerSilentFail?.ideos?.primaryIdeo?.HasPrecept(VREA_DefOf.VRE_Androids_Tools)==true && victim.IsAndroid())
            {          
                return false;
            }
            return true;
        }
    }
}
