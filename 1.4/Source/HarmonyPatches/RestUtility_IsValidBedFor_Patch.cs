using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RestUtility), "IsValidBedFor")]
    public static class RestUtility_IsValidBedFor_Patch
    {
        public static void Postfix(ref bool __result, Thing bedThing, Pawn sleeper)
        {
            if (__result && bedThing != null && sleeper != null)
            {
                if (sleeper.HasActiveGene(VREA_DefOf.VREA_NeutroCirculation))
                {
                    if (bedThing?.def != VREA_DefOf.VREA_NeutroCasket)
                    {
                        __result = false;
                    }
                }
                else if (sleeper.IsAndroid())
                {
                    __result = false;
                }
                else if (bedThing?.def == VREA_DefOf.VREA_NeutroCasket)
                {
                    __result =  false;
                }
            }
        }
    }
}
