using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffUtility), "CanHealFromTending")]
    public static class HediffUtility_CanHealFromTending_Patch
    {
        public static void Postfix(Hediff_Injury hd, ref bool __result)
        {
            if (hd.pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
