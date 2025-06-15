using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Xenogerm), "GetFloatMenuOptions")]
    public static class Xenogerm_GetFloatMenuOptions_Patch
    {
        public static IEnumerable<FloatMenuOption> Postfix(IEnumerable<FloatMenuOption> __result, Pawn selPawn, Xenogerm __instance)
        {
            foreach (var option in __result)
            {
                if (selPawn.IsAndroid() && option.labelInt == "OrderImplantationIntoPawn".Translate(selPawn.Named("PAWN")) + 
                    " (" + __instance.xenotypeName + ")")
                {
                    yield return new FloatMenuOption("CannotGenericWorkCustom".Translate("OrderImplantationIntoPawn".Translate(selPawn.Named("PAWN")).Resolve().UncapitalizeFirst() 
                        + ": " + "VREA.CannotInstallOnAndroid".Translate()), null);
                }
                else
                {
                    yield return option;
                }
            }
        }
    }
}
