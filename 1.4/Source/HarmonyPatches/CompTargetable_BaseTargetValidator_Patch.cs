using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompTargetable), "BaseTargetValidator")]
    public static class CompTargetable_BaseTargetValidator_Patch
    {
        public static void Postfix(CompTargetable __instance, Thing t, ref bool __result)
        {
            if (__result && __instance.Props.fleshCorpsesOnly && t is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
