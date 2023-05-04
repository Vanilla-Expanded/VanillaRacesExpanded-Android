using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompAbilityEffect_ReimplantXenogerm), "Valid")]
    public static class CompAbilityEffect_ReimplantXenogerm_Valid_Patch
    {
        public static void Postfix(ref bool __result, LocalTargetInfo target, bool throwMessages = false)
        {
            if (__result)
            {
                Pawn pawn = target.Pawn;
                if (pawn != null && pawn.IsAndroid())
                {
                    if (throwMessages)
                    {
                        Messages.Message("VREA.CannotImplantGenesOnAndroid".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    __result = false;
                }
            }
        }
    }
}
