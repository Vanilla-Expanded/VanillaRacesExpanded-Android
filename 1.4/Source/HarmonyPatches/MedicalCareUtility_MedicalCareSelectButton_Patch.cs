using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MedicalCareUtility), "MedicalCareSelectButton")]
    public static class MedicalCareUtility_MedicalCareSelectButton_Patch
    {
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
