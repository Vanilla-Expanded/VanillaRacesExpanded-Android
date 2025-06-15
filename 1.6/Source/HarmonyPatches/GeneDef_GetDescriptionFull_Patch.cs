using HarmonyLib;
using RimWorld;
using System.Text;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(GeneDef), "GetDescriptionFull")]
    public static class GeneDef_GetDescriptionFull_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(GeneDef __instance, ref string __result)
        {
            if (__instance.IsAndroidGene())
            {
                if (__instance.biostatMet != 0)
                {
                    var originalLine = "Metabolism".Translate().Colorize(GeneUtility.METColor) + ": " + __instance.biostatMet.ToStringWithSign();
                    var newLine = "VREA.PowerEfficiency".Translate().Colorize(SimpleColor.Yellow.ToUnityColor()) + ": " + __instance.biostatMet.ToStringWithSign();
                    __result = __result.Replace(originalLine, newLine);
                }
            }
        }
    }
}
