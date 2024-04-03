using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "CannotDoBillDueToMedicineRestriction")]
    public static class WorkGiver_DoBill_CannotDoBillDueToMedicineRestriction_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref bool __result, IBillGiver giver)
        {
            if (giver is Pawn pawn && pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
