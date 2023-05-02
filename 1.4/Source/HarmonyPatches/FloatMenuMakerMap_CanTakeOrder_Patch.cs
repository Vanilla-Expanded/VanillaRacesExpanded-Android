using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(FloatMenuMakerMap), "CanTakeOrder")]
    public static class FloatMenuMakerMap_CanTakeOrder_Patch
    {
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (__result && pawn.HasActiveGene(VREA_DefOf.VREA_Uncontrollable))
            {
                __result = false;
            }
        }
    }
}
