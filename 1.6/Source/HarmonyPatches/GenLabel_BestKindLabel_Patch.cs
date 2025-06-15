using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(GenLabel), "BestKindLabel", new Type[] {typeof(Pawn), typeof(bool), typeof(bool), typeof(bool), typeof(int) })]
    public static class GenLabel_BestKindLabel_Patch
    {
        public static void Prefix(Pawn pawn, ref bool mustNoteLifeStage)
        {
            if (mustNoteLifeStage && pawn.IsAndroid())
            {
                mustNoteLifeStage = false;
            }
        }
    }
}
