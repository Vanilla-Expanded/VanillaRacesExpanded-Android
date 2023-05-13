using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class CompAbilityEffect_BloodfeederBite_Valid_Patch
    {
        public static bool VanillaRacesExpandedSanguophageActive = ModsConfig.IsActive("vanillaracesexpanded.sanguophage");
        
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.DeclaredMethod(typeof(CompAbilityEffect_BloodfeederBite), "Valid");
            if (VanillaRacesExpandedSanguophageActive)
            {
                yield return AccessTools.DeclaredMethod("VanillaRacesExpandedSanguophage.CompAbilityEffect_SanguofeederBite:Valid");
                yield return AccessTools.DeclaredMethod("VanillaRacesExpandedSanguophage.CompAbilityEffect_CorpsefeederBite:Valid");
            }
        }

        public static void Postfix(ref bool __result, LocalTargetInfo target, bool throwMessages = false)
        {
            if (__result)
            {
                if (target.Thing is Pawn pawn && pawn.IsAndroid())
                {
                    if (throwMessages)
                    {
                        Messages.Message("VREA.CannotFeedOnAndroid".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    __result = false;
                }
                else if (target.Thing is Corpse corpse && corpse.InnerPawn.IsAndroid())
                {
                    if (throwMessages)
                    {
                        Messages.Message("VREA.CannotFeedOnAndroid".Translate(corpse.InnerPawn.Named("PAWN")), corpse, MessageTypeDefOf.RejectInput, historical: false);
                    }
                    __result = false;
                }
            }
        }
    }
}
