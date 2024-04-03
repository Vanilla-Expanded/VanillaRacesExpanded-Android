using HarmonyLib;
using RimWorld;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(CompRevenant), "Hypnotize")]
    public static class CompRevenant_Hypnotize_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn victim)
        {
            if (victim.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
