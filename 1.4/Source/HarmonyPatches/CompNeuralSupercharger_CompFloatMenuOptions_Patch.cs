using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompNeuralSupercharger), "CompFloatMenuOptions")]
    public static class CompNeuralSupercharger_CompFloatMenuOptions_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(CompNeuralSupercharger __instance, Pawn selPawn, ref IEnumerable<FloatMenuOption> __result)
        {
            if (selPawn.IsAndroid())
            {
                __result = __result.Where(x => x.Label != __instance.Props.jobString);
            }
        }
    }
}
