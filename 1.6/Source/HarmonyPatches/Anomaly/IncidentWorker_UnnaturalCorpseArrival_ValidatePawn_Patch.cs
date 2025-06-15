using HarmonyLib;
using RimWorld;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(IncidentWorker_UnnaturalCorpseArrival), "ValidatePawn")]
    public static class IncidentWorker_UnnaturalCorpseArrival_ValidatePawn_Patch
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
