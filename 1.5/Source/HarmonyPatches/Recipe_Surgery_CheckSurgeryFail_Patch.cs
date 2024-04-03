using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Recipe_Surgery), "CheckSurgeryFail")]
    public static class Recipe_Surgery_CheckSurgeryFail_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn surgeon, Pawn patient)
        {
            if (patient.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
