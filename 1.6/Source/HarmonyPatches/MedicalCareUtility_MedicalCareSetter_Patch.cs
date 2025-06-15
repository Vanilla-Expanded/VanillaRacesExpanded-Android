using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MedicalCareUtility), "MedicalCareSetter")]
    public static class MedicalCareUtility_MedicalCareSetter_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix()
        {
            var pawn = ITab_Pawn_Visitor_FillTab_Patch.curPawn;
            if (pawn != null && pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
