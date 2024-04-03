using HarmonyLib;
using System.Reflection;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class DeathRattle_PawnCapacitiesHandler_Notify_CapacityLevelsDirty_Patch
    {
        public static MethodBase targetMethod;
        public static bool Prepare()
        {
            targetMethod = AccessTools.Method("DeathRattle.PawnCapacitiesHandler_Notify_CapacityLevelsDirty_Patch:Notify_CapacityLevelsDirty_Postfix");
            return targetMethod != null;
        }

        public static MethodBase TargetMethod() => targetMethod;

        public static bool Prefix(Pawn __1)
        {
            if (__1.IsAndroid())
            {
                return false;
            }
            return true;
        }
    }
}
