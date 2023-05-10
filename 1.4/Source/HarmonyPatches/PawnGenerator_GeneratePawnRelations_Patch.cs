using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawnRelations")]
    public static class PawnGenerator_GeneratePawnRelations_Patch
    {
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.Emotionless())
            {
                return false;
            }
            return true;
        }
    }
}
