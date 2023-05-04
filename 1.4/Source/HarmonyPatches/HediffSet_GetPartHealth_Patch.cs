using HarmonyLib;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffSet), "GetPartHealth")]
    public static class HediffSet_GetPartHealth_Patch
    {
        public static bool Prefix(HediffSet __instance, ref float __result, BodyPartRecord part) 
        {
            if (__instance.pawn.IsAndroid())
            {
                __result = GetPartHealth(__instance, part);
                return false;
            }
            return true;
        }

        public static float GetPartHealth(HediffSet __instance, BodyPartRecord part)
        {
            if (part == null)
            {
                return 0f;
            }
            float num = part.def.GetMaxHealth(__instance.pawn);
            for (int i = 0; i < __instance.hediffs.Count; i++)
            {
                if (__instance.hediffs[i] is Hediff_MissingPart && __instance.hediffs[i].Part == part)
                {
                    return 0f;
                }
                if (__instance.hediffs[i].Part == part)
                {
                    Hediff_Injury hediff_Injury = __instance.hediffs[i] as Hediff_Injury;
                    if (hediff_Injury != null)
                    {
                        num -= hediff_Injury.Severity;
                    }
                }
            }
            num = Mathf.Max(num, 0f);
            return Mathf.RoundToInt(num);
        }
    }
}
