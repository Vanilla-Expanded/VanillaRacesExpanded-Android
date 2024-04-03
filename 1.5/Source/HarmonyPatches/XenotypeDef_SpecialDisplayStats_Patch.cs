using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(XenotypeDef), "SpecialDisplayStats")]
    public static class XenotypeDef_SpecialDisplayStats_Patch
    {
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, XenotypeDef __instance, StatRequest req)
        {
            if (__instance.genes.Any(x => x.IsAndroidGene()))
            {
                foreach (var entry in __result)
                {
                    if (entry.labelInt == "Genes".Translate().CapitalizeFirst())
                    {
                        yield return new StatDrawEntry(StatCategoryDefOf.Basics, 
                            "VREA.Components".Translate().CapitalizeFirst(), __instance.genes.Select((GeneDef x) => x.label).ToCommaList()
                            .CapitalizeFirst(), "VREA.ComponentsDesc".Translate() + "\n\n" + __instance.genes.Select((GeneDef x) => x.label)
                            .ToLineList("  - ", capitalizeItems: true), 1000);
                    }
                    else
                    {
                        if (entry.labelInt != "GenesAreInheritable".Translate())
                        {
                            yield return entry;
                        }
                    }
                }
            }
            else
            {
                foreach (var entry in __result)
                {
                    yield return entry;
                }
            }
        }
    }
}
