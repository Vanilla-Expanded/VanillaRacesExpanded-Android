﻿using HarmonyLib;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MentalBreaker), "CanDoRandomMentalBreaks", MethodType.Getter)]
    public static class MentalBreaker_CanDoRandomMentalBreaks_Patch
    {
        public static void Postfix(MentalBreaker __instance, ref bool __result)
        {
            if (__instance.pawn.HasActiveGene(VREA_DefOf.VREA_MentalBreaksDisabled))
            {
                __result = false;
            }
        }
    }
}
