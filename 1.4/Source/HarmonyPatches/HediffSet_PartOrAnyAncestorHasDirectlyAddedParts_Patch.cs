using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffSet), "PartOrAnyAncestorHasDirectlyAddedParts")]
    public static class HediffSet_PartOrAnyAncestorHasDirectlyAddedParts_Patch
    {
        public static bool Prefix(HediffSet __instance, BodyPartRecord part, ref bool __result)
        {
            if (__instance.pawn.IsAndroid())
            {
                __result = __instance.HasDirectlyAddedPartFor(part);
                return false;
            }
            return true;
        }
    }
}
