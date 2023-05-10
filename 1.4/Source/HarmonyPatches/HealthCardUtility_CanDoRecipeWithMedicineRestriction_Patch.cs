using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthCardUtility), "CanDoRecipeWithMedicineRestriction")]
    public static class HealthCardUtility_CanDoRecipeWithMedicineRestriction_Patch
    {
        public static void Postfix(ref bool __result, IBillGiver giver)
        {
            if (giver is Pawn pawn && pawn.IsAndroid())
            {
                __result = true;
            }
        }
    }
}
