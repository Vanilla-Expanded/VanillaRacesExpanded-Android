using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_GeneTracker), "XenotypeDescShort", MethodType.Getter)]
    public static class Pawn_GeneTracker_XenotypeDescShort_Patch
    {
        public static bool Prefix(Pawn_GeneTracker __instance, ref string __result, Pawn ___pawn)
        {
            if (___pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                if (__instance.UniqueXenotype)
                {
                    __result = "UniqueXenotypeDesc".Translate();
                }
                else if (!__instance.Xenotype.descriptionShort.NullOrEmpty())
                {
                    __result = __instance.Xenotype.descriptionShort + "\n\n" + "VREA.MoreInfoInInfoScreen".Translate().Colorize(ColoredText.SubtleGrayColor);
                }
                else
                {
                    __result = __instance.Xenotype.description;
                }
                return false;
            }
            return true;
        }
    }
}
