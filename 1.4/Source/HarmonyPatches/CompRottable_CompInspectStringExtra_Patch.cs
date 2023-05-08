using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompRottable), "CompInspectStringExtra")]
    public static class CompRottable_CompInspectStringExtra_Patch
    {
        public static bool Prefix(CompRottable __instance, ref string __result)
        {
            if (__instance.parent is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = null;
                return false;
            }
            return true;
        }
    }
}
