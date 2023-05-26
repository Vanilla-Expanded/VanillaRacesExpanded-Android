using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffSet), "HasDirectlyAddedPartFor")]
    public static class HediffSet_HasDirectlyAddedPartFor_Patch
    {
        public static bool Prefix(HediffSet __instance, BodyPartRecord part, ref bool __result)
        {
            if (__instance.pawn.IsAndroid())
            {
                __result = HasDirectlyAddedPartFor(__instance, part);
                return false;
            }
            return true;
        }
    
        public static bool HasDirectlyAddedPartFor(HediffSet __instance, BodyPartRecord part)
        {
            for (int i = 0; i < __instance.hediffs.Count; i++)
            {
                if (__instance.hediffs[i].Part == part && __instance.hediffs[i] is Hediff_AddedPart)
                {
                    if (__instance.hediffs[i] is not Hediff_AndroidPart)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
