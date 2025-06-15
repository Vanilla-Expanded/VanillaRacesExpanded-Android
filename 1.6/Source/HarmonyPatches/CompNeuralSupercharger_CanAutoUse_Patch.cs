using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompNeuralSupercharger), "CanAutoUse")]
    public static class CompNeuralSupercharger_CanAutoUse_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (__result && pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
