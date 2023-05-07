using HarmonyLib;
using RimWorld;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompRottable), "RotProgress", MethodType.Setter)]
    public static class CompRottable_RotProgress_Patch
    {
        public static bool Prefix(CompRottable __instance)
        {
            if (__instance.parent is Pawn pawn && pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CompRottable), "Active", MethodType.Getter)]
    public static class CompRottable_Active_Patch
    {
        public static bool Prefix(CompRottable __instance, ref bool __result)
        {
            if (__instance.parent is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(CompRottable), "CompInspectStringExtra")]
    public static class CompRottable_CompInspectStringExtra_Patch
    {
        public static bool Prefix(CompRottable __instance, ref string __result)
        {
            if (__instance.parent is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = null;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(Corpse), "GetInspectString")]
    public static class Corpse_GetInspectString_Patch
    {
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
