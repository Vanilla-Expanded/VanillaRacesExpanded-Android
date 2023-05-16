using HarmonyLib;
using RimWorld;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_AgeTracker), "RecalculateLifeStageIndex")]
    public static class Pawn_AgeTracker_RecalculateLifeStageIndex_Patch
    {
        [HarmonyPriority(int.MaxValue)]
        public static bool Prefix(Pawn_AgeTracker __instance)
        {
            if (__instance.pawn.IsAndroid())
            {
                var lifestage = __instance.pawn.RaceProps.lifeStageAges
                    .FirstOrDefault(x => x.def == LifeStageDefOf.HumanlikeAdult || x.minAge >= 18 && x.minAge < float.MaxValue);
                int num = __instance.pawn.RaceProps.lifeStageAges.IndexOf(lifestage);
                __instance.growth = 1f;
                int index = __instance.cachedLifeStageIndex;
                bool num4 = __instance.cachedLifeStageIndex != num;
                __instance.cachedLifeStageIndex = num;
                if (!num4)
                {
                    return false;
                }
                __instance.lifeStageChange = true;
                if (!__instance.pawn.RaceProps.Humanlike || ModsConfig.BiotechActive)
                {
                    LongEventHandler.ExecuteWhenFinished(delegate
                    {
                        __instance.pawn.Drawer.renderer.graphics.SetAllGraphicsDirty();
                        if (__instance.pawn.IsColonist)
                        {
                            PortraitsCache.SetDirty(__instance.pawn);
                        }
                    });
                    __instance.CheckChangePawnKindName();
                }
                __instance.CurLifeStage.Worker.Notify_LifeStageStarted(__instance.pawn, __instance.GetLifeStageAge(index)?.def);
                if (__instance.pawn.SpawnedOrAnyParentSpawned)
                {
                    PawnComponentsUtility.AddAndRemoveDynamicComponents(__instance.pawn);
                }
                return false;
            }
            return true;
        }
    }
}
