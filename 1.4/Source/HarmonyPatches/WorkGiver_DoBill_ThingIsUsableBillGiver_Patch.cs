using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "ThingIsUsableBillGiver")]
    public static class WorkGiver_DoBill_ThingIsUsableBillGiver_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Thing thing, ref bool __result, WorkGiver_DoBill __instance)
        {
            if (__instance.def.billGiversAllHumanlikes && __instance.def.billGiversAllHumanlikesCorpses)
            {
                Pawn pawn = thing as Pawn;
                Corpse corpse = thing as Corpse;
                Pawn pawn2 = null;
                if (corpse != null)
                {
                    pawn2 = corpse.InnerPawn;
                }
                if (__instance.def == VREA_DefOf.VREA_DoBillsAndroidOperation)
                {
                    if (pawn != null && pawn.IsAndroid() is false)
                    {
                        __result = false;
                    }
                    if (corpse != null && pawn2 != null && pawn2.IsAndroid() is false)
                    {
                        __result = false;
                    }
                }
                else if (__instance.def == WorkGiverDefOf.DoBillsMedicalHumanOperation)
                {
                    if (pawn != null && pawn.IsAndroid())
                    {
                        __result = false;
                    }
                    if (corpse != null && pawn2 != null && pawn2.IsAndroid())
                    {
                        __result = false;
                    }
                }
            }
        }
    }
}
