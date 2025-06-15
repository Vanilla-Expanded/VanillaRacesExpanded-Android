using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompRottable), "Active", MethodType.Getter)]
    public static class CompRottable_Active_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(CompRottable __instance, ref bool __result)
        {
            if (__instance.parent is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
