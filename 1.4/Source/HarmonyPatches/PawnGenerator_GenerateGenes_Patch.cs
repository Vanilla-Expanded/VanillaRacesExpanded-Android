using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "GenerateGenes")]
    public static class PawnGenerator_GenerateGenes_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(Pawn pawn)
        {
            curPawn = pawn;
        }

        public static void Postfix(Pawn pawn)
        {
            curPawn = null;
            if (pawn.IsAndroid())
            {
                pawn.needs.AddOrRemoveNeedsAsAppropriate();
            }
        }
    }
}
