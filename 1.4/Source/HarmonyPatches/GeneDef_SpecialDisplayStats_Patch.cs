using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{

    [HarmonyPatch(typeof(GeneDef), "SpecialDisplayStats")]
    public static class GeneDef_SpecialDisplayStats_Patch
    {
        public static void Postfix(GeneDef __instance, ref IEnumerable<StatDrawEntry> __result)
        {
            var stats = __result.ToList();
            if (__instance.IsAndroidGene())
            {
                stats.RemoveAll(x => x.category == StatCategoryDefOf.Genetics);
                if (__instance.biostatCpx != 0)
                {
                    stats.Add(new StatDrawEntry(VREA_DefOf.VREA_Android, "Complexity".Translate(), __instance.biostatCpx.ToString(), 
                        "VREA.ComplexityDesc".Translate(), 998));
                }
                if (__instance.biostatMet != 0)
                {
                    stats.Add(new StatDrawEntry(VREA_DefOf.VREA_Android, "VREA.PowerEfficiency".Translate(), __instance.biostatMet.ToString(), "VREA.PowerEfficiencyDesc".Translate(), 997));
                }
            }
            __result = stats;
        }
    }
}
