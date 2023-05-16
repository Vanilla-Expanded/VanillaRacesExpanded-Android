using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_StoryTracker), "SkinColor", MethodType.Getter)]
    public static class Pawn_StoryTracker_SkinColor_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn ___pawn, ref Color __result, Pawn_StoryTracker __instance)
        {
            var neutroloss = ___pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_NeutroLoss);
            if (neutroloss != null)
            {
                __result = Color.Lerp(__result, Color.white, neutroloss.Severity);
            }
        }
    }
}
