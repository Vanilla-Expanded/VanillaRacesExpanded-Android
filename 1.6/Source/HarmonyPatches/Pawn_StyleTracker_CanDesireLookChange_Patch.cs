using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_StyleTracker), "CanDesireLookChange", MethodType.Getter)]
    public static class Pawn_StyleTracker_CanDesireLookChange_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn_StyleTracker __instance, ref bool __result)
        {
            if (__instance.pawn.IsAndroid() && __instance.pawn.IsAwakened())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
