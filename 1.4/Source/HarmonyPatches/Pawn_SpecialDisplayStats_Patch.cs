using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn), "SpecialDisplayStats")]
    public static class Pawn_SpecialDisplayStats_Patch
    {
        public static HashSet<string> statLabelsPreventFromAndroids = new HashSet<string>
        {
            "StatsReport_AgeRateMultiplier".Translate(), "StatsReport_DevelopmentStage".Translate(), 
            "Diet".Translate(), 
            "FoodConsumption".Translate(), "LeatherType".Translate(), "StatsReport_LifeExpectancy".Translate()
        };
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, Pawn __instance)
        {
            foreach (var entry in __result)
            {
                if (__instance.IsAndroid())
                {
                    if (entry.labelInt == "Race".Translate())
                    {
                        string reportText = (__instance.genes.UniqueXenotype ? "VREA.UniqueAndroidDesc".Translate().ToString() : __instance.DescriptionFlavor);
                        yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "VREA.Android".Translate(), __instance.genes.XenotypeLabel, reportText, 2100, null, __instance.genes.UniqueXenotype ? null : Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(__instance.genes.Xenotype)));
                    }
                    if (entry.labelInt == "MeditationFocuses".Translate())
                    {
                        if (__instance.HasActiveGene(VREA_DefOf.VREA_JoyDisabled))
                        {
                            continue;
                        }
                        else
                        {
                            yield return entry;
                        }
                    }
                    if (statLabelsPreventFromAndroids.Contains(entry.labelInt))
                    {
                        continue;
                    }
                }
                else
                {
                    yield return entry;
                }
            }
        }
    }
}
