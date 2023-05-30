using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(InteractionUtility), "CanInitiateRandomInteraction")]
    public static class InteractionUtility_CanInitiateRandomInteraction_Patch
    {
        public static Dictionary<Pawn, CachedResult<bool>> cachedResults = new();
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(Pawn p, ref bool __result)
        {
            if (__result)
            {
                if (!cachedResults.TryGetValue(p, out var cachedResult))
                {
                    cachedResults[p] = cachedResult = new CachedResult<bool>();
                    cachedResult.Result = p.Emotionless();
                }
                else if (cachedResult.CacheExpired)
                {
                    cachedResult.Result = p.Emotionless();
                }

                if (cachedResult.Result)
                {
                    __result = false;
                }
            }
        }
    }
}
