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
                if (p.IsAndroid())
                {
                    __result = bedDef == VREA_DefOf.VREA_NeutroCasket;
                }
                else
                {
                    __result = bedDef != VREA_DefOf.VREA_NeutroCasket;
                }
            }
        }
    }
}
