using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnAt")]
    public static class PawnRenderer_RenderPawnAt_Patch
    {
        public static void Prefix(Pawn ___pawn)
        {
            PawnUtility_GetPosture_Patch.isPawnRendering = true;
        }
        public static void Postfix(Pawn ___pawn)
        {
            PawnUtility_GetPosture_Patch.isPawnRendering = false;
        }
    }
}
