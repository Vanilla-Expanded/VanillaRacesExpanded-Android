using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Hediff_Injury), "PainOffset", MethodType.Getter)]
    public static class Hediff_Injury_PainOffset_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Hediff_Injury __instance, ref float __result)
        {
            if (__instance.pawn.IsAndroid())
            {
                if (__instance.pawn.Dead || __instance.causesNoPain)
                {
                    __result = 0f;
                }
                else
                {
                    HediffComp_GetsPermanent hediffComp_GetsPermanent = __instance.TryGetComp<HediffComp_GetsPermanent>();
                    float num = ((hediffComp_GetsPermanent == null || !hediffComp_GetsPermanent.IsPermanent)
                        ? (__instance.Severity * __instance.def.injuryProps.painPerSeverity)
                        : (__instance.Severity * __instance.def.injuryProps.averagePainPerSeverityPermanent
                        * hediffComp_GetsPermanent.PainFactor));
                    __result = num / __instance.pawn.HealthScale;
                }
                return false;
            }
            return true;
        }
    }
}
