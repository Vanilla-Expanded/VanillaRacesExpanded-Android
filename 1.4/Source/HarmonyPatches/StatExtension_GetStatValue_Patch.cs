using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(StatExtension), nameof(StatExtension.GetStatValue))]
    public static class StatExtension_GetStatValue_Patch
    {
        private static void Postfix(Thing thing, StatDef stat, bool applyPostProcess, ref float __result)
        {
            if (stat == StatDefOf.PsychicSensitivity)
            {
                if (thing is Pawn pawn && pawn.IsAndroid())
                {
                    __result = 0;
                }
            }
            else if (stat == StatDefOf.ToxicResistance)
            {
                if (thing is Pawn pawn && pawn.IsAndroid())
                {
                    __result = 1f;
                }
            }
        }
    }
}
