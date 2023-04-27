using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(JoyGiver_Meditate), "TryGiveJob")]
    public static class JoyGiver_Meditate_Patch
    {
        public static void Postfix(ref Job __result, Pawn pawn)
        {
            if (pawn.HasActiveGene(VREA_DefOf.VREA_JoyDisabled))
            {
                __result = null;
            }
        }
    }
}
