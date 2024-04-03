using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnComponentsUtility), "AddAndRemoveDynamicComponents")]
    public static class PawnComponentsUtility_AddAndRemoveDynamicComponents
    {
        public static void Postfix(Pawn pawn)
        {
            if (pawn.IsAndroid() && pawn.playerSettings is not null)
            {
                pawn.playerSettings.medCare = MedicalCareCategory.NoMeds;
            }
        }
    }
}
