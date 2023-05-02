using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn), "SpecialDisplayStats")]
    public static class Pawn_SpecialDisplayStats_Patch
    {
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, Pawn __instance)
        {
            foreach (var entry in __result)
            {
                if (entry.labelInt == "Race".Translate() && __instance.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
                {
                    string reportText = (__instance.genes.UniqueXenotype ? "VREA.UniqueAndroidDesc".Translate().ToString() : __instance.DescriptionFlavor);
                    yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "VREA.Android".Translate(), __instance.genes.XenotypeLabel, reportText, 2100, null, __instance.genes.UniqueXenotype ? null : Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(__instance.genes.Xenotype)));
                }
                else
                {
                    yield return entry;
                }
            }
        }
    }
}
