using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(StatPart_Age), "ActiveFor")]
    public static class StatPart_Age_ActiveFor_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
