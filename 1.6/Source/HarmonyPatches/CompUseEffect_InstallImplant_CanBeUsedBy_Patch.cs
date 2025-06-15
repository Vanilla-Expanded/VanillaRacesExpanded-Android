using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompUseEffect_InstallImplant), "CanBeUsedBy")]
    public static class CompUseEffect_InstallImplant_CanBeUsedBy_Patch
    {
        public static void Prefix(CompUseEffect_InstallImplant __instance, Pawn p, out bool __state)
        {
            __state = __instance.Props.requiresPsychicallySensitive;
            if (__instance.Props.hediffDef == HediffDefOf.MechlinkImplant && p.HasActiveGene(VREA_DefOf.VREA_MechlinkSupport))
            {
                __instance.Props.requiresPsychicallySensitive = false;
            }
        }

        public static void Postfix(CompUseEffect_InstallImplant __instance, bool __state)
        {
            __instance.Props.requiresPsychicallySensitive = __state;
        }
    }
}
