using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HealthCardUtility), "CanDoRecipeWithMedicineRestriction")]
    public static class HealthCardUtility_CanDoRecipeWithMedicineRestriction_Patch
    {
        public static void Postfix(ref bool __result)
        {
            if (HealthCardUtility_CreateSurgeryBill_Patch.curPawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                __result = true;
            }
        }
    }
}
