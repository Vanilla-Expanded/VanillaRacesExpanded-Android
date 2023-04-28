using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompAbilityEffect_Coagulate), "Valid")]
    public static class CompAbilityEffect_Coagulate_Valid_Patch
    {
        public static void Postfix(ref bool __result, LocalTargetInfo target, bool throwMessages = false)
        {
            if (__result)
            {
                Pawn pawn = target.Pawn;
                if (pawn != null && pawn.HasActiveGene(VREA_DefOf.VREA_SyntheticBody))
                {
                    if (throwMessages)
                    {
                        Messages.Message("VREA.CannotCoagulateOnAndroid".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    __result = false;
                }
            }
        }
    }
}
