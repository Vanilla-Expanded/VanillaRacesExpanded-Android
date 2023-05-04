using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RestUtility), "CanUseBedEver")]
    public static class RestUtility_CanUseBedEver_Patch
    {
        public static void Postfix(ref bool __result, Pawn p, ThingDef bedDef)
        {
            if (__result)
            {
                if (p.HasActiveGene(VREA_DefOf.VREA_NeutroCirculation))
                {
                    if (bedDef != VREA_DefOf.VREA_NeutroCasket)
                    {
                        __result = false;
                    }
                }
                else if (p.IsAndroid())
                {
                    __result = false;
                }
                else if (bedDef == VREA_DefOf.VREA_NeutroCasket)
                {
                    __result = false;
                }
            }
        }
    }
}
