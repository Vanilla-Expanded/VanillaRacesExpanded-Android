using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(RestUtility), "FindBedFor",
    new Type[] { typeof(Pawn), typeof(Pawn), typeof(bool), typeof(bool), typeof(GuestStatus?) },
    new ArgumentType[] { ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal })]
    public static class RestUtility_FindBedFor_Patch
    {
        public static void Prefix(Pawn sleeper, Pawn traveler, out List<ThingDef> __state)
        {
            if (sleeper.IsAndroid())
            {
                __state = RestUtility.bedDefsBestToWorst_Medical.ListFullCopy();
                RestUtility.bedDefsBestToWorst_Medical.RemoveAll(x => x == VREA_DefOf.VREA_NeutroCasket);
                RestUtility.bedDefsBestToWorst_Medical.Insert(0, VREA_DefOf.VREA_NeutroCasket);
            }
            else
            {
                __state = RestUtility.bedDefsBestToWorst_Medical;
            }
        }
        public static void Postfix(List<ThingDef> __state)
        {
            RestUtility.bedDefsBestToWorst_Medical = __state;
        }
    }
}
