using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InteractionUtility), "CanInitiateInteraction")]
    public static class InteractionUtility_CanInitiateInteraction_Patch
    {
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (pawn.Emotionless())
            {
                __result = false;
            }
        }
    }
}
