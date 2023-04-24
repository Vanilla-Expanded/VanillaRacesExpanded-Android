using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RecipeWorker), "AvailableOnNow")]
    public static class RecipeWorker_AvailableOnNow_Patch
    {
        public static void Postfix(ref bool __result, RecipeWorker __instance, Thing thing, BodyPartRecord part = null)
        {
            if (thing is Pawn pawn && pawn.IsAndroid() && __instance is Recipe_AdministerIngestible)
            {
                __result = false;
            }
        }
    }
}
