using HarmonyLib;
using RimWorld;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_StyleTracker), "CanDesireLookChange", MethodType.Getter)]
    public static class Pawn_StyleTracker_CanDesireLookChange_Patch
    {
        public static bool Prefix(Pawn_StyleTracker __instance, ref bool __result)
        {
            var gene = __instance.pawn.genes?.GetGene(VREA_DefOf.VREA_SyntheticBody) as Gene_SyntheticBody;
            if (gene != null && gene.Awakened is false)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
