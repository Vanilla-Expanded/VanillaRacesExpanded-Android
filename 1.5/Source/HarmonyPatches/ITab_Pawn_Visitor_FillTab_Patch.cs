using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ITab_Pawn_Visitor), "FillTab")]
    public static class ITab_Pawn_Visitor_FillTab_Patch
    {
        public static Pawn curPawn;
        public static void Prefix(ITab_Pawn_Visitor __instance)
        {
            curPawn = __instance.SelPawn;
        }

        public static void Postfix()
        {
            curPawn = null;
        }
    }
}
