﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThinkNode_ConditionalBurning), "Satisfied")]
    public static class ThinkNode_ConditionalBurning_Satisfied_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(Pawn pawn)
        {
            if (pawn.IsAndroid() && pawn.HasActiveGene(VREA_DefOf.VREA_FireVulnerability) is false && pawn.Drafted)
                return false;
            return true;
        }
    }
}
