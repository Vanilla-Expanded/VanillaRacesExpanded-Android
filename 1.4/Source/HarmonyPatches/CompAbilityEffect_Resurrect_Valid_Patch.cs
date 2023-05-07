using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompAbilityEffect_Resurrect), "Valid")]
    public static class CompAbilityEffect_Resurrect_Valid_Patch
    {
        public static void Postfix(LocalTargetInfo target, ref bool __result)
        {
            if (__result && target.Thing is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
