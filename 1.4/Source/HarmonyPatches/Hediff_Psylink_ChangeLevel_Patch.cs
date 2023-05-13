using HarmonyLib;
using System;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Hediff_Psylink), "ChangeLevel", new Type[]
    {
        typeof(int)
    })]
    public static class Hediff_Psylink_ChangeLevel_Patch
    {
        private static bool Prefix(Hediff_Psylink __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
