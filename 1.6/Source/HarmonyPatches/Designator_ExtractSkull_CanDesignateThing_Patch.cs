using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Designator_ExtractSkull), "CanDesignateThing")]
    public static class Designator_ExtractSkull_CanDesignateThing_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref AcceptanceReport __result, Thing t)
        {
            if (__result && t is Corpse corpse && corpse.InnerPawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
