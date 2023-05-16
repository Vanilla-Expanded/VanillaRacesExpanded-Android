using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnColumnWorker_FoodRestriction), "DoAssignFoodRestrictionButtons")]
    public static class PawnColumnWorker_FoodRestriction_DoAssignFoodRestrictionButtons_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Rect rect, Pawn pawn)
        {
            return ButtonOverride(rect, pawn);
        }
        public static bool ButtonOverride(Rect rect, Pawn pawn)
        {
            if (pawn.IsAndroid())
            {
                var oldAnchor = Text.Anchor;
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = Color.grey;
                Widgets.Label(rect, "VREA.CannotSetNotLivingCreature".Translate());
                GUI.color = Color.white;
                Text.Anchor = oldAnchor;
                return false;
            }
            return true;
        }
    }

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
