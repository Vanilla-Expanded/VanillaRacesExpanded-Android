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
            if (ModsConfig.IsActive("Troopersmith1.DeathRattle"))
            {
                targetMethod = AccessTools.Method("DeathRattle.PawnCapacitiesHandler_Notify_CapacityLevelsDirty_Patch:Postfix");
                if (targetMethod != null)
                {
                    return true;
                }
                Log.Error("[VREAndroids] Failed to patch DeathRattle mod for PawnCapacitiesHandler_Notify_CapacityLevelsDirty_Patch");
            }
            return false;
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
