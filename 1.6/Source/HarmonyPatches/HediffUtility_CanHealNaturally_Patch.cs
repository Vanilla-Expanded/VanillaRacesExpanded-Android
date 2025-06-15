using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(HediffUtility), "CanHealNaturally")]
    public static class HediffUtility_CanHealNaturally_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Hediff_Injury hd, ref bool __result)
        {
            if (hd.pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
