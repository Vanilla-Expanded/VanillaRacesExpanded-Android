using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RestUtility), "IsValidBedFor")]
    public static class RestUtility_IsValidBedFor_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref bool __result, Thing bedThing, Pawn sleeper)
        {
            if (__result && bedThing != null && sleeper != null)
            {
                if (sleeper.IsAndroid() is false && bedThing?.def.IsAndroidBed() is true)
                {
                    __result = false;
                }
            }
        }
    }
}
