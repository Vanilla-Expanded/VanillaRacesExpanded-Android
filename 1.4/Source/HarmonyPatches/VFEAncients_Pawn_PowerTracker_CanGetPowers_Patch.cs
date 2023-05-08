using HarmonyLib;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class VFEAncients_Pawn_PowerTracker_CanGetPowers_Patch
    {
        public static MethodInfo targetMethod;
        public static bool Prepare()
        {
            targetMethod = AccessTools.Method("VFEAncients.Pawn_PowerTracker:CanGetPowers");
            return targetMethod != null;
        }
        public static MethodBase TargetMethod()
        {
            return targetMethod;
        }

        public static void Postfix(Pawn pawn, ref bool __result)
        {
            if (pawn.IsAndroid())
            {
                __result = false;
            }
        }
    }
}
