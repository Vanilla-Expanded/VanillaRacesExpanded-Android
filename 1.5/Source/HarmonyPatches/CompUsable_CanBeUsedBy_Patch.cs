using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompUseEffect_LearnSkill), "CanBeUsedBy")]
    public static class CompUseEffect_LearnSkill_CanBeUsedBy_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(CompUseEffect_LearnSkill __instance, ref AcceptanceReport __result, Pawn p)
        {
            if (p.IsAndroid())
            {
                __result = "VREA.CannotUseAndroid".Translate();
            }
        }
    }
}
