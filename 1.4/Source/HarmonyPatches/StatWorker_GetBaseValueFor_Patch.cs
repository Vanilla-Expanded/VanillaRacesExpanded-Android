using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(StatWorker), nameof(StatWorker.GetBaseValueFor))]
    public static class StatWorker_GetBaseValueFor_Patch
    {
        private static void Postfix(StatWorker __instance, StatRequest request, ref float __result)
        {
            if (__instance.stat == StatDefOf.ComfyTemperatureMin)
            {
                if (request.thingInt is Pawn pawn && pawn.IsAndroid())
                {
                    __result -= 10f;
                }
            }
            else if (__instance.stat == StatDefOf.ComfyTemperatureMax)
            {
                if (request.thingInt is Pawn pawn && pawn.IsAndroid())
                {
                    __result += 10f;
                }
            }
            else if (__instance.stat == StatDefOf.PsychicSensitivity)
            {
                if (request.thingInt is Pawn pawn && pawn.IsAndroid())
                {
                    __result = 0;
                }
            }
            else if (__instance.stat == StatDefOf.ToxicResistance)
            {
                if (request.thingInt is Pawn pawn && pawn.IsAndroid())
                {
                    __result = 1;
                }
            }
        }
    }


}
