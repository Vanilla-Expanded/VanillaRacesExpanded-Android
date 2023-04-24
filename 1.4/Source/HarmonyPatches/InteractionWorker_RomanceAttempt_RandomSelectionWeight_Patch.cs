using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "RandomSelectionWeight")]
    public static class InteractionWorker_RomanceAttempt_RandomSelectionWeight_Patch
    {
        public static void Postfix(ref float __result, Pawn initiator, Pawn recipient)
        {
            if (initiator.IsAndroid(out var state) && state != AndroidState.Awakened)
            {
                __result = 0f;
            }
            else if (recipient.IsAndroid(out var state2) && state2 != AndroidState.Awakened)
            {
                __result = 0f;
            }
        }
    }
}
