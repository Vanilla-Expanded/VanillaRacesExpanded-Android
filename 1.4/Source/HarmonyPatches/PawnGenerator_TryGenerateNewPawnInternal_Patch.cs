using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "TryGenerateNewPawnInternal")]
    public static class PawnGenerator_TryGenerateNewPawnInternal_Patch
    {
        public static void Postfix(ref Pawn __result)
        {
            if (Rand.Chance(0.1f))
            {
                __result.health.AddHediff(VREA_DefOf.VREA_Android);
            }
        }
    }
}
