using HarmonyLib;
using RimWorld;
using Verse;
using System;


namespace VREAndroids
{
    [HarmonyPatch(typeof(TaleUtility), "Notify_PawnDied")]
    public static class TaleUtility_Notify_PawnDied_Patch_Patch
    {
        public static void Postfix(Pawn victim, DamageInfo? dinfo)
        {
            if (ModsConfig.IdeologyActive && Utils.IsAndroid(victim)) {

                Pawn pawn = dinfo?.Instigator as Pawn;
                if (pawn != null)
                {
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(VREA_DefOf.VRE_AndroidDied, new SignalArgs(pawn.Named(HistoryEventArgsNames.Doer))), true);
                }
                else
                {
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(VREA_DefOf.VRE_AndroidDied));
                }

            }
            


        }
    }
}
