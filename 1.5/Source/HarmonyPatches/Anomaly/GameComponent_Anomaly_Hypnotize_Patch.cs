using HarmonyLib;
using RimWorld;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(GameComponent_Anomaly), "Hypnotize")]
    public static class GameComponent_Anomaly_Hypnotize_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn pawn)
        {
            if (pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
