using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompTargetable), "ValidateTarget")]
    public static class CompTargetable_ValidateTarget_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(CompTargetable __instance, LocalTargetInfo target, ref bool __result)
        {
            if (__result && __instance.Props.fleshCorpsesOnly && target.HasThing && target.Thing is Corpse corpse 
                && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
