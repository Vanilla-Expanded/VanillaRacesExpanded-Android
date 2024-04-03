using HarmonyLib;
using RimWorld;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(MutantUtility), "CanResurrectAsShambler")]
    public static class MutantUtility_CanResurrectAsShambler_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Corpse corpse)
        {
            if (corpse.InnerPawn != null && corpse.InnerPawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
