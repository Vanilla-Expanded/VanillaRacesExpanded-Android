using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_DraftController), "Drafted", MethodType.Setter)]
    public static class Pawn_DraftController_Drafted_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        private static bool Prefix(Pawn_DraftController __instance, ref bool value)
        {
            if (__instance.pawn.HasActiveGene(VREA_DefOf.VREA_Uncontrollable))
            {
                value = false;
                return false;
            }
            return true;
        }
    }
}
