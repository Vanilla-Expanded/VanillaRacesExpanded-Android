using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System.Text;
using System.Text.RegularExpressions;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(QuestNode_Root_MysteriousCargo), "ValidatePawn")]
    public static class QuestNode_Root_MysteriousCargo_ValidatePawn_Patch
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
