using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InteractionWorker_RomanceAttempt), "RandomSelectionWeight")]
    public static class InteractionWorker_RomanceAttempt_RandomSelectionWeight_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref float __result, Pawn initiator, Pawn recipient)
        {
            if (initiator.Emotionless())
            {
                __result = 0f;
            }
            else if (recipient.Emotionless())
            {
                __result = 0f;
            }
        }
    }
}
