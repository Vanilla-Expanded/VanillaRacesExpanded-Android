using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Hediff_Injury), "BleedRate", MethodType.Getter)]
    public static class Hediff_Injury_BleedRate_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(ref float __result, Hediff_Injury __instance)
        {
            if (__instance.pawn.HasActiveGene(VREA_DefOf.VREA_NeutroCirculation))
            {
                __result = BleedRate(__instance);
                return false;
            }
            return true;
        }
        public static float BleedRate(Hediff_Injury __instance)
        {
            if (__instance.pawn.Dead)
            {
                return 0f;
            }
            if (__instance.IsTended() || __instance.IsPermanent())
            {
                return 0f;
            }
            float num = __instance.Severity * __instance.def.injuryProps.bleedRate;
            if (__instance.Part != null)
            {
                num *= __instance.Part.def.bleedRate;
            }
            return num;
        }
    }
}
