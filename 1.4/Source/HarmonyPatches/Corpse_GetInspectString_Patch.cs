using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Corpse), "GetInspectString")]
    public static class Corpse_GetInspectString_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Corpse __instance, ref string __result)
        {
            if (__instance.InnerPawn.IsAndroid())
            {
                float num = 1f - __instance.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(__instance.InnerPawn.RaceProps.body.corePart);
                __result = __result.Replace("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent(), "");
            }
        }
    }
}
