using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompRottable), "RotProgress", MethodType.Getter)]
    public static class CompRottable_RotProgress_Patch
    {
        public static bool Prefix(CompRottable __instance)
        {
            if (__instance.parent is Pawn pawn && pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
