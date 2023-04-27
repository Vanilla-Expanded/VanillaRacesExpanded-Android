using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), "JobOnThing")]
    public static class WorkGiver_DoBill_JobOnThing_Patch
    {
        public static void Prefix(Pawn pawn, Thing thing, bool forced = false)
        {
            if (thing is Pawn pawn2 && pawn2.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
            {
                if (pawn2.playerSettings is not null)
                {
                    pawn2.playerSettings.medCare = MedicalCareCategory.NoMeds;
                }
            }
        }
    }
}
