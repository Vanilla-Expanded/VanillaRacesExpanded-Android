﻿using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(TraitSet), "GainTrait")]
    public static class TraitSet_GainTrait_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn ___pawn, Trait trait)
        {
            if (___pawn.HasActiveGene(VREA_DefOf.VREA_PsychologyDisabled))
            {
                if (VREA_DefOf.VREA_AndroidSettings.allowedTraits.Contains(trait.def.defName))
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
