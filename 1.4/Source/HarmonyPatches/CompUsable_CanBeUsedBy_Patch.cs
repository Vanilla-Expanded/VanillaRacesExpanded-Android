using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompUsable), "CanBeUsedBy")]
    public static class CompUsable_CanBeUsedBy_Patch
    {
        public static void Postfix(CompUsable __instance, ref bool __result, Pawn p, ref string failReason)
        {
            if (p.IsAndroid() && __instance is CompNeurotrainer)
            {
                failReason = "VREA.CannotUseAndroid".Translate();
                __result = false;
            }
        }
    }
}
