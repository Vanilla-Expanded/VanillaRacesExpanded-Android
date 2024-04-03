using HarmonyLib;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(LifeStageWorkSettings), "IsDisabled")]
    public static class LifeStageWorkSettings_IsDisabled_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn, ref bool __result)
        {
            if (pawn.IsAndroid())
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
