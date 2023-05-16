using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn), "DescriptionFlavor", MethodType.Getter)]
    public static class Pawn_DescriptionFlavor_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(ref string __result, Pawn __instance)
        {
            if (__instance.IsAndroid())
            {
                string text = ((__instance.genes.Xenotype != XenotypeDefOf.Baseliner) ? __instance.genes.Xenotype.description : 
                    ((__instance.genes.CustomXenotype == null) ? __instance.genes.Xenotype.description : ((string)"VREA.UniqueAndroidDesc".Translate())));
                __result = "VREA.StatsReport_AndroidDescription".Translate(__instance.genes.XenotypeLabel) + "\n\n" + text;
                return false;
            }
            return true;
        }
    }
}
