using HarmonyLib;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Dialog_InfoCard), "DoWindowContents")]
    public static class Dialog_InfoCard_DoWindowContents_Patch
    {
        public static void Prefix(Dialog_InfoCard __instance, Rect inRect)
        {
            if (__instance.thing is Building_AndroidSleepMode building)
            {
                __instance.thing = building.android;
            }
        }
    }
}
