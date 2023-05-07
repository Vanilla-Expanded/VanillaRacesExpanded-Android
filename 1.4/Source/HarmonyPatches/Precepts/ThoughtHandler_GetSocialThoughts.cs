using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;
using System;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThoughtHandler), "GetSocialThoughts", new Type[]
    {
        typeof(Pawn), typeof(List<ISocialThought>)
    })]
    public static class ThoughtHandler_GetSocialThoughts_Patch
    {
        public static bool Prefix(Pawn otherPawn, List<ISocialThought> outThoughts, ThoughtHandler __instance)
        {
            if (!__instance.pawn.IsAndroid() && (__instance.pawn.Ideo?.HasPrecept(VREA_DefOf.VRE_Androids_Tools)==true&&otherPawn.IsAndroid()))
            {          
                return false;
            }
            return true;
        }
    }
}
