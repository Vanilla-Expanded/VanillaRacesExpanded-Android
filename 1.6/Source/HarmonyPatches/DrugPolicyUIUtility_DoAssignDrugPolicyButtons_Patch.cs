using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(DrugPolicyUIUtility), "DoAssignDrugPolicyButtons")]
    public static class DrugPolicyUIUtility_DoAssignDrugPolicyButtons_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Rect rect, Pawn pawn)
        {
            return PawnColumnWorker_FoodRestriction_DoAssignFoodRestrictionButtons_Patch.ButtonOverride(rect, pawn);
        }
    }

}
