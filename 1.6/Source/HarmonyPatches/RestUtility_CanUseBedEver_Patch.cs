using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RestUtility), "CanUseBedEver")]
    public static class RestUtility_CanUseBedEver_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref bool __result, Pawn p, ThingDef bedDef)
        {
            if (__result && p.IsAndroid() is false && bedDef.IsAndroidBed())
            {
                __result = false;
            }
        }
    }
}
